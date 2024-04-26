using C_FundamentalDay2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_FundamentalDay2
{
	public class MainLINQ
	{
		private List<Member> members;

		public MainLINQ()
		{
			members = new List<Member> {
				new Member()
				{
					FirstName = "Ánh",
					LastName = "Nguyễn",
					Age = 20,
					DoB = new DateTime(2003, 9, 19),
					Birthplace = "Bac Giang",
					Gender =  true,
					IsGraduated = true,
					PhoneNumber = "0123456789",
				},
				new Member()
				{
					FirstName = "Hoà",
					LastName = "Nguyễn",
					Age = 21,
					DoB = new DateTime(2002, 11, 5),
					Birthplace = "Viet Yen",
					Gender =  true,
					IsGraduated = true,
					PhoneNumber = "0123456789",
				},
				new Member()
				{
					FirstName = "Phương",
					LastName = "Nguyễn",
					Age = 22,
					DoB = new DateTime(2001, 4, 6),
					Birthplace = "Bac Giang",
					Gender =  true,
					IsGraduated = true,
					PhoneNumber = "0123456789",
				},
				new Member()
				{
					FirstName = "Phúc",
					LastName = "Nguyễn",
					Age = 25,
					DoB = new DateTime(1998, 10, 20),
					Birthplace = "Ha Noi",
					Gender =  true,
					IsGraduated = true,
					PhoneNumber = "0123456789",
				},
				new Member()
				{
					FirstName = "Hoàng",
					LastName = "Nguyễn",
					Age = 23,
					DoB = new DateTime(2000, 1, 5),
					Birthplace = "Ho Chi Minh",
					Gender =  true,
					IsGraduated = true,
					PhoneNumber = "0123456789",
				}
			};
		}

		public List<Member> ReturnMaleMembers()
		{
			return members.Where(member => member.Gender == true).ToList();
		}

		public Member ReturnOldestMember()
		{
			return members.Where(member => member.DoB.Ticks == members.Min(member => member.DoB.Ticks)).First();
		}

		public List<string> ReturnFullNameList()
		{
			return members.Select(member => $"{member.LastName} {member.FirstName}").ToList();
		}

		public List<Member> ReturnMembersAboveDobYear(int year)
		{
			return members.Where(member => member.DoB.Year > year).ToList();
		}
		public List<Member> ReturnMembersIsDobYear(int year)
		{
			return members.Where(member => member.DoB.Year == year).ToList();
		}
		public List<Member> ReturnMembersLessDobYear(int year)
		{
			return members.Where(member => member.DoB.Year < year).ToList();
		}

		public Member? ReturnFirstMemberInHanoi()
		{
			return members.FirstOrDefault(member => member.Birthplace == "Ha Noi");
		}
	}
}
