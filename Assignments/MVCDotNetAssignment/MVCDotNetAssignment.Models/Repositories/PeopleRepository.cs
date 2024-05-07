using MVCDotNetAssignment.Models.Entities;
using MVCDotNetAssignment.Models.Repositories;

namespace MVCDotNetAssignment.Models.Repositories
{
    public interface IPeopleRepository
    {
        Task CreateAsync(Person person);
        Task UpdateAsync(string id, Person person);
        Task DeleteAsync(string id);
        Task<IEnumerable<Person>> GetAllAsync();
    }
    public class PeopleRepository : IPeopleRepository
    {
        private readonly List<Person> _people;

        public PeopleRepository() { 
            _people = PeopleDatabase._people;
        }

        public async Task CreateAsync(Person person)
        {
            await Task.Delay(100);
            _people.Add(person);
        }
        public async Task UpdateAsync(string id, Person person)
        {
            await Task.Delay(100); //Simulate database
            Person? personToUpdate = _people.Find(p => p.Id == new Guid(id));
            if (personToUpdate == null) throw new ArgumentException("Person not found", nameof(id));
            foreach (var prop in personToUpdate.GetType().GetProperties())
            {
                if (prop.CanWrite)
                {
                    prop.SetValue(personToUpdate, prop.GetValue(person));
                }
            }
        }

        public async Task DeleteAsync(string id)
        {
            await Task.Delay(100); //Simulate database
            int personToRemoveIndex = _people.FindIndex(p => p.Id == new Guid(id));
            if (personToRemoveIndex == -1) throw new ArgumentException("Person not found", nameof(id));

            _people.RemoveAt(personToRemoveIndex);
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            await Task.Delay(100);
            return _people;
        }

    }
}
