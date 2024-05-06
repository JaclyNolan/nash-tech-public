using MVCDotNetAssignment.BusinessLogics.Repositories;
using MVCDotNetAssignment.Models.DTOs;
using MVCDotNetAssignment.Models.Entities;

namespace MVCDotNetAssignment.BusinessLogics.Services
{
    public interface IPeopleBusinessLogics
    {
        Task<List<Person>> GetPeopleByGenderAsync(int gender);
        Task<Person> GetOldestPersonAsync();
        Task<List<FullNameViewModel>> GetFullNameAsync();
        Task<List<Person>> GetPeopleBirthYearAboveAsync(int year);
        Task<List<Person>> GetPeopleBirthYearIsAsync(int year);
        Task<List<Person>> GetPeopleBirthYearLessAsync(int year);
        //Task<List<Person>> GetPeopleByYearAsync(string operation, int year);
        Task<List<Person>> GetPeopleAsync();
    }
    public class PeopleBusinessLogics : IPeopleBusinessLogics
    {
        private readonly IPeopleRepository _peopleRepository;
        public PeopleBusinessLogics(IPeopleRepository peopleRepository)
        {
            _peopleRepository = peopleRepository;
        }

        public async Task<List<FullNameViewModel>> GetFullNameAsync()
        {
            var people = await _peopleRepository.GetAllAsync();
            return people.Select(person => new FullNameViewModel
            {
                FullName = $"{person.LastName} {person.FirstName}"
            }).ToList();
        }

        //To-do: add DTOs to remove Id and reformat DateTime and Gender
        //Also do I have to do that for all methods in here??? Ask your mentor
        public async Task<Person> GetOldestPersonAsync()
        {
            var people = await _peopleRepository.GetAllAsync();
            return people.Where(person => person.Age == people.Max(person => person.Age)).First();
        }

        public async Task<List<Person>> GetPeopleAsync()
        {
            var people = await _peopleRepository.GetAllAsync();
            return people.ToList();
        }

        public async Task<List<Person>> GetPeopleBirthYearAboveAsync(int year)
        {
            var people = await _peopleRepository.GetAllAsync();
            return people.Where(person => person.DoB.Year > year).ToList();
        }
        //public async Task<List<Person>> GetPeopleByYearAsync(string operation, int year)
        //{

        //}

        public async Task<List<Person>> GetPeopleBirthYearIsAsync(int year)
        {
            var people = await _peopleRepository.GetAllAsync();
            return people.Where(person => person.DoB.Year == year).ToList();
        }

        public async Task<List<Person>> GetPeopleBirthYearLessAsync(int year)
        {
            var people = await _peopleRepository.GetAllAsync();
            return people.Where(person => person.DoB.Year < year).ToList();
        }

        public async Task<List<Person>> GetPeopleByGenderAsync(int gender)
        {
            var people = await _peopleRepository.GetAllAsync();
            return people.Where(person => person.Gender == (Person.GenderEnum)gender).ToList();
        }
    }
}
