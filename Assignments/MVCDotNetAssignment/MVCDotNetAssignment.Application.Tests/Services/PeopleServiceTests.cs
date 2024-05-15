using Moq;
using MVCDotNetAssignment.Domain.Repositories;
using MVCDotNetAssignment.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCDotNetAssignment.Domain.Entities;
using MVCDotNetAssignment.Application.DTOs;

namespace MVCDotNetAssignment.Application.Tests.Services
{
    public class PersonServiceTests
    {
        private readonly Mock<IPeopleRepository> _mockRepository;
        private readonly PersonService _personService;

        public PersonServiceTests()
        {
            _mockRepository = new Mock<IPeopleRepository>();
            _personService = new PersonService(_mockRepository.Object);
        }

        [Fact]
        public async Task CreatePersonAsync_NullPerson_ThrowsArgumentNullException()
        {
            // Arrange
            Person person = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _personService.CreatePersonAsync(person));
        }

        [Fact]
        public async Task CreatePersonAsync_ValidPerson_CallsRepositoryCreateAsync()
        {
            // Arrange
            var person = new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Gender = Person.GenderEnum.Male,
                DoB = new DateTime(1980, 1, 1),
                Birthplace = "New York",
                PhoneNumber = "1234567890",
                IsGraduated = true
            };

            // Act
            await _personService.CreatePersonAsync(person);

