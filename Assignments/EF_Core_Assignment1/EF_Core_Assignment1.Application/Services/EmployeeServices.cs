using AutoMapper;
using EF_Core_Assignment1.Application.DTOs.Employee;
using EF_Core_Assignment1.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Core_Assignment1.Application.Services
{
    public class EmployeeServices
    {
        private readonly NashTechContext _context;
        private readonly IMapper _mapper;
        public EmployeeServices(NashTechContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<EmployeeViewModel>> GetEmployeesAsync()
        {
            return _mapper.Map<List<EmployeeViewModel>>(await _context.Employees.Include(e => e.Salary).ToListAsync());
        }
    }
}
