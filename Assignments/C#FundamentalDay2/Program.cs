using C_FundamentalDay2;
using System.ComponentModel;

PrintMenu();
int option;
int year;
bool shouldExit = false;
MainLINQ main = new MainLINQ();
do
{
	BreakLine();
	if (!int.TryParse(Console.ReadLine(), out option))
	{
		Console.WriteLine("Invalid input");
		continue;
	}

	switch (option)
	{
		case 0:
			shouldExit = true;
			break;
		case 1:
			PrintList(main.ReturnMaleMembers());
			break;
		case 2:
			PrintOne(main.ReturnOldestMember());
			break;
		case 3:
			foreach (string s in main.ReturnFullNameList())
				Console.WriteLine($"{s}");
			break;
		case 4:
			Return3List();
			break;
		case 5:
			Member? member = main.ReturnFirstMemberInHanoi();
			if (member != null)
			{
				PrintOne(member);
				break;
			}
			Console.WriteLine("No member is from Ha Noi");
			break;
		default:
			Console.WriteLine($"No option is signed to {option}");
			break;
	}
} while (shouldExit == false);

void PrintMenu()
{
	Console.WriteLine("Welcome to my program");
	Console.WriteLine("1. Return a list of members who is Male");
	Console.WriteLine("2. Return the oldest member");
	Console.WriteLine("3. Return a new list that contains Full Name only");
	Console.WriteLine("4. Return 3 lists");
	Console.WriteLine("5. Return the first member who was born in Ha Noi.");
	Console.WriteLine("0. Exit");
}

void Return3List()
{
	Console.WriteLine("1. Return a list of members who has birth year above the provided year");
	Console.WriteLine("2. Return a list of members who has birth year is the provided year");
	Console.WriteLine("3. Return a list of members who has birth year less the provided year");
	Console.WriteLine("0. Exit");

	BreakLine();
	int option;
	if (!int.TryParse(Console.ReadLine(), out option))
	{
		Console.WriteLine("Invalid input");
		PrintMenu();
		return;
	}

	switch (option)
	{
		case 0:
			break;
		case 1:
			Console.WriteLine("Please choose a year between 1 and 9999");
			if (!int.TryParse(Console.ReadLine(), out year) || year < 1 || year > 9999)
			{
				Console.WriteLine("Invalid input");
				break;
			}
			PrintList(main.ReturnMembersAboveDobYear(year));
			break;
		case 2:
			Console.WriteLine("Please choose a year between 1 and 9999");
			if (!int.TryParse(Console.ReadLine(), out year) || year < 1 || year > 9999)
			{
				Console.WriteLine("Invalid input");
				break;
			}
			PrintList(main.ReturnMembersIsDobYear(year));
			break;
		case 3:
			Console.WriteLine("Please choose a year between 1 and 9999");
			if (!int.TryParse(Console.ReadLine(), out year) || year < 1 || year > 9999)
			{
				Console.WriteLine("Invalid input");
				break;
			}
			PrintList(main.ReturnMembersLessDobYear(year));
			break;
		default:
			Console.WriteLine($"No option is signed to {option}");
			break;
	}
	PrintMenu();
}

void BreakLine()
{
	Console.WriteLine("====================================================");
	Console.WriteLine("Please choose an option");
}

void PrintList(List<Member> members)
{
	if (members.Count == 0) Console.WriteLine("List is empty");
	foreach (Member member in members)
	{
		PrintOne(member);
	}
}

void PrintOne(Member member)
{
	Console.WriteLine(member.ToString());
}