            // Assert
            _mockRepository.Verify(r => r.CreateAsync(person), Times.Once);
        }

        [Fact]
        public async Task GetOldestPersonAsync_EmptyRepository_ReturnsNull()
        {
            // Arrange
            var people = new List<Person>();

            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(people);

            // Act
            var result = await _personService.GetOldestPersonAsync();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetOldestPersonAsync_ReturnsOldestPerson()
        {
            // Arrange
            var people = new List<Person>
            {
                new Person { DoB = new DateTime(1980, 1, 1) },
                new Person { DoB = new DateTime(1970, 5, 15) }, // Oldest person
                new Person { DoB = new DateTime(2000, 3, 20) }
            };

            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(people);

            // Act
            var result = await _personService.GetOldestPersonAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1970, result.DoB.Year);
        }

        [Fact]
        public async Task GetOldestPersonAsync_ReturnsOneOfTheOldestPeopleWhenMultipleOldest()
        {
            // Arrange
            var people = new List<Person>
            {
                new Person { DoB = new DateTime(1980, 1, 1) },
                new Person { DoB = new DateTime(1970, 5, 15) }, // Oldest person
                new Person { DoB = new DateTime(1970, 7, 20) }  // Another oldest person
            };

            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(people);

            // Act
            var result = await _personService.GetOldestPersonAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1970, result.DoB.Year);
        }

        [Fact]
        public async Task GetPeopleAsync_EmptyRepository_ReturnsEmptyList()
        {
            // Arrange
            var people = new List<Person>();
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(people);

            // Act
            var result = await _personService.GetPeopleAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetPeopleAsync_ReturnsListOfPeople()
        {
            // Arrange
            var people = new List<Person>
        {
            new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Gender = Person.GenderEnum.Male,
                DoB = new DateTime(1980, 1, 1),
                Birthplace = "New York",
                PhoneNumber = "1234567890",
                IsGraduated = true
            },
            new Person
            {
                FirstName = "Jane",
                LastName = "Smith",
                Gender = Person.GenderEnum.Female,
                DoB = new DateTime(1990, 5, 15),
                Birthplace = "Los Angeles",
                PhoneNumber = "9876543210",
                IsGraduated = false
            }
        };

            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(people);

            // Act
            var result = await _personService.GetPeopleAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(people.Count, result.Count);
            for (int i = 0; i < people.Count; i++)
            {
                Assert.Equal(people[i].Id, result[i].Id);
                Assert.Equal(people[i].FirstName, result[i].FirstName);
                Assert.Equal(people[i].LastName, result[i].LastName);
                Assert.Equal(people[i].Gender, result[i].Gender);
                Assert.Equal(people[i].DoB, result[i].DoB);
                Assert.Equal(people[i].Birthplace, result[i].Birthplace);
                Assert.Equal(people[i].PhoneNumber, result[i].PhoneNumber);
                Assert.Equal(people[i].IsGraduated, result[i].IsGraduated);
            }
        }

        [Fact]
        public async Task GetPeopleByBirthYearAsync_InvalidOption_ThrowsArgumentException()
        {
            // Arrange
            var people = new List<Person>
        {
            new Person { DoB = new DateTime(1980, 1, 1) },
            new Person { DoB = new DateTime(1990, 5, 15) },
            new Person { DoB = new DateTime(2000, 3, 20) }
        };

            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(people);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _personService.GetPeopleByBirthYearAsync("invalid", 1990));
        }

        [Fact]
        public async Task GetPeopleByBirthYearAsync_OptionAbove_ReturnsPeopleWithBirthYearAboveGivenYear()
        {
            // Arrange
            var people = new List<Person>
            {
                new Person { DoB = new DateTime(1980, 1, 1) },
                new Person { DoB = new DateTime(1990, 5, 15) },
                new Person { DoB = new DateTime(2000, 3, 20) }
            };

            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(people);

            // Act
            var result = await _personService.GetPeopleByBirthYearAsync("above", 1990);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result); // Only one person should have DoB.Year > 1990
            Assert.Equal(2000, result[0].DoB.Year);
        }

        [Fact]
        public async Task GetPeopleByBirthYearAsync_OptionIs_ReturnsPeopleWithBirthYearEqualToGivenYear()
        {
            // Arrange
            var people = new List<Person>
            {
                new Person { DoB = new DateTime(1980, 1, 1) },
                new Person { DoB = new DateTime(1990, 5, 15) },
                new Person { DoB = new DateTime(2000, 3, 20) }
            };

            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(people);

            // Act
            var result = await _personService.GetPeopleByBirthYearAsync("is", 1990);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result); // Only one person should have DoB.Year == 1990
            Assert.Equal(1990, result[0].DoB.Year);
        }

        [Fact]
        public async Task GetPeopleByBirthYearAsync_OptionLess_ReturnsPeopleWithBirthYearLessThanGivenYear()
        {
            // Arrange
            var people = new List<Person>
            {
                new Person { DoB = new DateTime(1980, 1, 1) },
                new Person { DoB = new DateTime(1990, 5, 15) },
                new Person { DoB = new DateTime(2000, 3, 20) }
            };

            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(people);

            // Act
            var result = await _personService.GetPeopleByBirthYearAsync("less", 1990);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result); // Only one person should have DoB.Year < 1990
            Assert.Equal(1980, result[0].DoB.Year);
        }

        [Fact]
        public async Task GetPersonAsync_IdNotFound_ReturnsNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            var people = new List<Person>
        {
            new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "Smith",
                Gender = Person.GenderEnum.Female,
                DoB = new DateTime(1990, 5, 15),
                Birthplace = "Los Angeles",
                PhoneNumber = "9876543210",
                IsGraduated = false
            },
            new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "Alice",
                LastName = "Johnson",
                Gender = Person.GenderEnum.Other,
                DoB = new DateTime(2000, 10, 10),
                Birthplace = "Chicago",
                PhoneNumber = "5555555555",
                IsGraduated = true
            }
        };
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(people);

            // Act
            var result = await _personService.GetPersonAsync(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetPersonAsync_RepositoryReturnsEmptyList_ReturnsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Person>());

            // Act
            var result = await _personService.GetPersonAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetPersonAsync_ValidId_ReturnsPerson()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedPerson = new Person
            {
                Id = id,
                FirstName = "John",
                LastName = "Doe",
                Gender = Person.GenderEnum.Male,
                DoB = new DateTime(1980, 1, 1),
                Birthplace = "New York",
                PhoneNumber = "1234567890",
                IsGraduated = true
            };
            var people = new List<Person>
        {
            new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "Smith",
                Gender = Person.GenderEnum.Female,
                DoB = new DateTime(1990, 5, 15),
                Birthplace = "Los Angeles",
                PhoneNumber = "9876543210",
                IsGraduated = false
            },
            expectedPerson,
            new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "Alice",
                LastName = "Johnson",
                Gender = Person.GenderEnum.Other,
                DoB = new DateTime(2000, 10, 10),
                Birthplace = "Chicago",
                PhoneNumber = "5555555555",
                IsGraduated = true
            }
        };
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(people);

            // Act
            var result = await _personService.GetPersonAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedPerson.Id, result.Id);
            Assert.Equal(expectedPerson.FirstName, result.FirstName);
            Assert.Equal(expectedPerson.LastName, result.LastName);
            Assert.Equal(expectedPerson.Gender, result.Gender);
            Assert.Equal(expectedPerson.DoB, result.DoB);
            Assert.Equal(expectedPerson.Birthplace, result.Birthplace);
            Assert.Equal(expectedPerson.PhoneNumber, result.PhoneNumber);
            Assert.Equal(expectedPerson.IsGraduated, result.IsGraduated);
        }

        [Fact]
        public async Task GetFullNameAsync_ReturnsCorrectFullNameViewModelList()
        {
            // Arrange
            var people = new List<Person>
        {
            new Person { FirstName = "John", LastName = "Doe" },
            new Person { FirstName = "Jane", LastName = "Smith" },
            new Person { FirstName = "Emily", LastName = "Johnson" }
        };

            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(people);

            // Act
            var result = await _personService.GetFullNameAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal("Doe John", result[0].FullName);
            Assert.Equal("Smith Jane", result[1].FullName);
            Assert.Equal("Johnson Emily", result[2].FullName);
        }

        [Fact]
        public async Task GetFullNameAsync_EmptyRepository_ReturnsEmptyList()
        {
            // Arrange
            var people = new List<Person>();

            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(people);

            // Act
            var result = await _personService.GetFullNameAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task DeletePersonAsync_ValidId_DeletesPerson()
        {
            // Arrange
            var personId = Guid.NewGuid();
            var people = new List<Person>
        {
            new Person { Id = personId, FirstName = "John", LastName = "Doe", DoB = new DateTime(1980, 1, 1) },
            new Person { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith", DoB = new DateTime(1990, 5, 15) }
        };

            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(people);
            _mockRepository.Setup(r => r.DeleteAsync(personId)).Returns(Task.CompletedTask);

            // Act
            await _personService.DeletePersonAsync(personId);

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync(personId), Times.Once);
        }

        [Fact]
        public async Task DeletePersonAsync_InvalidId_ThrowsArgumentException()
        {
            // Arrange
            var people = new List<Person>
        {
            new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", DoB = new DateTime(1980, 1, 1) },
            new Person { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith", DoB = new DateTime(1990, 5, 15) }
        };

            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(people);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _personService.DeletePersonAsync(Guid.NewGuid()));

            // Verify that DeleteAsync was never called
            _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
        }
        [Fact]
        public async Task UpdatePersonAsync_PersonExists_UpdatesPerson()
        {
            // Arrange
            var personId = Guid.NewGuid();
            var person = new Person { Id = personId, FirstName = "John", LastName = "Doe" };
            var updatedPerson = new Person { Id = personId, FirstName = "Jane", LastName = "Doe" };

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Person> { person });
            _mockRepository.Setup(repo => repo.UpdateAsync(personId, updatedPerson)).Returns(Task.CompletedTask);

            // Act
            await _personService.UpdatePersonAsync(personId, updatedPerson);

            // Assert
            _mockRepository.Verify(repo => repo.UpdateAsync(personId, updatedPerson), Times.Once);
        }

        [Fact]
        public async Task UpdatePersonAsync_PersonDoesNotExist_ThrowsArgumentException()
        {
            // Arrange
            var personId = Guid.NewGuid();
            var updatedPerson = new Person { Id = personId, FirstName = "Jane", LastName = "Doe" };

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Person>());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _personService.UpdatePersonAsync(personId, updatedPerson));
            Assert.Equal("Person not found (Parameter 'id')", exception.Message);
        }
        [Fact]
        public async Task GetPeopleByGenderAsync_ReturnsCorrectPeople()
        {
            // Arrange
            var gender = (int)Person.GenderEnum.Male;
            var people = new List<Person>
        {
            new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Gender = Person.GenderEnum.Male },
            new Person { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Doe", Gender = Person.GenderEnum.Female },
            new Person { Id = Guid.NewGuid(), FirstName = "Jim", LastName = "Beam", Gender = Person.GenderEnum.Male }
        };

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(people);

            // Act
            var result = await _personService.GetPeopleByGenderAsync(gender);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, person => Assert.Equal(Person.GenderEnum.Male, person.Gender));
        }

        [Fact]
        public async Task GetPeopleByGenderAsync_NoMatchingGender_ReturnsEmptyList()
        {
            // Arrange
            var gender = (int)Person.GenderEnum.Other;
            var people = new List<Person>
        {
            new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Gender = Person.GenderEnum.Male },
            new Person { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Doe", Gender = Person.GenderEnum.Female }
        };

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(people);

            // Act
            var result = await _personService.GetPeopleByGenderAsync(gender);

            // Assert
            Assert.Empty(result);
        }
    }
}