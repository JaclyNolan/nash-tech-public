using API_ASPNET_Assignment1.BusinessLogic.Exceptions;
using API_ASPNET_Assignment1.Models.DTOs;
using API_ASPNET_Assignment1.Models.Entities;
using API_ASPNET_Assignment1.WebAPI.DTOs;
using AutoMapper;
using Bogus;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using Person = API_ASPNET_Assignment1.Models.Entities.Person;

namespace API_ASPNET_Assignment1.BusinessLogic.Services
{
    public interface IPersonBusinessLogic
    {
        Task<List<Person>> GetPeopleAsync(PersonGetRequest personGetRequest);
        Task<Person?> GetPersonAsync(Guid id);
        Task AddPeopleAsync(List<Person> people);
        List<Person> GeneratePeopleAsync(int count);
        Task UpdatePersonAsync(Guid id, PersonUpdateRequest personUpdateRequest);
        Task<Guid> CreatePersonAsync(PersonCreateRequest personCreateRequest);
        Task DeletePersonAsync(Guid id);
    }
    public class PersonBusinessLogic : IPersonBusinessLogic
    {
        private readonly PersonContext _personContext;
        private readonly IMapper _mapper;
        public PersonBusinessLogic(PersonContext personContext, IMapper mapper)
        {
            _personContext = personContext;
            _mapper = mapper;
        }
        public async Task<List<Person>> GetPeopleAsync(PersonGetRequest personGetRequest)
        {
            IQueryable<Person> query = _personContext.People;
            if (!string.IsNullOrEmpty(personGetRequest.FirstName))
                query = query.Where(p => p.FirstName == personGetRequest.FirstName);
            if (!string.IsNullOrEmpty(personGetRequest.LastName))
                query = query.Where(p => p.LastName== personGetRequest.LastName);
            if (personGetRequest.Gender != null)
            {
                Person.GenderEnum gender = (Person.GenderEnum)personGetRequest.Gender;
                query = query.Where(p => p.Gender == gender);
            }
            if (!string.IsNullOrEmpty(personGetRequest.Birthplace))
                query = query.Where(p => p.Birthplace == personGetRequest.Birthplace);

            return await query.ToListAsync();
        }

        public async Task AddPeopleAsync(List<Person> people)
        {
            _personContext.AddRange(people);
            await _personContext.SaveChangesAsync();
        }

        public List<Person> GeneratePeopleAsync(int count)
        {
            var faker = new Faker<Person>()
                .RuleFor(p => p.FirstName, f => f.Person.FirstName)
                .RuleFor(p => p.LastName, f => f.Person.LastName)
                .RuleFor(p => p.Gender, f => f.PickRandomWithout<Person.GenderEnum>(Person.GenderEnum.Unknown))
                .RuleFor(p => p.DoB, f => f.Date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-18)))
                .RuleFor(p => p.Birthplace, f => f.Address.City());

            return faker.Generate(count);
        }
        public async Task<Person?> GetPersonAsync(Guid id)
        {
            return await _personContext.People.FirstOrDefaultAsync(p => p.Id.Equals(id));
        }

        public async Task UpdatePersonAsync(Guid id, PersonUpdateRequest personUpdateRequest)
        {
            Person? person = await GetPersonAsync(id);
            if (person == null) throw new PersonNotFoundException();
            _mapper.Map(personUpdateRequest, person);
            _personContext.Entry(person).State = EntityState.Modified;

            try
            {
                await _personContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public async Task<Guid> CreatePersonAsync(PersonCreateRequest personCreateRequest)
        {
            var person = _mapper.Map<Person>(personCreateRequest);
            _personContext.People.Add(person);
            await _personContext.SaveChangesAsync();
            return person.Id;
        }

        public async Task DeletePersonAsync(Guid id)
        {
            Person? person = await GetPersonAsync(id);
            if (person == null) throw new PersonNotFoundException();

            _personContext.People.Remove(person);
            await _personContext.SaveChangesAsync();
        }
    }
}
