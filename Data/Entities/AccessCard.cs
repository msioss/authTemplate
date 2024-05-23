namespace StaffTracking.Data.Entities;

public class AccessCard
{
    public int Id { get; set; }
    public string Value { get; set; }

    public string EmployeeId { get; set; }
    public Employee Employee { get; set; }
    
    public int DepartmentId { get; set; }
    public Department Department { get; set; }
}