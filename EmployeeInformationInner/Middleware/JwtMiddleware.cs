using EmpInfoInner.Config;
using EmpInfoService.Constant;
using EmpInfoService.Model;
using GlobalExceptionHandler.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace EmpInfoInner.Middleware
{
    
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _jwtSecret;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MemoryCacheService _memoryCacheService;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration, IHttpClientFactory httpClientFactory, MemoryCacheService memoryCacheService)
        {
            _next = next;
            _jwtSecret = configuration["JWT_SECRET"] ?? throw new InvalidOperationException("JWT Secret missing from configuration.");
            _httpClientFactory = httpClientFactory;
            _memoryCacheService = memoryCacheService;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value ?? "";

            // Skip JWT validation for swagger/public paths
            if (path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            UserDto? user = null;
            string? bearerToken = null;

            // Try JWT from Authorization header
            if (user == null && context.Request.Headers.TryGetValue("Authorization", out var authHeader) &&
                authHeader.ToString().StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                bearerToken = authHeader.ToString()["Bearer ".Length..].Trim();
                user = ValidateJwtLocally(bearerToken);
                if (user != null) context.Items["JWT"] = bearerToken;

            }

            // 3. If validation failed but we have a token, try to refresh
            if (user == null && !string.IsNullOrEmpty(bearerToken))
            {
                user = await RefreshJwtUsingRefreshToken(context, bearerToken);
            }

            // 4. If still null → return 401
            if (user == null) {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Invalid or expired token.");
                return;
            }

            // 5. Attach user info to HttpContext
            context.Items["User"] = user;
            context.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new(CustomClaimTypes.EmpId, user.EmpId),
                        new(CustomClaimTypes.Designation, user.Designation)
                    }, "JWT"));

            await _next(context);
        }

        private async Task<UserDto?> RefreshJwtUsingRefreshToken(HttpContext context, string expiredToken)
        {
            string url = await _memoryCacheService.GetModuleName(BussinessConstant.SSO_MODULE_NAME) + "/api/internal/refresh";
            HttpClient httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", expiredToken);

            var response = await httpClient.PostAsJsonAsync(url, new { });

            if (!response.IsSuccessStatusCode) return null;

            BaseResponse<RefreshResponse>? result =  await response.Content.ReadFromJsonAsync<BaseResponse<RefreshResponse>>();
            if (result == null ||
                 result.Result?.Token == null ||
                 (result.Errors != null && result.Errors.Any()))
            {
                // Log errors if necessary: unifiedResponse.Errors
                return null;
            }
            String newJwt = result.Result.Token;
            if (string.IsNullOrEmpty(newJwt))
            {
                return null;
            }
            UserDto? user = ValidateJwtLocally(newJwt);
            if (user != null)
            {
                context.Items["JWT"] = newJwt;

                // IMPORTANT: Also update the Authorization header for downstream services
                context.Request.Headers.Authorization = $"Bearer {newJwt}";
            }


            return user;
        }

        private UserDto? ValidateJwtLocally(string jwtToken)
        {
            if (string.IsNullOrWhiteSpace(jwtToken))
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            
            var key = Encoding.UTF8.GetBytes(_jwtSecret);

            var validationParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.FromMinutes(0)
            };

            try
            {
                var principal = tokenHandler.ValidateToken(jwtToken, validationParams, out var validatedToken);

                if (validatedToken is not JwtSecurityToken jwt ||
                    !jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    return null;

                var empIdClaim = principal.FindFirst(CustomClaimTypes.EmpId)?.Value;
                var designationClaim = principal.FindFirst(CustomClaimTypes.Designation)?.Value;


                return new UserDto
                {
                    EmpId = empIdClaim ?? "Unknown",
                    Designation = designationClaim ?? "Unknown",

                };
            }
            catch
            {
                return null;
            }
        }
    }

    public class RefreshResponse
    {
        public string Token { get; set; }= string.Empty;
        public User User { get; set; } = new();
    }

    public class User
    {
        public bool Authenticated { get; set; }
        public int EmpId { get; set; }
        public string Designation { get; set; } = string.Empty;
        public UserAttributes UserAttributes { get; set; } = new();
    }

    public class UserAttributes
    {
        public string GivenName { get; set; } = string.Empty;
        public string FamilyName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public string Picture { get; set; } = string.Empty;
    }   



    public static class CustomClaimTypes
    {
        public const string EmpId = "empId";
        public const string Designation = "designation";
    }
}
