namespace EmpInfoInner
{
    public class ApplicationConstants
    {
        public const  string Employee = "SELECT Id,first_name,email_address,city FROM dbo.user_details";
        public const string EmployeeDetails = @"
    SELECT 
    e.EmpID,
    e.Id AS EmpGuid,
    e.FirstName, e.LastName,
    e.Email,
	pd.Gender,
	pd.PersonalPhoneNumber,
    d.Desg,
    d.DesgID,
    d.Id AS DesgGuid,
	p.ProjID,
	p.Id AS ProjGuid,
	p.Project,
	w.ManagerEmpCode
FROM Employee e
INNER JOIN PersonalDetails pd ON pd.EmpId=e.EmpID 
INNER JOIN WorkInfo w ON e.EmpID = w.EmpID
INNER JOIN Project p on p.ProjID=w.ProjId
INNER JOIN Designation d ON w.DesgnID = d.Id";
    }
}
