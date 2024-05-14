﻿using EF_Core_Assignment1.Application.DTOs.Employee;
using NuGet.Packaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Core_Assignment1.Application.DTOs.Department
{
    public class DepartmentViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Guid> EmployeeIds { get; set; }
    }
}
