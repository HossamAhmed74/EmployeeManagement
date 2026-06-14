using EmployeeManagement.Data;
using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _context;

    public EmployeeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync(string? search = null)
    {
        var query = _context.Employees
            .Include(e => e.Department)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();
            query = query.Where(e =>
                e.FullName.Contains(search) ||
                e.Department!.Name.Contains(search));
        }

        return await query.OrderBy(e => e.FullName).ToListAsync();
    }

    public async Task<Employee?> GetByIdAsync(int id) =>
        await _context.Employees
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.Id == id);

    public async Task<bool> EmailExistsAsync(string email, int? excludeId = null) =>
        await _context.Employees
            .AnyAsync(e => e.Email == email && (excludeId == null || e.Id != excludeId));

    public async Task AddAsync(Employee employee)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Employee employee)
    {
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Employee employee)
    {
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
    }
}
