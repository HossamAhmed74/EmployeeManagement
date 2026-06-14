using EmployeeManagement.Models;

namespace EmployeeManagement.Services;

public interface IDepartmentService
{
    Task<IEnumerable<Department>> GetDepartmentsAsync();
}
