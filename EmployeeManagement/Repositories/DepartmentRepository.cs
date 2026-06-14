using EmployeeManagement.Data;
using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly AppDbContext _context;

    public DepartmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Department>> GetAllAsync() =>
        await _context.Departments.AsNoTracking().OrderBy(d => d.Name).ToListAsync();

    public async Task<Department?> GetByIdAsync(int id) =>
        await _context.Departments.FindAsync(id);
}
