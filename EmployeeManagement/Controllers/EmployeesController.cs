using EmployeeManagement.Models;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeManagement.Controllers;

public class EmployeesController : Controller
{
    private readonly IEmployeeService _employeeService;
    private readonly IDepartmentService _departmentService;
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(
        IEmployeeService employeeService,
        IDepartmentService departmentService,
        ILogger<EmployeesController> logger)
    {
        _employeeService = employeeService;
        _departmentService = departmentService;
        _logger = logger;
    }

    // GET: /Employees?search=...
    public async Task<IActionResult> Index(string? search)
    {
        var employees = await _employeeService.GetEmployeesAsync(search);
        ViewData["Search"] = search;
        return View(employees);
    }

    // GET: /Employees/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var employee = await _employeeService.GetEmployeeAsync(id);
        if (employee is null) return NotFound();
        return View(employee);
    }

    // GET: /Employees/Create
    public async Task<IActionResult> Create()
    {
        await PopulateDepartmentsAsync();
        return View(new Employee());
    }

    // POST: /Employees/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Employee employee)
    {
        await ValidateUniqueEmailAsync(employee);

        if (!ModelState.IsValid)
        {
            await PopulateDepartmentsAsync(employee.DepartmentId);
            return View(employee);
        }

        await _employeeService.CreateAsync(employee);
        TempData["Success"] = "Employee created successfully.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Employees/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var employee = await _employeeService.GetEmployeeAsync(id);
        if (employee is null) return NotFound();

        await PopulateDepartmentsAsync(employee.DepartmentId);
        return View(employee);
    }

    // POST: /Employees/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Employee employee)
    {
        if (id != employee.Id) return NotFound();

        await ValidateUniqueEmailAsync(employee);

        if (!ModelState.IsValid)
        {
            await PopulateDepartmentsAsync(employee.DepartmentId);
            return View(employee);
        }

        await _employeeService.UpdateAsync(employee);
        TempData["Success"] = "Employee updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Employees/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var employee = await _employeeService.GetEmployeeAsync(id);
        if (employee is null) return NotFound();
        return View(employee);
    }

    // POST: /Employees/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _employeeService.DeleteAsync(id);
        TempData["Success"] = "Employee deleted successfully.";
        return RedirectToAction(nameof(Index));
    }

    // Adds a model error if the email is already used by another employee.
    private async Task ValidateUniqueEmailAsync(Employee employee)
    {
        if (!string.IsNullOrWhiteSpace(employee.Email) &&
            await _employeeService.IsEmailTakenAsync(employee.Email, employee.Id))
        {
            ModelState.AddModelError(nameof(Employee.Email), "This email is already in use.");
        }
    }

    private async Task PopulateDepartmentsAsync(int? selectedId = null)
    {
        var departments = await _departmentService.GetDepartmentsAsync();
        ViewBag.Departments = new SelectList(departments, "Id", "Name", selectedId);
    }
}
