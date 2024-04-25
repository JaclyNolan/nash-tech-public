using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_FundamentalDay1
{
	public class Main
	{
		private List<Member> members;

		public Main(){
			members = new List<Member> {
			new Member()
			{
				FirstName = "Ánh",
				LastName = "Nguyễn",
				Age = 20,
				DoB = new DateTime(2003, 9, 19),
				Birthplace = "Bắc Giang",
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
			List<Member> result = new List<Member>();
			foreach (Member member in members)
			{
				if (member.Gender == true) result.Add(member); // true is male
			}
			return result;
		}

		public Member ReturnOldestMember()
		{
			Member oldest = members[0];
			foreach (Member member in members)
			{
				if (member.DoB.Ticks < oldest.DoB.Ticks)
				{
					oldest = member;
				}
			}
			return oldest;
		}

		public List<string> ReturnFullNameList()
		{
			List<string> result = new List<string> ();
			foreach (Member member in members)
			{
				result.Add($"{member.LastName} {member.FirstName}");
			}
			return result;
		}

		public List<Member> ReturnMembersAboveDobYear(int year)
		{
			List<Member> result = new List<Member>();
			foreach (Member member in members)
			{
				if (member.DoB.Year > year) result.Add(member);
			}
			return result;
		}
		public List<Member> ReturnMembersIsDobYear(int year)
		{
			List<Member> result = new List<Member>();
			foreach (Member member in members)
			{
				if (member.DoB.Year == year) result.Add(member);
			}
			return result;
		}
		public List<Member> ReturnMembersLessDobYear(int year)
		{
			List<Member> result = new List<Member>();
			foreach (Member member in members)
			{
				if (member.DoB.Year < year) result.Add(member);
			}
			return result;
		}

		public Member? ReturnFirstMemberInHanoi()
		{
			int i = 0;
			while (true)
			{
				Member member = members[i];

				if (member.Birthplace.Equals("Ha Noi")) return member; 

				if (i > members.Count) break;
				i++;
			}
			return null;
		}
	}
}
