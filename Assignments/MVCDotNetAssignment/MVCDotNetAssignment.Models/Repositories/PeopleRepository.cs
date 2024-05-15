using MVCDotNetAssignment.Domain.Common;
using MVCDotNetAssignment.Domain.Entities;
using MVCDotNetAssignment.Domain.Repositories;

namespace MVCDotNetAssignment.Domain.Repositories
{
    public interface IPeopleRepository
    {
        Task CreateAsync(Person person);
        Task DeleteAsync(Guid id);

        Task<IEnumerable<Person>> GetAllAsync();

        Task UpdateAsync(Guid id, Person person);
    }
    public class PeopleRepository : IPeopleRepository
    {
        private readonly List<Person> _people = [];

        public PeopleRepository() { 
            _people = PeopleDatabase._people;
        }

        public async Task CreateAsync(Person person)
        {
            await Task.Delay(100);
            _people.Add(person);
        }
        public async Task DeleteAsync(Guid id)
        {
            await Task.Delay(100); //Simulate database
            int personToRemoveIndex = _people.FindIndex(p => p.Id == id);
            if (personToRemoveIndex == -1) throw new ArgumentException("Person not found", nameof(id));

            _people.RemoveAt(personToRemoveIndex);
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            await Task.Delay(100);
            if (_people.IsNullOrEmpty()) return [];
            return _people;
        }

        public async Task UpdateAsync(Guid id, Person person)
        {
            await Task.Delay(100); //Simulate database
            Person? personToUpdate = _people.Find(p => p.Id == id);
            if (personToUpdate == null) throw new ArgumentException("Person not found", nameof(id));
            foreach (var prop in personToUpdate.GetType().GetProperties())
            {
                if (prop.CanWrite)
                {
                    prop.SetValue(personToUpdate, prop.GetValue(person));
                }
            }
        }
    }
}
