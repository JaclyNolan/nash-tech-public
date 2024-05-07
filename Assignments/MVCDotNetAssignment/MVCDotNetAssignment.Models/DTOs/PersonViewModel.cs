using MVCDotNetAssignment.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCDotNetAssignment.Models.DTOs
{
    public class PersonViewModel
    {
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Gender")]
        public string Gender { get; set; }

        [DisplayName("Date of Birth")]
        public DateTime DoB { get; set; }

        [DisplayName("Birth place")]
        public string Birthplace { get; set; }

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [DisplayName("Age")]
        public int Age { get; set; }

        [DisplayName("Is Graduated?")]
        public bool IsGraduated { get; set; }

        public PersonViewModel(Person person)
        {
            FirstName = person.FirstName;
            LastName = person.LastName;
            Gender = Enum.GetName(typeof(Person.GenderEnum), person.Gender); // Convert enum to string
            DoB = person.DoB;
            Birthplace = person.Birthplace;
            PhoneNumber = person.PhoneNumber;
            Age = person.Age;
            IsGraduated = person.IsGraduated;
        }
    }
}
