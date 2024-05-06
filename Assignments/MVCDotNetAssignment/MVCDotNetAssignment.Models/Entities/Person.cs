namespace MVCDotNetAssignment.Models.Entities
{
    public class Person
    {
        public Guid Id { get; set; }
        //Add attribute names here for other businesses to use
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public GenderEnum Gender { get; set; } = GenderEnum.Unknown;
        public DateTime DoB { get; set; }
        public string Birthplace { get; set; }
        public string PhoneNumber { get; set; }
        public int Age => CalculateAge();
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
