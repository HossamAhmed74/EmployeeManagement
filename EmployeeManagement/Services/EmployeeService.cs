using EmployeeManagement.Models;
using EmployeeManagement.Repositories;

namespace EmployeeManagement.Services;

// Holds business logic and sits between the controller and the repository.
public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;

    public EmployeeService(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Employee>> GetEmployeesAsync(string? search = null) =>
        _repository.GetAllAsync(search);

    public Task<Employee?> GetEmployeeAsync(int id) =>
        _repository.GetByIdAsync(id);

    public Task<bool> IsEmailTakenAsync(string email, int? excludeId = null) =>
        _repository.EmailExistsAsync(email, excludeId);

    public Task CreateAsync(Employee employee) =>
        _repository.AddAsync(employee);

    public Task UpdateAsync(Employee employee) =>
        _repository.UpdateAsync(employee);

    public async Task DeleteAsync(int id)
    {
        var employee = await _repository.GetByIdAsync(id);
        if (employee is not null)
        {
            await _repository.DeleteAsync(employee);
        }
    }
}
