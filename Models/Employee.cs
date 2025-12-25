namespace ITHelpDesk.Models;

public class Employee
{
    public int EmployeeId { get; set; }
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string? Phone { get; set; }
    public int DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public int TotalRecordCount { get; set; }
}