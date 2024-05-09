using static API_ASPNET_Assignment1.Models.Entities.Person;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using API_ASPNET_Assignment1.Models.Entities;

namespace API_ASPNET_Assignment1.WebAPI.DTOs
{
    public class PersonViewModel
    {
        public Guid Id { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        //To-do: Gender should have use model binding to string or int in http request
        [Required(ErrorMessage = "Gender is required")]
        public GenderEnum Gender { get; set; } = GenderEnum.Unknown;

        [DisplayName("Date of Birth")]
        public DateTime DoB { get; set; }

        [DisplayName("Birthplace")]
        public string Birthplace { get; set; }
    }
}
