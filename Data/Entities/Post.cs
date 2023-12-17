namespace StaffTracking.Data.Entities;

public class Post
{
    public int Id { get; set; }
    public string PostName { get; set; }

    public int DepartmentId { get; set; }
    public Department Department { get; set; }
    public List<Employee> Employees { get; set; }
}