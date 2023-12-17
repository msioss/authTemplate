using Microsoft.AspNetCore.Identity;

namespace StaffTracking.Data.Entities;

// Add profile data for application users by adding properties to the ApplicationUser class
public class User : IdentityUser
{
    public Employee Employee { get; set; }
    public List<AccessLog> AccessLogs { get; set; }
}

