using EmployeeManagement.Models;

namespace EmployeeManagement.Services;

public interface IEmployeeService
{
    Task<IEnumerable<Employee>> GetEmployeesAsync(string? search = null);
    Task<Employee?> GetEmployeeAsync(int id);
    Task<bool> IsEmailTakenAsync(string email, int? excludeId = null);
    Task CreateAsync(Employee employee);
    Task UpdateAsync(Employee employee);
    Task DeleteAsync(int id);
}
