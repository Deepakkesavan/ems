
using EmpInfoInfra.ConncectionStrings;
using EmpInfoInner;
using EmpInfoService.Model;
using GlobalExceptionHandler.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace EmpInfoService.Services.ServiceImpl
{
    public class SqlEmployeeWatcher
    { 
        private readonly string _connStr;
        private readonly IMemoryCache _cache;
        private readonly ILogger<SqlEmployeeWatcher> _logger;
        private SqlDependency? _dependency;

        public SqlEmployeeWatcher(DbConnectionStrings dbConnectionStrings, IMemoryCache cache, ILogger<SqlEmployeeWatcher> logger)
        {

            _connStr = dbConnectionStrings.MainDb ?? throw new DataNotFoundException("Connection string not found");
           
            _cache = cache;
            _logger = logger;
            //Starting SqlDependency...
            _logger.LogInformation("Starting SqlDependency");
            SqlDependency.Start(_connStr);
            //register for changes
            RegisterAllDependencies();
            LoadEmployeeCache();
        }
        private void RegisterAllDependencies()
        {
            RegisterDependency("Employee", ApplicationConstants.Employee);
            RegisterDependency("Designation", ApplicationConstants.Designation);
            RegisterDependency("PersonalDetails", ApplicationConstants.PersonalDetails);
            RegisterDependency("Project", ApplicationConstants.Project);


        }

        private void RegisterDependency(string name, string query)
        {
            List<EmployeeDto> employees = [];
            using (var connection = new SqlConnection(_connStr))
            {
                var command = new SqlCommand(query, connection); // simple, static query
                _dependency = new SqlDependency(command);
                _dependency.OnChange += OnTableChange;
                connection.Open();
                var reader = command.ExecuteReader();
                _logger.LogInformation($"SqlDependency registered for {name}");
            }
        }



        /// <summary>
        /// Called when a change is detected in the monitored table/query
        /// this checks for the changes in db and removes the cachekey
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTableChange(object sender, SqlNotificationEventArgs e)
        {
            //checks for changes in db and deletes the cache
            if (e.Type == SqlNotificationType.Change)
            {
                _logger.LogInformation("Detected change in employee table. Updating cache...");
                _cache.Remove("employees");
                // 3. Re-register for future changes
                RegisterAllDependencies();
                LoadEmployeeCache();
            }
            else
            {
                _logger.LogInformation($"SqlDependency not triggered correctly. Type={e.Type}, Info={e.Info}, Source={e.Source}");
            }
        }

        private void LoadEmployeeCache()
        {
            List<EmployeeDto> employees = new();
            using (var connection = new SqlConnection(_connStr))
            {
                var command = new SqlCommand(ApplicationConstants.EmployeeDetails, connection); // simple, static query
                _dependency = new SqlDependency(command);
                connection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    employees.Add(new EmployeeDto
                    {
                        EmpGuid = (Guid)reader["Id"],
                        EmpId = (string)reader["EmpID"],
                        FirstName = reader["FirstName"]?.ToString() ?? string.Empty,
                        LastName = reader["LastName"]?.ToString() ?? string.Empty,
                        Email = reader["Email"]?.ToString() ?? string.Empty,
                        PersonalPhoneNumber = reader["PersonalPhoneNumber"]?.ToString() ?? string.Empty,
                        DesgGuid = reader["DesgGuid"] is DBNull ? null : (Guid?)reader["DesgGuid"],
                        DesgId = reader["DesgID"] is DBNull ? null : (int?)reader["DesgID"],
                        Desg = reader["Desg"]?.ToString() ?? string.Empty,
                        ProjGuid = reader["ProjGuid"] is DBNull ? null : (Guid?)reader["ProjGuid"],
                        ProjId = reader["ProjID"] is DBNull ? null : (int?)reader["ProjID"],
                        Project = reader["Project"]?.ToString() ?? string.Empty,
                        Gender = reader["Gender"]?.ToString() ?? string.Empty,
                        ManagerEmpCode = reader["ManagerEmpCode"]?.ToString() ?? string.Empty,
                        Doj = reader["Doj"] is DBNull?null:(DateTime?)reader["Doj"]
                    });
                }

                //set the cache data with the cachekey
                _cache.Set("employees", employees);
                _logger.LogInformation("Employee cache loaded successfully");

            }
        }
    }
}
