using System;
using System.Collections.Generic;

namespace EmpInfoInfra.Models;

public partial class SsoUser
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateOnly? ExitDate { get; set; }

    public bool? FirstTimeLogin { get; set; }
}
