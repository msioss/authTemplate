namespace StaffTracking.Models;

public class UserDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public DateTime? LastLogIn { get; set; }
    public DateTime? LastLogOut { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
}