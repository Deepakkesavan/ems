using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GlobalExceptionHandler.Models;
using Microsoft.Extensions.Configuration;
using EmpInfoInfra.Models;
using EmpInfoInfra.CatalogModels;
using EmpInfoService.Model;
using Microsoft.AspNetCore.Http;
using EmpInfoService.Constant;

namespace EmpInfoService.Common
{
    public class CommonService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CommonService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<UserDto> GetUserDtoAsync()

        {
            var user = _httpContextAccessor.HttpContext.User;
            if (user.Identity?.IsAuthenticated ?? false)
            {
                var empIdClaim = user.FindFirst("empId")?.Value;
                var designationClaim = user.FindFirst("designation")?.Value;

                return new UserDto
                {
                    EmpId = empIdClaim ?? "Unknown",
                    Designation = designationClaim ?? "Unknown",
                };
            }

            throw new Exception("Session expired or user not authenticated");

        }
       
        public async Task<List<Designation>> GetDesignationAsync()
        {
            string baseUrl = _configuration["URL:CacheUrl"]!;
            string url = $"{baseUrl}/CatalogCache/designation";
            //if (job == true) url = $"{baseUrl}/CatalogCache/designation";

            var client = _httpClientFactory.CreateClient("DesignationApiClient");
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Designation API call failed: {response.ReasonPhrase}");
            }
            var result = await response.Content.ReadFromJsonAsync<BaseResponse<List<Designation>>>();

            return result?.Result ?? new List<Designation>();

        }

        public async Task<List<Permission>> GetPermissionAsync()
        {
            string baseUrl = _configuration["URL:CacheUrl"]!;
            string url = $"{baseUrl}/CatalogCache/permission";
            //if (job == true) url = $"{baseUrl}/CatalogCache/designation";

            var client = _httpClientFactory.CreateClient("PermissionApiClient");
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Permission API call failed: {response.ReasonPhrase}");
            }
            var result = await response.Content.ReadFromJsonAsync<BaseResponse<List<Permission>>>();

            return result?.Result ?? new List<Permission>();
        }

        public async Task<Dictionary<string, string>> GetModulesConfigAsync()
        {
            return await FetchConfig(BussinessConstant.MODULES_URI);
        }

        public async Task<Dictionary<string, string>> GetConnectionStringConfigAsync()
        {
            return await FetchConfig(BussinessConstant.CONNECTION_STRING_URI);
        }

        private async Task<Dictionary<string, string>> FetchConfig(string uri)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            string? serviceUrl = _configuration["Config_Url"];
            if (String.IsNullOrEmpty(serviceUrl))
            {
                throw new InvalidOperationException("Missing Config Url from environment configuration");
            }
            string url = serviceUrl + uri;
            HttpResponseMessage response = await client.PostAsJsonAsync(url, new Dictionary<string, string>());
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Dictionary<string, string>>() ?? [];
        }
    }
}
