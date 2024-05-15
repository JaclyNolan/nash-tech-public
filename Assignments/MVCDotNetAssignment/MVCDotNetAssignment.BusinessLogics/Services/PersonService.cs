using MVCDotNetAssignment.Domain.Repositories;
using MVCDotNetAssignment.Application.DTOs;
using MVCDotNetAssignment.Domain.Entities;
using MVCDotNetAssignment.Application.Common;
using DocumentFormat.OpenXml.Office2013.Word;
using Person = MVCDotNetAssignment.Domain.Entities.Person;

namespace MVCDotNetAssignment.Application.Services
{
    public interface IPeopleService
    {
        Task CreatePersonAsync(Person person);

        Task DeletePersonAsync(Guid id);

        Task<List<FullNameViewModel>> GetFullNameAsync();

        Task<Person?> GetOldestPersonAsync();

        //Task<List<Person>> GetPeopleByYearAsync(string operation, int year);
        Task<List<Person>> GetPeopleAsync();

        Task<List<Person>> GetPeopleByBirthYearAsync(string operation, int year);

        Task<List<Person>> GetPeopleByGenderAsync(int gender);
        Task<Person?> GetPersonAsync(Guid id);
        Task UpdatePersonAsync(Guid id, Person person);
    }
    public class PersonService(IPeopleRepository peopleRepository) : IPeopleService
    {
        private readonly IPeopleRepository _peopleRepository = peopleRepository;

        public async Task CreatePersonAsync(Person person)
        {
            ArgumentNullException.ThrowIfNull(person);
            //To-do: validate data here later
            await _peopleRepository.CreateAsync(person);
        }

        public async Task DeletePersonAsync(Guid id)
        {
            int personToRemoveIndex = (await _peopleRepository.GetAllAsync()).ToList().FindIndex(p => p.Id == id);
            if (personToRemoveIndex == -1) throw new ArgumentException("Person not found", nameof(id));

            await _peopleRepository.DeleteAsync(id);
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
        public async Task<Person?> GetOldestPersonAsync()
        {
            var people = await _peopleRepository.GetAllAsync();
            return people.FirstOrDefault(person => person.Age == people.Max(person => person.Age));
        }

        public async Task<List<Person>> GetPeopleAsync()
        {
            var people = await _peopleRepository.GetAllAsync();
            return people.ToList();
        }

        public async Task<List<Person>> GetPeopleByBirthYearAsync(string operation, int year)
        {
            var people = await _peopleRepository.GetAllAsync();

            switch (operation)
            {
                case "above":
                    return people.Where(person => person.DoB.Year > year).ToList();
                case "is":
                    return people.Where(person => person.DoB.Year == year).ToList();
                case "less":
                    return people.Where(person => person.DoB.Year < year).ToList();
                default:
                    throw new ArgumentException("Invalid option", nameof(operation));
            }
        }

        public async Task<List<Person>> GetPeopleByGenderAsync(int gender)
        {
            var people = await _peopleRepository.GetAllAsync();
            return people.Where(person => person.Gender == (Person.GenderEnum)gender).ToList();
        }

        public async Task<Person?> GetPersonAsync(Guid id)
        {
            var people = await _peopleRepository.GetAllAsync();
            return people.FirstOrDefault(person => person.Id == id);
        }
        public async Task UpdatePersonAsync(Guid id, Person person)
        {
            int personToRemoveIndex = (await _peopleRepository.GetAllAsync()).ToList().FindIndex(p => p.Id == id);
            if (personToRemoveIndex == -1) throw new ArgumentException("Person not found", nameof(id));
            await _peopleRepository.UpdateAsync(id, person);
        }
    }
}
