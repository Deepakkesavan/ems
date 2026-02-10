namespace EmpInfoInner
{
    public class ApplicationConstants
    {
        public const  string Employee = @"SELECT EmpID,Id,FirstName,LastName,Email FROM dbo.Employee";
        public const string Designation = @"SELECT DesgID,Desg,Id FROM dbo.Designation";
        public const string PersonalDetails = @"SELECT EmpId,Gender,PersonalPhoneNumber FROM dbo.PersonalDetails";
        public const string Project = @"SELECT Id,Project FROM dbo.Project";



        public const string EmployeeDetails = @"
    SELECT 
    e.EmpID,
    e.Id,
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
	w.ManagerEmpCode,
    w.DOJ
FROM Employee e
INNER JOIN PersonalDetails pd ON pd.EmpId=e.EmpID 
INNER JOIN WorkInfo w ON e.EmpID = w.EmpID
INNER JOIN Project p on p.ProjID=w.ProjId
INNER JOIN Designation d ON w.DesgnID = d.Id";
    }
}
