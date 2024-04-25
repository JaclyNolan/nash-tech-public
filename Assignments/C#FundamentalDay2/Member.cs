using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_FundamentalDay2
{
	public class Member
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public bool Gender { get; set; }
		public DateTime DoB { get; set; }
		public string Birthplace { get; set; }
		public string PhoneNumber { get; set; }
		public int Age { get; set; }
		public bool IsGraduated { get; set; }

		public override string ToString()
		{
			string Gender = this.Gender ? "Male" : "Female";
			string IsGraduated = this.IsGraduated ? "Yes" : "No";
			return $"First Name: {this.FirstName}\n" +
				   $"Last Name: {this.LastName}\n" +
				   $"Age: {this.Age}\n" +
				   $"Gender: {Gender}\n" +
				   $"Date of Birth: {this.DoB.ToString("dd/MM/yyyy")}\n" +
				   $"Birth place: {this.Birthplace}\n" +
				   $"Phone Number: {this.PhoneNumber}\n" +
				   $"Is Graduated: {IsGraduated}\n";
		}
	}
}
