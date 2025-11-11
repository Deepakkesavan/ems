using System.Linq.Dynamic.Core;
using EmpInfoService.Model;
using EmployeeInformationInner.Services.Interfaces;
using GlobalExceptionHandler.Exceptions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;


namespace EmpInfoService.Services.ServiceImpl
{
    public class EmployeeDetails : IEmployeeDetails
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<EmployeeDetails> _logger;
        //Semaphore to restrict concurrent access to GetAllEmployees
        //it allows only one user at a time
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public EmployeeDetails(IMemoryCache cache, ILogger<EmployeeDetails> logger)
        {
            _cache = cache;
            _logger = logger;
        }
        public List<EmployeeDto> GetEmployeeDetailsFromCache()
        {
            //Gets the data from in-memory cache
            if (_cache.TryGetValue("employees", out List<EmployeeDto> employees))
            {
                _logger.LogInformation("Employee data retrieved from in-memory cache.");
               
                return employees;
            }
            else
            {
                _logger.LogError("Employee data not found in in-memory cache.");
                throw new CacheNotFoundException("employees");

            }
        }
        public List<EmployeeDto> GetAllEmployeesFromCache(string query)
        {
            //Gets the data from in-memory cache
            if (_cache.TryGetValue("employees", out List<EmployeeDto> employees))
            {
                _logger.LogInformation("Employee data retrieved from in-memory cache.");
                //based on query filters the data from cache and returns the result
                var result = employees.AsQueryable().Where(query).ToList();
                return employees;
            }
            else
            {
                _logger.LogError("Employee data not found in in-memory cache.");
                throw new CacheNotFoundException("employees");
              
            }
        }
        //Gets the data from shared cache(EmployeeCacheManager)
        //Semaphore prevents concurrent access
        //public async Task<List<EmployeeDto>> GetAllEmployees(string project_name)
        //{
        //    await _semaphore.WaitAsync();
        //    try
        //    {
        //        //If the cache data is available in shared cache it will retrive from there
        //        //or else it will get the data from in-memory cache for every 10 mins
        //        DataCaching dataCaching = new DataCaching();
        //        var response = dataCaching.DataCache(project_name);
        //        if(response== null)
        //            throw new InvalidOperationException($"DataCache returned null for project: {project_name}");
        //        var result = EmployeeCacheManager.GetOrCreate(
        //        response.CacheKey,
        //       () => GetAllEmployeesFromCache(response.Query)
        //       // Optional custom expiration: TimeSpan.FromMinutes(2)
        //       //,TimeSpan.FromMinutes(2)
        //     );               
        //        return result;
        //    }
        //    finally
        //    {
        //        _semaphore.Release();
        //    }
        //}
    }
}
