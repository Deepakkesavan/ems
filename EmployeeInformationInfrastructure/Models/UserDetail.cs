namespace EmpInfoInfra.Models;

public partial class UserDetail
{
    public int Id { get; set; }

    public string Password { get; set; } = null!;

    public string Username { get; set; } = null!;
}
