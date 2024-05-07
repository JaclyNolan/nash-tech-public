using MVCDotNetAssignment.Models.Entities;
using MVCDotNetAssignment.Models.Repositories;

namespace MVCDotNetAssignment.Models.Repositories
{
    public interface IPeopleRepository
    {
        Task<Person> CreateAsync(Person person);
        Task<Person> UpdateAsync(Person person);
        Task<Person> DeleteAsync(Person person);
        Task<IEnumerable<Person>> GetAllAsync();
    }
    public class PeopleRepository : IPeopleRepository
    {
        private readonly IEnumerable<Person> _people = new List<Person>();

        public PeopleRepository() { 
            _people = PeopleDatabase._people;
        }

        public async Task<Person> CreateAsync(Person person)
        {
            throw new NotImplementedException();
        }

        public async Task<Person> UpdateAsync(Person person)
        {
            throw new NotImplementedException();
        }

        public async Task<Person> DeleteAsync(Person person)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            await Task.Delay(100);
            return _people;
        }

    }
}
