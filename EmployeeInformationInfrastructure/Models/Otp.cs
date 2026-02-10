using System;
using System.Collections.Generic;

namespace EmpInfoInfra.Models;

public partial class Otp
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string? Username { get; set; }

    public string OtpCode { get; set; } = null!;

    public DateTime Expiry { get; set; }
}
