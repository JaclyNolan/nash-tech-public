using MVCDotNetAssignment.Models.Entities;

namespace MVCDotNetAssignment.Models.Repositories
{
    public static class PeopleDatabase
    {
        public static IEnumerable<Person> _people = new List<Person>
        {
            new Person()
            {
                Id = Guid.NewGuid(),
                FirstName = "Ánh",
                LastName = "Nguyễn",
                DoB = new DateTime(2003, 9, 19),
                Birthplace = "Bắc Giang",
                Gender = Person.GenderEnum.Male,
                IsGraduated = true,
                PhoneNumber = "0123456789",
            },
            new Person()
            {
                Id = Guid.NewGuid(),
                FirstName = "Hoà",
                LastName = "Nguyễn",
                DoB = new DateTime(2002, 11, 5),
                Birthplace = "Viet Yen",
                Gender = Person.GenderEnum.Female,
                IsGraduated = true,
                PhoneNumber = "0123456789",
            },
            new Person()
            {
                Id = Guid.NewGuid(),
                FirstName = "Phương",
                LastName = "Nguyễn",
                DoB = new DateTime(2001, 4, 6),
                Birthplace = "Bac Giang",
                Gender = Person.GenderEnum.Male,
                IsGraduated = true,
                PhoneNumber = "0123456789",
            },
            new Person()
            {
                Id = Guid.NewGuid(),
                FirstName = "Phúc",
                LastName = "Nguyễn",
                DoB = new DateTime(1998, 10, 20),
                Birthplace = "Ha Noi",
                Gender = Person.GenderEnum.Male,
                IsGraduated = true,
                PhoneNumber = "0123456789",
            },
            new Person()
            {
                Id = Guid.NewGuid(),
                FirstName = "Hoàng",
                LastName = "Nguyễn",
                DoB = new DateTime(2000, 1, 5),
                Birthplace = "Ho Chi Minh",
                Gender = Person.GenderEnum.Other,
                IsGraduated = true,
                PhoneNumber = "0123456789",
            }
        };
    }
}
