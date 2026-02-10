using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpInfoService.Constant
{
    public static class BussinessConstant
    {
        public const string CONNECTION_STRING_URI = "/api/ClariumConfiguration/connection-strings";
        public const string MODULES_URI = "/api/ClariumConfiguration/modules";
        public const string MODULES_CACHE_KEY = "Modules";
        public const string CONNECTION_STRING_CACHE_KEY = "ConnectionStrings";
        public const string SSO_MODULE_NAME = "SingleSignoutModule";
        public const string EMPLOYEE_CONNECTION_STRING_KEY = "EmployeeManagement";
        public const string CATALOG_CONNECTION_STRING_KEY = "CatalogManagement";
    }
}
