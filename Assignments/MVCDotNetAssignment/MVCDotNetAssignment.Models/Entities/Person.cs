using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVCDotNetAssignment.Models.Entities
{
    public class Person
    {
        public Guid Id { get; }
        public Person()
        {
            Id = Guid.NewGuid(); // Generate a new unique identifier
        }

        //Add attribute names here for other businesses to use
        [DisplayName("First Name")]
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name must be between 1 and 50 characters", MinimumLength = 1)]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name must be between 1 and 50 characters", MinimumLength = 1)]
        public string LastName { get; set; }

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

        [DisplayName("Phone Number")]
        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^\d{10,12}$", ErrorMessage = "Phone number must be 10 to 12 digits")]
        public string PhoneNumber { get; set; }

        public int Age => CalculateAge();

        [DisplayName("Graduated?")]
        public bool IsGraduated { get; set; }

        public override string ToString()
        {
            string IsGraduated = this.IsGraduated ? "Yes" : "No";
            return $"First Name: {FirstName}\n" +
                   $"Last Name: {LastName}\n" +
                   $"Age: {Age}\n" +
                   $"Gender: {Gender}\n" +
                   $"Date of Birth: {DoB.ToString("dd/MM/yyyy")}\n" +
                   $"Birth place: {Birthplace}\n" +
                   $"Phone Number: {PhoneNumber}\n" +
                   $"Is Graduated: {IsGraduated}\n";
        }

        private int CalculateAge()
        {
            DateTime now = DateTime.Now;
            int age = now.Year - DoB.Year;
            if (now.Month < DoB.Month || now.Month == DoB.Month && now.Day < DoB.Day)
            {
                age--;
            }
            return age;
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
