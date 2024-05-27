using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EF_Core_Assignment1.Persistance.Contexts;
using EF_Core_Assignment1.Application.DTOs.Employee;
using EF_Core_Assignment1.Application.Services;
using EF_Core_Assignment1.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Internal;
using System.Globalization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace EF_Core_Assignment1.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeServices _employeeServices;
        private readonly NashTechContext _context;
        private readonly IMapper _mapper;

        public EmployeesController(IEmployeeServices employeeServices, NashTechContext context, IMapper mapper)
        {
            _employeeServices = employeeServices;
            _context = context;
            _mapper = mapper;
        }


        // GET: api/Employees
        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<EmployeeViewModel>>> GetEmployees(
            [FromQuery] float? SalaryAmount,
            [FromQuery(Name = "date")] string? dateString)
        {
            // Prepare the filters
            if (!dateString.IsNullOrEmpty())
            {
                if (!DateTime.TryParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    // If the date string cannot be parsed, return a bad request
                    return BadRequest("Invalid date format. Use dd/MM/yyyy.");
                }
                return await _employeeServices.GetEmployeesAsync(SalaryAmount, date);
            }

            return await _employeeServices.GetEmployeesAsync(SalaryAmount, null);
        }

        [HttpGet("with-department-names")]
        public async Task<ActionResult<IEnumerable<Object>>> GetEmployeesWithDepartmentNames()
        {
            var employeesWithDepartments = await _context.Employees
                .Join(
                    _context.Departments,
                    e => e.DepartmentId,
                    d => d.Id,
                    (e, d) => new
                    {
                        EmployeeId = e.Id,
                        EmployeeName = e.Name,
                        DepartmentName = d.Name
                    })
                .ToListAsync();
            return employeesWithDepartments;
        }

        [HttpGet("with-projects")]
        public async Task<ActionResult<IEnumerable<Object>>> GetEmployeesWithProjects()
        {
            var employeesWithProjects = await _context.Employees
                .GroupJoin(
                    _context.ProjectEmployees,
                    employee => employee.Id,
                    projectEmployee => projectEmployee.EmployeeId,
                    (employee, projectEmployees) => new

                    {
                        EmployeeId = employee.Id,
                        EmployeeName = employee.Name,
                        Projects = projectEmployees.Select(pe => pe.Project.Name).ToList()
                    }
                )
                //.SelectMany(
                //    e => e.Projects.DefaultIfEmpty(),
                //    (e, project) => new
                //    {
                //        EmployeeId = e.EmployeeId,
                //        EmployeeName = e.EmployeeName,
                //        ProjectName = project
                //    }
                //)
                .ToListAsync();
            return employeesWithProjects;
        }

        //GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeViewModel>> GetEmployee(Guid id)
        {
            var employee = await _context.Employees
                .Include(e => e.Salary)
                .Include(e => e.ProjectEmployees)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return _mapper.Map<EmployeeViewModel>(employee);
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(Guid id, [FromBody] EmployeeEditRequest employeeEditRequest)
        {
            if (!EmployeeExists(id))
            {
                return NotFound();
            }

            var employeeToCheck = await _context.Employees.Include(e => e.Salary).SingleAsync(e => e.Id == id);
            // Map data from EmployeeEditRequest to the existing employeeToCheck
            _mapper.Map(employeeEditRequest, employeeToCheck);

            _context.Employees.Entry(employeeToCheck).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmployeeViewModel>> PostEmployee(EmployeeCreateRequest employeeCreateRequest)
        {
            var employee = _mapper.Map<Employee>(employeeCreateRequest);
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, null);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(Guid id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
