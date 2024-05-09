using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_ASPNET_Assignment1.Models.Entities
{
    public class Person : EntityBase
    {
        [Key]
        public Guid Id {  get; set; }
        public Person()
        {
            Id = Guid.NewGuid(); // Generate a new unique identifier
        }

        [DisplayName("First Name")]
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name must be between 1 and 50 characters", MinimumLength = 1)]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name must be between 1 and 50 characters", MinimumLength = 1)]
        public string LastName { get; set; }

        //To-do: Gender should have use model binding to string or int in http request
        [Required(ErrorMessage = "Gender is required")]
        public GenderEnum Gender { get; set; } = GenderEnum.Unknown;

        [DisplayName("Date of Birth")]
        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DoB { get; set; }

        [DisplayName("Birthplace")]
        [Required(ErrorMessage = "Birthplace is required")]
        [StringLength(100, ErrorMessage = "Birthplace must be between 1 and 100 characters", MinimumLength = 1)]
        public string Birthplace { get; set; }

        public override string ToString()
        {
            return $"First Name: {FirstName}\n" +
                   $"Last Name: {LastName}\n" +
                   $"Gender: {Gender}\n" +
                   $"Date of Birth: {DoB.ToString("dd/MM/yyyy")}\n" +
                   $"Birth place: {Birthplace}\n";
        }
        public enum GenderEnum
        {
            Unknown = 0,
            Male = 1,
            Female = 2,
            Other = 3
        }
    }
}
