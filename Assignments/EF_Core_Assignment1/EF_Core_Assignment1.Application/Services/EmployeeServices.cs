using AutoMapper;
using EF_Core_Assignment1.Application.DTOs.Employee;
using EF_Core_Assignment1.Persistance.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Core_Assignment1.Application.Services
{
    public interface IEmployeeServices
    {
        Task<List<EmployeeViewModel>> GetEmployeesAsync(float? SalaryAmount, DateTime? date);
    }
    public class EmployeeServices : IEmployeeServices
    {
        private readonly NashTechContext _context;
        private readonly IMapper _mapper;
        public EmployeeServices(NashTechContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //To-do: Test this service by using an in-memory provider for the context rather than mocking it
        public async Task<List<EmployeeViewModel>> GetEmployeesAsync(float? SalaryAmount, DateTime? date)
        {
            // Prepare the filters
            var query = _context.Employees
                .Include(e => e.Salary)
                .Include(e => e.ProjectEmployees)
                .AsQueryable(); // Start with the base query

            if (SalaryAmount.HasValue)
            {
                query = query.Where(e => e.Salary.SalaryAmount > SalaryAmount);
            }
            if (date.HasValue)
            {
                query = query.Where(e => e.JoinedDate.Date >= date); // Filter by date
            }
            var employees = await query.ToListAsync();
            return _mapper.Map<List<EmployeeViewModel>>(employees);
        }
    }
}
