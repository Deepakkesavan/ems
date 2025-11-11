using AutoMapper;
using EmpInfoInfra.Models;
using EmpInfoService.Model;
using GlobalExceptionHandler.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmpInfoService.Services.ServiceImpl
{
    public  class EmpService
    {
        private readonly EmployeeDetailsContext _context;
        private readonly IMapper _mapper;
        public EmpService(EmployeeDetailsContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }
        public async Task<List<EmployeeDto>> GetEmployeeDetails()
        {
            var employees = await _context.Employees
                .Include(i => i.PersonalDetail)
                .Include(j => j.WorkInfo)
                    .ThenInclude(w => w.Desgn)
                .Include(j => j.WorkInfo)
                    .ThenInclude(w => w.Role)
                .Include(j => j.WorkInfo)
                    .ThenInclude(w => w.Proj)
                .Include(k => k.IdentityInfo)
                .ToListAsync();
            Console.WriteLine(employees);
            List<EmployeeDto> employeesList = new();
            foreach(var employee in employees)
            {
                EmployeeDto responseDto = new();
                //responseDto = _mapper.Map<EmployeeDto>(employee);
                responseDto = new EmployeeDto
                {
                    // Direct Employee fields
                    EmpGuid = employee.Id,
                    EmpId = employee.EmpId,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    Profile = employee.Profile,

                    // PersonalDetail (null-safe)
                    Dob = employee.PersonalDetail?.Dob,
                    Age = employee.PersonalDetail?.Age,
                    Gender = employee.PersonalDetail?.Gender,
                    PersonalPhoneNumber = employee.PersonalDetail?.PersonalPhoneNumber,
                    EmergencyContact1 = employee.PersonalDetail?.EmergencyContact1,
                    EmergencyContactName1 = employee.PersonalDetail?.EmergencyContactName1,
                    EmergencyContact2 = employee.PersonalDetail?.EmergencyContact2,
                    EmergencyContactName2 = employee.PersonalDetail?.EmergencyContactName2,
                    PermanentAddress = employee.PersonalDetail?.PermanentAddress,
                    PresentAddress = employee.PersonalDetail?.PresentAddress,

                    // WorkInfo (null-safe)
                    DesgId = employee.WorkInfo?.Desgn?.DesgId,
                    DesgGuid = employee.WorkInfo?.Desgn?.Id,
                    ProjId = employee.WorkInfo?.ProjId,
                    ProjGuid = employee.WorkInfo?.Proj?.Id,
                    SourceOfHire = employee.WorkInfo?.SourceOfHire,
                    Doj = employee.WorkInfo?.Doj,
                    Doc = employee.WorkInfo?.Doc,
                    Status = employee.WorkInfo?.Status,
                    CurrExp = employee.WorkInfo?.CurrExp,
                    TotalExp = employee.WorkInfo?.TotalExp,
                    Role = employee.WorkInfo?.Role?.RoleName,
                    Desg = employee.WorkInfo?.Desgn?.Desg,
                    ProjectName = employee.WorkInfo?.Proj?.ProjectName,
                    EmailTriggerStatus = employee.WorkInfo?.EmailTriggerStatus,
                    TransferFromDate = employee.WorkInfo?.TransferFromDate,
                    TypeId = employee.WorkInfo?.TypeId,
                    ReportingManager = employee.WorkInfo?.ReportingManager,

                    // IdentityInfo (null-safe)
                    Uan = employee.IdentityInfo?.Uan,
                    Pan = employee.IdentityInfo?.Pan,
                    Aadhar = employee.IdentityInfo?.Aadhar,
                    Ifsc = employee.IdentityInfo?.Ifsc
                };
                employeesList.Add(responseDto);
            }
            return employeesList;

        }
        public async Task<string> CreateEmployee(EmployeeDto employeeDto)
        {
            var employee = await _context.Employees.FindAsync(employeeDto.EmpId);
            if (employee != null)
            {
                throw new Exception($"Employee {employeeDto.FirstName + employeeDto.LastName} is already created");
            }
                employee = new Employee
                {
                    Id = Guid.NewGuid(),
                    FirstName = employeeDto.FirstName,
                    LastName = employeeDto.LastName,
                    Email = employeeDto.Email,
                    Profile = employeeDto.Profile,
                    CreatedTime = DateTime.UtcNow,
                    UpdatedTime = DateTime.UtcNow,
                    WorkInfo = new WorkInfo
                    {
                        SourceOfHire = employeeDto.SourceOfHire,
                        Doj = employeeDto.Doj,
                        Doc = employeeDto.Doc,
                        Status = employeeDto.Status,
                        CurrExp = employeeDto.CurrExp,
                        TotalExp = employeeDto.TotalExp,
                        TypeId = employeeDto.TypeId,
                        ReportingManager = employeeDto.ReportingManager,
                        EmailTriggerStatus = employeeDto.EmailTriggerStatus,
                        TransferFromDate = employeeDto.TransferFromDate
                    },
                };
           
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                return ($"Employee {employeeDto.FirstName + employeeDto.LastName} is already created");
            

        }
        public async Task<EmployeeDto> GetEmployeeDetailsById(string empId)
        {
            var employee = await _context.Employees
                .Include(i => i.PersonalDetail)
                .Include(j => j.WorkInfo)
                    .ThenInclude(w => w.Desgn)
                .Include(j => j.WorkInfo)
                    .ThenInclude(w => w.Role)
                .Include(j => j.WorkInfo)
                    .ThenInclude(w => w.Proj)
                .Include(k => k.IdentityInfo)
                .FirstOrDefaultAsync(z => z.EmpId == empId);
            if (employee == null)
                throw new DataNotFoundException($"Employee with ID {empId} not found.");
            EmployeeDto responseDto = new();
            //EmployeeDto responseDto = _mapper.Map<EmployeeDto>(employee);
            responseDto = new EmployeeDto
            {
                // Direct Employee fields
                EmpGuid = employee.Id,
                EmpId = employee.EmpId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Profile = employee.Profile,

                // PersonalDetail (null-safe)
                Dob = employee.PersonalDetail?.Dob,
                Age = employee.PersonalDetail?.Age,
                Gender = employee.PersonalDetail?.Gender,
                PersonalPhoneNumber = employee.PersonalDetail?.PersonalPhoneNumber,
                EmergencyContact1 = employee.PersonalDetail?.EmergencyContact1,
                EmergencyContactName1 = employee.PersonalDetail?.EmergencyContactName1,
                EmergencyContact2 = employee.PersonalDetail?.EmergencyContact2,
                EmergencyContactName2 = employee.PersonalDetail?.EmergencyContactName2,
                PermanentAddress = employee.PersonalDetail?.PermanentAddress,
                PresentAddress = employee.PersonalDetail?.PresentAddress,

                // WorkInfo (null-safe)
                DesgId = employee.WorkInfo?.Desgn?.DesgId,
                DesgGuid = employee.WorkInfo?.Desgn?.Id,
                ProjId = employee.WorkInfo?.ProjId,
                ProjGuid = employee.WorkInfo?.Proj?.Id,
                Project=employee.WorkInfo?.Proj?.ProjectName,
                ManagerEmpCode = employee.WorkInfo?.ManagerEmpCode,
                SourceOfHire = employee.WorkInfo?.SourceOfHire,
                Doj = employee.WorkInfo?.Doj,
                Doc = employee.WorkInfo?.Doc,
                Status = employee.WorkInfo?.Status,
                CurrExp = employee.WorkInfo?.CurrExp,
                TotalExp = employee.WorkInfo?.TotalExp,
                Role = employee.WorkInfo?.Role?.RoleName,
                Desg = employee.WorkInfo?.Desgn?.Desg,
                ProjectName = employee.WorkInfo?.Proj?.ProjectName,
                EmailTriggerStatus = employee.WorkInfo?.EmailTriggerStatus,
                TransferFromDate = employee.WorkInfo?.TransferFromDate,
                TypeId = employee.WorkInfo?.TypeId,
                ReportingManager = employee.WorkInfo?.ReportingManager,

                // IdentityInfo (null-safe)
                Uan = employee.IdentityInfo?.Uan,
                Pan = employee.IdentityInfo?.Pan,
                Aadhar = employee.IdentityInfo?.Aadhar,
                Ifsc = employee.IdentityInfo?.Ifsc
            };
            return responseDto;

        }
        public async Task<string> UploadProfile(BaseRequestDto dto)
        {
            byte[]? profileData = null;
            var employee = await _context.Employees.FindAsync(dto.EmpId);
            //Employee employee = new();
            if (dto.Profile != null && dto.Profile.Length > 0)
            {
                using var ms = new MemoryStream();
                await dto.Profile.CopyToAsync(ms);
                profileData = ms.ToArray();
            }
            employee.UpdatedBy=dto.EmpId;
            employee.UpdatedTime = DateTime.UtcNow;
            employee.Profile = profileData;

            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();

            return "Uploaded Successfully";
        }
        public async Task<byte[]?> GetProfile(BaseRequestDto requestDto)
        {
            var emp = await _context.Employees.FindAsync(requestDto.EmpId);
            if (emp == null || emp.Profile == null)
                throw new DataNotFoundException($"{requestDto.EmpId} profile is not found");
            return emp.Profile;

        }

        // assuming default png if unknown, you can also store ContentType separately
        public async Task<string> UpdateEmployeeDetails(UpdateEmployeeDto updatedEmployeeDto)
        {
            if (updatedEmployeeDto == null)
                throw new ArgumentNullException(nameof(updatedEmployeeDto));
  
                var employee = await _context.Employees
                    .Include(i => i.PersonalDetail)                   
                    .Include(k => k.IdentityInfo)
                    .FirstOrDefaultAsync(z => z.EmpId == updatedEmployeeDto.EmpId);

                if (employee == null)
                    throw new DataNotFoundException($"Employee with ID {updatedEmployeeDto.EmpId} not found.");

                // Update employee entity with values from DTO
                employee.FirstName = updatedEmployeeDto.FirstName;
                employee.LastName = updatedEmployeeDto.LastName;
                employee.Email = updatedEmployeeDto.Email;
                employee.Profile = updatedEmployeeDto.Profile;

                // Update PersonalDetail
                if (employee.PersonalDetail != null)
                {
                    employee.PersonalDetail.EmpId = updatedEmployeeDto.EmpId;
                    employee.PersonalDetail.PersonalPhoneNumber = updatedEmployeeDto.PersonalPhoneNumber;
                    employee.PersonalDetail.Age = updatedEmployeeDto.Age;
                    employee.PersonalDetail.Dob = updatedEmployeeDto.Dob;
                    employee.PersonalDetail.EmergencyContactName1 = updatedEmployeeDto.EmergencyContactName1;
                    employee.PersonalDetail.EmergencyContact1 = updatedEmployeeDto.EmergencyContact1;
                    employee.PersonalDetail.EmergencyContactName2 = updatedEmployeeDto.EmergencyContactName2;
                    employee.PersonalDetail.EmergencyContact2 = updatedEmployeeDto.EmergencyContact2;
                    employee.PersonalDetail.PermanentAddress = updatedEmployeeDto.PermanentAddress;
                    employee.PersonalDetail.PresentAddress = updatedEmployeeDto.PresentAddress;
                }
                else
                {
                    // Create new PersonalDetail if it doesn't exist
                    employee.PersonalDetail = new PersonalDetail
                    {
                        PersonalPhoneNumber = updatedEmployeeDto.PersonalPhoneNumber,
                        Age = updatedEmployeeDto.Age,
                        Dob = updatedEmployeeDto.Dob,
                        EmergencyContactName1 = updatedEmployeeDto.EmergencyContactName1,
                        EmergencyContact1 = updatedEmployeeDto.EmergencyContact1,
                        EmergencyContactName2 = updatedEmployeeDto.EmergencyContactName2,
                        EmergencyContact2 = updatedEmployeeDto.EmergencyContact2,
                        PermanentAddress = updatedEmployeeDto.PermanentAddress,
                        PresentAddress = updatedEmployeeDto.PresentAddress
                    };
                }

                // Update IdentityInfo
                if (employee.IdentityInfo != null)
                {
                    employee.IdentityInfo.EmpId = updatedEmployeeDto.EmpId;
                    employee.IdentityInfo.Pan = updatedEmployeeDto.Pan;
                    employee.IdentityInfo.Uan = updatedEmployeeDto.Uan;
                    employee.IdentityInfo.Ifsc = updatedEmployeeDto.Ifsc;
                }
                else
                {
                    // Create new IdentityInfo if it doesn't exist
                    employee.IdentityInfo = new IdentityInfo
                    {
                        Pan = updatedEmployeeDto.Pan,
                        Uan = updatedEmployeeDto.Uan,
                        Ifsc = updatedEmployeeDto.Ifsc
                    };
                }
                await _context.SaveChangesAsync();
                return "Updated Successfully";


            // Return updated employee as DTO
            }
           
        }
        //public async Task<EmployeeDto> UpdateEmployeeDetails(UpdateEmployeeDto updatedEmployeeDto)
        //{
        //    var employee = await _context.Employees
        //        .Include(i => i.PersonalDetail)
        //        .Include(j => j.WorkInfo)
        //            .ThenInclude(w => w.Desgn)
        //        .Include(j => j.WorkInfo)
        //            .ThenInclude(w => w.Role)
        //        .Include(j => j.WorkInfo)
        //            .ThenInclude(w => w.Proj)
        //        .Include(k => k.IdentityInfo)
        //        .FirstOrDefaultAsync(z => z.EmpId == updatedEmployeeDto.EmpId);
        //    if (updatedEmployeeDto == null)
        //        throw new ArgumentNullException(nameof(updatedEmployeeDto));
        //    UpdateEmployeeDto employeeDto = new();
        //    updatedEmployeeDto.FirstName = employee.FirstName;
        //    updatedEmployeeDto.LastName = employee.LastName;
        //    updatedEmployeeDto.Email = employee.Email;
        //    updatedEmployeeDto.PersonalPhoneNumber = employee.PersonalDetail.PersonalPhoneNumber;
        //    updatedEmployeeDto.Age = employee.PersonalDetail.Age;
        //    updatedEmployeeDto.Dob = employee.PersonalDetail.Dob;
        //    updatedEmployeeDto.EmergencyContactName1 = employee.PersonalDetail.EmergencyContactName1;
        //    updatedEmployeeDto.EmergencyContact1 = employee.PersonalDetail.EmergencyContact1;
        //    updatedEmployeeDto.EmergencyContactName2 = employee.PersonalDetail.EmergencyContactName2;
        //    updatedEmployeeDto.EmergencyContact2 = employee.PersonalDetail.EmergencyContact2;
        //    updatedEmployeeDto.Pan = employee.IdentityInfo.Pan;
        //    updatedEmployeeDto.Uan = employee.IdentityInfo.Uan;
        //    updatedEmployeeDto.Profile = employee.Profile;
        //    updatedEmployeeDto.PermanentAddress = employee.PersonalDetail.PermanentAddress;
        //    updatedEmployeeDto.Ifsc = employee.IdentityInfo.Ifsc;
        //    updatedEmployeeDto.PresentAddress = employee.PersonalDetail.PresentAddress;

        //    if (employee == null)
        //        throw new Exception($"Employee with ID {updatedEmployeeDto.EmpId} not found.");

        //    // Map changes from DTO to entity (AutoMapper can handle this if configured)
        //    //_mapper.Map(updatedEmployeeDto, employee);

        //    // Save changes
        //    await _context.SaveChangesAsync();

        //    // Map back to DTO
        //    return _mapper.Map<EmployeeDto>(employee);
        //}
    }

