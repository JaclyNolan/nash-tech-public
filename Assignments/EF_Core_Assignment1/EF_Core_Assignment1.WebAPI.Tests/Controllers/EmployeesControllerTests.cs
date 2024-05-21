using AutoMapper;
using EF_Core_Assignment1.Application.DTOs.Employee;
using EF_Core_Assignment1.Application.Services;
using EF_Core_Assignment1.Persistance.Contexts;
using EF_Core_Assignment1.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Core_Assignment1.WebAPI.Tests.Controllers
{
    public class EmployeesControllerTests
    {
        private readonly Mock<IEmployeeServices> _mockEmployeeServices = new Mock<IEmployeeServices>();
        private readonly Mock<NashTechContext> _mockNashTechContext = new Mock<NashTechContext>();
        private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();
        private readonly EmployeesController _employeesController;

        public EmployeesControllerTests()
        {
            _employeesController = new EmployeesController(_mockEmployeeServices.Object, _mockNashTechContext.Object, _mockMapper.Object);
        }
        [Fact]
        public async Task GetEmployees_InvalidDate_ReturnsBadRequest()
        {
            // Arrange
            string invalidDate = "invalid-date";

            // Act
            var result = await _employeesController.GetEmployees(null, invalidDate);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid date format. Use dd/MM/yyyy.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetEmployees_ReturnsEmployees_ForValidDateAndSalary()
        {
            // Arrange
            float? salaryAmount = 50000;
            string validDate = "01/01/2020";
            DateTime parsedDate = DateTime.ParseExact(validDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var employees = new List<EmployeeViewModel> { new EmployeeViewModel { Name = "John Doe" } };

            _mockEmployeeServices.Setup(svc => svc.GetEmployeesAsync(salaryAmount, parsedDate)).ReturnsAsync(employees);

            // Act
            var result = await _employeesController.GetEmployees(salaryAmount, validDate);

            // Assert
            var okResult = Assert.IsType<ActionResult<IEnumerable<EmployeeViewModel>>>(result);
            var returnValue = Assert.IsType<List<EmployeeViewModel>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetEmployees_ValidSalary_ReturnsEmployees()
        {
            // Arrange
            float? salaryAmount = 50000;
            var employees = new List<EmployeeViewModel> { new EmployeeViewModel { Name = "John Doe" } };

            _mockEmployeeServices.Setup(svc => svc.GetEmployeesAsync(salaryAmount, null)).ReturnsAsync(employees);

            // Act
            var result = await _employeesController.GetEmployees(salaryAmount, null);

            // Assert
            var okResult = Assert.IsType<ActionResult<IEnumerable<EmployeeViewModel>>>(result);
            var returnValue = Assert.IsType<List<EmployeeViewModel>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetEmployees_ForValidDateOnly_ReturnsEmployees()
        {
            // Arrange
            string validDate = "01/01/2020";
            DateTime parsedDate = DateTime.ParseExact(validDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var employees = new List<EmployeeViewModel> { new EmployeeViewModel { Name = "John Doe" } };

            _mockEmployeeServices.Setup(svc => svc.GetEmployeesAsync(null, parsedDate)).ReturnsAsync(employees);

            // Act
            var result = await _employeesController.GetEmployees(null, validDate);

            // Assert
            var okResult = Assert.IsType<ActionResult<IEnumerable<EmployeeViewModel>>>(result);
            var returnValue = Assert.IsType<List<EmployeeViewModel>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetEmployees_ForNoFilters_ReturnsEmployees()
        {
            // Arrange
            var employees = new List<EmployeeViewModel> { new EmployeeViewModel { Name = "John Doe" } };

            _mockEmployeeServices.Setup(svc => svc.GetEmployeesAsync(null, null)).ReturnsAsync(employees);

            // Act
            var result = await _employeesController.GetEmployees(null, null);

            // Assert
            var okResult = Assert.IsType<ActionResult<IEnumerable<EmployeeViewModel>>>(result);
            var returnValue = Assert.IsType<List<EmployeeViewModel>>(okResult.Value);
            Assert.Single(returnValue);
        }
    }
}
