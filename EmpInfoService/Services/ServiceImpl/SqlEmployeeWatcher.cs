
using EmpInfoInner;
using EmpInfoService.Model;
using GlobalExceptionHandler.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace EmpInfoService.Services.ServiceImpl
{
    public class SqlEmployeeWatcher
    {
        private readonly string _connStr;
        private readonly IMemoryCache _cache;
        private readonly ILogger<SqlEmployeeWatcher> _logger;

        public SqlEmployeeWatcher(IConfiguration configuration, IMemoryCache cache, ILogger<SqlEmployeeWatcher> logger)
        {

            _connStr = configuration.GetConnectionString("DefaultConnection") ??throw new DataNotFoundException("Connection string not found");
            //if (string.IsNullOrEmpty(_connStr))
            //    throw new DataNotFoundException("Connection String not found");
            _cache = cache;
            _logger = logger;
            //Starting SqlDependency...
            _logger.LogInformation("Starting SqlDependency");
            SqlDependency.Start(_connStr);
            //register for changes
            RegisterEmployeeDependency();
        }
        private void RegisterEmployeeDependency()
        {
            List<EmployeeDto> employees = new();
            using (var connection = new SqlConnection(_connStr))
            {
                var command = new SqlCommand(ApplicationConstants.EmployeeDetails, connection); // simple, static query
                var dependency = new SqlDependency(command);
                dependency.OnChange += OnEmployeeChange;

                connection.Open();
                // Execute query and cache the results
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    employees.Add(new EmployeeDto
                    {
                        EmpGuid = (Guid)reader["EmpGuid"],
                        EmpId = (string)reader["EmpID"],
                        FirstName = reader["FirstName"]?.ToString() ?? string.Empty,
                        LastName = reader["LastName"]?.ToString() ?? string.Empty,
                        Email = reader["Email"]?.ToString()??string.Empty,
                        PersonalPhoneNumber = reader["PersonalPhoneNumber"]?.ToString()??string.Empty,
                        DesgGuid = (Guid?)reader["DesgGuid"],
                        DesgId = (int)reader["DesgID"],
                        Desg = reader["Desg"]?.ToString() ?? string.Empty,
                        ProjGuid = (Guid?)reader["ProjGuid"],
                        ProjId = (int)reader["ProjID"],
                        Project = reader["Project"]?.ToString()??string.Empty,
                        Gender = reader["Gender"]?.ToString()??string.Empty,
                        ManagerEmpCode = reader["ManagerEmpCode"]?.ToString()??string.Empty

                        //EmailAddress = reader["email_address"]?.ToString() ?? string.Empty,
                        //City = reader["city"]?.ToString() ?? string.Empty
                    });
                }
                if (!(employees.Count > 0))
                {
                    throw new DataNotFoundException("Employee Data is not there in in-memory cache");
                }
                //set the cache data with the cachekey
                _cache.Set("employees", employees);
              
            }
        }
     


        /// <summary>
        /// Called when a change is detected in the monitored table/query
        /// this checks for the changes in db and removes the cachekey
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEmployeeChange(object sender, SqlNotificationEventArgs e)
        {
            //checks for changes in db and deletes the cache
            if (e.Type == SqlNotificationType.Change)
            {
                _logger.LogInformation("Detected change in user_details table. Updating cache...");
                _cache.Remove("employees");
                // 3. Re-register for future changes
                RegisterEmployeeDependency();
            }
            else
            {
                _logger.LogInformation($"SqlDependency not triggered correctly. Type={e.Type}, Info={e.Info}, Source={e.Source}");
            }
        }
      

    }
}
