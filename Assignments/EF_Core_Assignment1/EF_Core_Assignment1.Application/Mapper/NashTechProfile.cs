using AutoMapper;
using EF_Core_Assignment1.Application.DTOs.Department;
using EF_Core_Assignment1.Application.DTOs.Employee;
using EF_Core_Assignment1.Application.DTOs.Project;
using EF_Core_Assignment1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Core_Assignment1.Application.Mapper
{
    public class NashTechProfile : Profile
    {
        
        public NashTechProfile() {
            // Employee
            CreateMap<Employee, EmployeeViewModel>()
                .ForMember(dest => dest.SalaryAmount, opt => opt.MapFrom(src => src.Salary.SalaryAmount))
                .ForMember(dest => dest.ProjectIds, opt => opt.MapFrom(src => src.ProjectEmployees.Select(pe => pe.ProjectId)));
            CreateMap<EmployeeCreateRequest, Employee>()
                .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => new Salary { SalaryAmount = src.SalaryAmount }));
            CreateMap<EmployeeEditRequest, Employee>()
                .ForPath(dest => dest.Salary.SalaryAmount, opt => opt.MapFrom(src => src.SalaryAmount));

            // Department
            CreateMap<Department, DepartmentViewModel>()
                .ForMember(dest => dest.EmployeeIds, opt => opt.MapFrom(src => src.Employees.Select(e => e.Id)));
            CreateMap<DepartmentCreateRequest, Department>();
            CreateMap<DepartmentEditRequest, Department>();

            // Project
            CreateMap<Project, ProjectViewModel>()
                .ForMember(dest => dest.EmployeeIds, opt => opt.MapFrom(src => src.Employees.Select(e => e.Id)));
            CreateMap<ProjectCreateRequest, Project>();
            CreateMap<ProjectEditRequest, Project>();
        }
    }
}
