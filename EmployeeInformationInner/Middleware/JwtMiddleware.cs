using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmpInfoInner.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtMiddleware> _logger;
        private readonly string _jwtSecret;

        public JwtMiddleware(RequestDelegate next, ILogger<JwtMiddleware> logger, string jwtSecret = "@290200muiralcssocustomlogin!1216")
        {
            _next = next;
            _logger = logger;
            _jwtSecret = jwtSecret;
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.LogInformation("Incoming request: {Path}", context.Request.Path.Value);

            var path = context.Request.Path.Value;

            if (path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            try
            {
                // Get JWT from cookie or Authorization header
                var jwtToken = context.Request.Cookies["JWT"];
                UserDto user = null;

                if (!string.IsNullOrEmpty(jwtToken))
                {
                    user = await ValidateJwtWithRefresh(context, jwtToken);
                }
                else if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    if (authHeader.ToString().StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        var bearerToken = authHeader.ToString().Substring("Bearer ".Length).Trim();
                        user = await ValidateJwtWithRefresh(context, bearerToken);
                    }
                }

                // If JWT missing, try refresh token
                if (user == null)
                {
                    var refreshToken = context.Request.Cookies["REFRESH_TOKEN"];
                    if (!string.IsNullOrEmpty(refreshToken))
                    {
                        _logger.LogInformation("JWT missing or expired. Attempting refresh with refresh token...");
                        user = await RefreshJwtUsingRefreshToken(context, refreshToken);
                    }
                }

                // Still null → throw
                if (user == null)
                    throw new UnauthorizedAccessException("No valid JWT or refresh token found.");

                // Attach user
                context.Items["User"] = user;

                var claims = new List<Claim>
                {
                    new Claim("empId", user.EmpId.ToString()),
                    new Claim("designation", user.Designation)
                };

                var identity = new ClaimsIdentity(claims, "JWT");
                context.User = new ClaimsPrincipal(identity);

                _logger.LogInformation("User attached: EmpId={EmpId}, Designation={Designation}", user.EmpId, user.Designation);

                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "JWT Middleware error: {Message}", ex.Message);
                await RespondUnauthorized(context, ex.Message);
            }
        }

        // ------------------ JWT Validation with Refresh ------------------
        private async Task<UserDto> ValidateJwtWithRefresh(HttpContext context, string jwtToken)
        {
            try
            {
                return ValidateJwtLocally(jwtToken, context);
            }
            catch (SecurityTokenExpiredException)
            {
                _logger.LogWarning("JWT expired. Attempting refresh...");

                var refreshToken = context.Request.Cookies["REFRESH_TOKEN"];
                if (string.IsNullOrEmpty(refreshToken))
                    throw new UnauthorizedAccessException("Refresh token missing in cookie.");

                var refreshedUser = await RefreshJwtUsingRefreshToken(context, refreshToken);
                if (refreshedUser == null)
                    throw new UnauthorizedAccessException("Unable to refresh JWT. Session expired.");

                return refreshedUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "JWT validation failed.");
                throw new UnauthorizedAccessException($"Invalid JWT: {ex.Message}");
            }
        }

        // ------------------ Refresh JWT using refresh token ------------------
        private async Task<UserDto> RefreshJwtUsingRefreshToken(HttpContext context, string refreshToken)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Cookie", $"REFRESH_TOKEN={refreshToken}");

            var response = await httpClient.PostAsync("http://localhost:8080/api/auth/refresh-token", null);
            if (!response.IsSuccessStatusCode)
                throw new UnauthorizedAccessException($"Refresh token request failed with status {response.StatusCode}");

            var result = await response.Content.ReadFromJsonAsync<RefreshResponse>();
            if (result == null)
                throw new UnauthorizedAccessException("Refresh token API returned null response.");

            if (string.IsNullOrEmpty(result.Token))
                throw new UnauthorizedAccessException("Refresh token API returned empty token.");

            var validatedUser = ValidateJwtLocally(result.Token, context);
            if (validatedUser == null)
                throw new UnauthorizedAccessException("Refreshed JWT could not be validated.");

            _logger.LogInformation("JWT successfully refreshed for EmpId={EmpId}", validatedUser.EmpId);
            return validatedUser;
        }

        // ------------------ Local JWT Validation ------------------
        private UserDto ValidateJwtLocally(string jwtToken, HttpContext context)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSecret);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(jwtToken, validationParameters, out var validatedToken);
            if (validatedToken is not JwtSecurityToken)
                throw new UnauthorizedAccessException("Invalid JWT token structure.");

            var empIdClaim = principal.FindFirst("empId")?.Value;
            var designationClaim = principal.FindFirst("designation")?.Value;

            if (string.IsNullOrEmpty(empIdClaim))
                throw new UnauthorizedAccessException("empId claim missing in JWT.");

            if (!int.TryParse(empIdClaim, out var empId))
                throw new UnauthorizedAccessException($"Invalid empId value: {empIdClaim}");

            return new UserDto
            {
                EmpId = empId,
                Designation = designationClaim ?? "Unknown",
                Authenticated = true
            };
        }

        // ------------------ Respond Unauthorized ------------------
        private async Task RespondUnauthorized(HttpContext context, string message)
        {
            if (!context.Response.HasStarted)
            {
                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { error = message });
            }
        }
    }

    // ------------------ DTOs ------------------
    public class UserDto
    {
        public int EmpId { get; set; }
        public string Designation { get; set; }
        public bool Authenticated { get; set; }
    }

    public class RefreshResponse
    {
        public string Token { get; set; }
        public UserDto User { get; set; }
    }
}
