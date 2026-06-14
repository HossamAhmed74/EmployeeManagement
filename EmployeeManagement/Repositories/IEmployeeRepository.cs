using EmployeeManagement.Models;

namespace EmployeeManagement.Repositories;

public interface IEmployeeRepository
{
    // Pass a search term to filter by name or department; null returns all.
    Task<IEnumerable<Employee>> GetAllAsync(string? search = null);
    Task<Employee?> GetByIdAsync(int id);
    Task<bool> EmailExistsAsync(string email, int? excludeId = null);
    Task AddAsync(Employee employee);
    Task UpdateAsync(Employee employee);
    Task DeleteAsync(Employee employee);
}
