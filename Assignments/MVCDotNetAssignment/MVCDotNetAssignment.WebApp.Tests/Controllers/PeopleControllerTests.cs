using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MVCDotNetAssignment.WebApp.Controllers;
using MVCDotNetAssignment.Domain.Entities;
using MVCDotNetAssignment.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCDotNetAssignment.Application.DTOs;
using DocumentFormat.OpenXml.Office2013.Word;
using Person = MVCDotNetAssignment.Domain.Entities.Person;

namespace MVCDotNetAssignment.WebApp.Tests.Controllers
{
    public class PeopleControllerTests
    {
        private readonly PeopleController _controller;
        private readonly Mock<IPeopleService> _mockPeopleService;
        public PeopleControllerTests()
        {
            _mockPeopleService = new Mock<IPeopleService>();
            _controller = new PeopleController(_mockPeopleService.Object);
        }

        [Fact]
        public void Create_Get_ReturnsViewResult()
        {
            // Act
            var result = _controller.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Create_Post_InvalidModel_ReturnsView()
        {
            // Arrange
            var person = new Person { FirstName = "John", LastName = "Doe", Gender = Person.GenderEnum.Male, DoB = DateTime.Parse("1990-01-01") };
            _controller.ModelState.AddModelError("Error", "Sample error");

            // Act
            var result = await _controller.Create(person);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(person, viewResult.Model);
        }

        [Fact]
        public async Task Create_Post_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var person = new Person { FirstName = "John", LastName = "Doe", Gender = Person.GenderEnum.Male, DoB = DateTime.Parse("1990-01-01") };

            // Act
            var result = await _controller.Create(person);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
        [Fact]
        public async Task Delete_PersonDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var personId = Guid.NewGuid();
            _mockPeopleService.Setup(svc => svc.GetPersonAsync(personId)).ReturnsAsync((Person)null);

            // Act
            var result = await _controller.Delete(personId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_PersonExists_ReturnsViewResult()
        {
            // Arrange
            var personId = Guid.NewGuid();
            var person = new Person { Id = personId, FirstName = "John", LastName = "Doe" };
            _mockPeopleService.Setup(svc => svc.GetPersonAsync(personId)).ReturnsAsync(person);

            // Act
            var result = await _controller.Delete(personId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public async Task Details_PersonDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var personId = Guid.NewGuid();
            _mockPeopleService.Setup(svc => svc.GetPersonAsync(personId)).ReturnsAsync((Person)null);

            // Act
            var result = await _controller.Details(personId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_PersonExists_ReturnsViewResult()
        {
            // Arrange
            var personId = Guid.NewGuid();
            var person = new Person { Id = personId, FirstName = "John", LastName = "Doe" };
            _mockPeopleService.Setup(svc => svc.GetPersonAsync(personId)).ReturnsAsync(person);

            // Act
            var result = await _controller.Details(personId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(person, viewResult.Model);
        }
        [Fact]
        public async Task Edit_Get_PersonDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var personId = Guid.NewGuid();
            _mockPeopleService.Setup(svc => svc.GetPersonAsync(personId)).ReturnsAsync((Person)null);

            // Act
            var result = await _controller.Edit(personId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Get_PersonExists_ReturnsViewResult()
        {
            // Arrange
            var personId = Guid.NewGuid();
            var person = new Person { Id = personId, FirstName = "John", LastName = "Doe" };
            _mockPeopleService.Setup(svc => svc.GetPersonAsync(personId)).ReturnsAsync(person);

            // Act
            var result = await _controller.Edit(personId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(person, viewResult.Model);
        }
        [Fact]
        public async Task Edit_Post_InvalidModel_ReturnsView()
        {
            // Arrange
            var personId = Guid.NewGuid();
            var person = new Person { Id = personId, FirstName = "John", LastName = "Doe", Gender = Person.GenderEnum.Male, DoB = DateTime.Parse("1990-01-01") };
            _controller.ModelState.AddModelError("Error", "Sample error");

            _mockPeopleService.Setup(s => s.GetPersonAsync(personId)).ReturnsAsync((Person)null);


            // Act
            var result = await _controller.Edit(personId, person);

            // Assert
            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Post_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var personId = Guid.NewGuid();
            var person = new Person { Id = personId, FirstName = "John", LastName = "Doe", Gender = Person.GenderEnum.Male, DoB = DateTime.Parse("1990-01-01") };

            _mockPeopleService.Setup(svc => svc.GetPersonAsync(personId)).ReturnsAsync(person);

            // Act
            var result = await _controller.Edit(personId, person);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
        [Fact]
        public async Task GetFullNamesAsync_ReturnsViewResult()
        {
            // Arrange
            var fullNames = new List<FullNameViewModel>
        {
            new FullNameViewModel { FullName = "John Doe" },
            new FullNameViewModel { FullName = "Jane Doe" }
        };
            _mockPeopleService.Setup(svc => svc.GetFullNameAsync()).ReturnsAsync(fullNames);

            // Act
            var result = await _controller.GetFullNamesAsync();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ViewJson", viewResult.ViewName);
            Assert.Equal(fullNames, viewResult.Model);
        }

        [Fact]
        public async Task GetOldestPersonAsync_ReturnsViewResult()
        {
            // Arrange
            var person = new Person { FirstName = "John", LastName = "Doe", Gender = Person.GenderEnum.Male, DoB = DateTime.Parse("1950-01-01") };
            _mockPeopleService.Setup(svc => svc.GetOldestPersonAsync()).ReturnsAsync(person);

            // Act
            var result = await _controller.GetOldestPersonAsync();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ViewObject", viewResult.ViewName);
            Assert.Equal(person, viewResult.Model);
        }

        [Fact]
        public async Task GetPeopleByGenderAsync_ReturnsViewResult()
        {
            // Arrange
            var gender = (int)Person.GenderEnum.Male;
            var people = new List<Person>
        {
            new Person { FirstName = "John", LastName = "Doe", Gender = Person.GenderEnum.Male },
            new Person { FirstName = "Jim", LastName = "Beam", Gender = Person.GenderEnum.Male }
        };
            _mockPeopleService.Setup(svc => svc.GetPeopleByGenderAsync(gender)).ReturnsAsync(people);

            // Act
            var result = await _controller.GetPeopleByGenderAsync(gender);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ViewJson", viewResult.ViewName);
            Assert.Equal(people, viewResult.Model);
        }

        [Fact]
        public async Task GetPeopleByYearAsync_ReturnsJsonResult()
        {
            // Arrange
            var operation = "above";
            var year = 1980;
            var people = new List<Person>
        {
            new Person { FirstName = "John", LastName = "Doe", DoB = DateTime.Parse("1990-01-01") },
            new Person { FirstName = "Jane", LastName = "Doe", DoB = DateTime.Parse("1985-01-01") }
        };
            _mockPeopleService.Setup(svc => svc.GetPeopleByBirthYearAsync(operation, year)).ReturnsAsync(people);

            // Act
            var result = await _controller.GetPeopleByYearAsync(operation, year);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(people, jsonResult.Value);
        }

        [Fact]
        public async Task Index_ReturnsViewResult()
        {
            // Arrange
            var people = new List<Person>
        {
            new Person { FirstName = "John", LastName = "Doe", DoB = DateTime.Parse("1990-01-01") },
            new Person { FirstName = "Jane", LastName = "Doe", DoB = DateTime.Parse("1985-01-01") }
        };
            _mockPeopleService.Setup(svc => svc.GetPeopleAsync()).ReturnsAsync(people);

            // Act
            var result = await _controller.Index(null, null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("List", viewResult.ViewName);
            List<Person> actualPeople = Assert.IsAssignableFrom<List<Person>>(viewResult.Model);
            Assert.Contains(people[0], actualPeople);
            Assert.Contains(people[1], actualPeople);
        }
    }
}
