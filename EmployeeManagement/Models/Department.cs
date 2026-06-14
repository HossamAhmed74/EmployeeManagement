using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models;

public class Department
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Department name is required.")]
    [StringLength(100, ErrorMessage = "Department name cannot exceed 100 characters.")]
    [Display(Name = "Department Name")]
    public string Name { get; set; } = string.Empty;

    // Navigation property: one department has many employees.
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
