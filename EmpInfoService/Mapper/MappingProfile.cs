using AutoMapper;
using EmpInfoInfra.Models;
using EmpInfoService.Model;

namespace EmpInfoService.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeDto>()
                // Direct fields
                .ForMember(dest => dest.EmpGuid, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(src => src.EmpId))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Profile, opt => opt.MapFrom(src => src.Profile))
                .ForPath(dest => dest.Dob, opt => opt.MapFrom(src => src.PersonalDetail.Dob));

                    // PersonalDetail
                    //.ForMember(dest => dest.Dob, opt => opt.MapFrom(src => src.PersonalDetail.Dob));
                    //        .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.PersonalDetail!.Age))
                    //        .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.PersonalDetail!.Gender))
                    //        .ForMember(dest => dest.EmergencyContact1, opt => opt.MapFrom(src => src.PersonalDetail!.EmergencyContact1))
                    //        .ForMember(dest => dest.PersonalPhoneNumber, opt => opt.MapFrom(src => src.PersonalDetail!.PersonalPhoneNumber))
                    //        .ForMember(dest => dest.EmergencyContactName1, opt => opt.MapFrom(src => src.PersonalDetail!.EmergencyContactName1))
                    //        .ForMember(dest => dest.EmergencyContactName2, opt => opt.MapFrom(src => src.PersonalDetail!.EmergencyContactName2))
                    //        .ForMember(dest => dest.EmergencyContact2, opt => opt.MapFrom(src => src.PersonalDetail!.EmergencyContact2))
                    //        .ForMember(dest => dest.PermanentAddress, opt => opt.MapFrom(src => src.PersonalDetail!.PermanentAddress))
                    //        .ForMember(dest => dest.PresentAddress, opt => opt.MapFrom(src => src.PersonalDetail!.PresentAddress))

            //        // WorkInfo
            //        .ForMember(dest => dest.SourceOfHire, opt => opt.MapFrom(src => src.WorkInfo!.SourceOfHire))
            //        .ForMember(dest => dest.Doj, opt => opt.MapFrom(src => src.WorkInfo!.Doj))
            //        .ForMember(dest => dest.Doc, opt => opt.MapFrom(src => src.WorkInfo!.Doc))
            //        .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.WorkInfo!.Status))
            //        .ForMember(dest => dest.CurrExp, opt => opt.MapFrom(src => src.WorkInfo!.CurrExp))
            //        .ForMember(dest => dest.TotalExp, opt => opt.MapFrom(src => src.WorkInfo!.TotalExp))
            //        .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.WorkInfo!.Role!.RoleName))
            //        .ForMember(dest => dest.EmailTriggerStatus, opt => opt.MapFrom(src => src.WorkInfo!.EmailTriggerStatus))
            //        .ForMember(dest => dest.TransferFromDate, opt => opt.MapFrom(src => src.WorkInfo!.TransferFromDate))
            //        .ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.WorkInfo!.TypeId))
            //        .ForMember(dest => dest.Desg, opt => opt.MapFrom(src => src.WorkInfo!.Desgn!.Desg))
            //        .ForMember(dest => dest.ReportingManager, opt => opt.MapFrom(src => src.WorkInfo!.ReportingManager))
            //        .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.WorkInfo!.Proj!.ProjectName))

            //// IdentityInfo
            //.ForMember(dest => dest.Uan, opt => opt.MapFrom(src => src.IdentityInfo!.Uan))
            //.ForMember(dest => dest.Pan, opt => opt.MapFrom(src => src.IdentityInfo!.Pan))
            //.ForMember(dest => dest.Aadhar, opt => opt.MapFrom(src => src.IdentityInfo!.Aadhar))
            //.ForMember(dest => dest.Ifsc, opt => opt.MapFrom(src => src.IdentityInfo!.Ifsc));
            // Map child entities into EmployeeDto
            //CreateMap<PersonalDetail, EmployeeDto>();
            ////CreateMap<WorkInfo, EmployeeDto>()
            ////    .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.RoleName))
            ////    .ForMember(dest => dest.Desg, opt => opt.MapFrom(src => src.Desgn.Desg))
            ////    .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Proj.ProjectName));
            ////CreateMap<IdentityInfo, EmployeeDto>();

            ////// Main Employee -> EmployeeDto
            //CreateMap<Employee, EmployeeDto>()
            //    .IncludeMembers(e => e.PersonalDetail);


            // Map from UpdateEmployeeDto to Employee
            CreateMap<UpdateEmployeeDto, Employee>()
                .ForMember(dest => dest.PersonalDetail, opt => opt.Ignore())
                .ForMember(dest => dest.IdentityInfo, opt => opt.Ignore())
                .ForMember(dest => dest.WorkInfo, opt => opt.Ignore());

            // Map from UpdateEmployeeDto to PersonalDetail
            CreateMap<UpdateEmployeeDto, PersonalDetail>();

            // Map from UpdateEmployeeDto to IdentityInfo
            CreateMap<UpdateEmployeeDto, IdentityInfo>();

            // Map from Employee to EmployeeDto
            CreateMap<Employee, EmployeeDto>();

            CreateMap<Employee, OrgChartDto>()
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(src => src.EmpId))
                .ForMember(dest=>dest.Name,opt=>opt.MapFrom(src=>src.FirstName+" "+src.LastName))
                .ForMember(dest=>dest.Designation,opt=>opt.MapFrom(src=>src.WorkInfo!.Desgn!.Desg))
                .ForMember(dest=>dest.Department,opt=>opt.MapFrom(src=>src.WorkInfo!.Dept!.Department1))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PersonalDetail!.PersonalPhoneNumber))
                .ForMember(dest=>dest.Email,opt=>opt.MapFrom(src=>src.Email))
                .ForMember(dest=>dest.HireDate,opt=>opt.MapFrom(src=>src.WorkInfo!.Doj))
                .ForMember(dest=>dest.ReportsCount,opt=>opt.Ignore())
                .ForMember(dest=>dest.ReportsTo,opt=>opt.MapFrom(src=>src.WorkInfo!.ManagerEmpCode))
                .ForMember(dest=>dest.Location,opt=>opt.MapFrom(src=>src.WorkInfo!.Loc))
                .ForMember(dest=>dest.Children,opt=>opt.Ignore());
        }
    }
        }

