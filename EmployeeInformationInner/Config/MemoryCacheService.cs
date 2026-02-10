using EmpInfoService.Common;
using EmpInfoService.Constant;
using Microsoft.Extensions.Caching.Memory;

namespace EmpInfoInner.Config
{
    public class MemoryCacheService
    {
        private readonly IMemoryCache _cache;
        private readonly CommonService _commonService;
        public MemoryCacheService(IMemoryCache cache, CommonService commonService)
        {
            _cache = cache;
            _commonService = commonService;
        }

        public async Task<string> GetModuleName(string key)
        {
            if (!_cache.TryGetValue(BussinessConstant.MODULES_CACHE_KEY, out Dictionary<string, string>? dict))
            {
                dict = await _commonService.GetModulesConfigAsync();
                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(24));
                _cache.Set(BussinessConstant.MODULES_CACHE_KEY, dict, cacheOptions);
            }
            return dict![key];
        }

        public async Task<string> GetConnectionString(string key)
        {
            if (!_cache.TryGetValue(BussinessConstant.CONNECTION_STRING_URI, out Dictionary<string, string>? dict))
            {
                dict = await _commonService.GetConnectionStringConfigAsync();
                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(24));
                _cache.Set(BussinessConstant.CONNECTION_STRING_CACHE_KEY, dict, cacheOptions);
            }
            return dict![key];
        }
    }
}
