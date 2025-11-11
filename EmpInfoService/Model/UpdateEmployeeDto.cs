namespace EmpInfoService.Model
{
    public class UpdateEmployeeDto
    {

        public string? EmpId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }
        public string? Email { get; set; }
        public byte[]? Profile { get; set; }

        public DateTime? Dob { get; set; }

        public string? Age { get; set; }

        public string? Gender { get; set; }

        public string? EmergencyContact1 { get; set; }

        public string? PersonalPhoneNumber { get; set; }

        public string? EmergencyContactName1 { get; set; }

        public string? EmergencyContactName2 { get; set; }

        public string? EmergencyContact2 { get; set; }

        public string? PermanentAddress { get; set; }

        public string? PresentAddress { get; set; }
        //identity info
        public string? Uan { get; set; }

        public string? Pan { get; set; }
        public long? Aadhar { get; set; }
        public string? Ifsc { get; set; }

    }
}
