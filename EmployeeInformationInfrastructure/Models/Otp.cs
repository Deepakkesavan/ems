namespace EmpInfoInfra.Models;

public partial class Otp
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string OtpCode { get; set; } = null!;

    public DateTime Expiry { get; set; }
}
