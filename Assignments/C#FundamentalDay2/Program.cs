using C_FundamentalDay2;
using System.ComponentModel;

printMenu();
int option;
int year;
bool shouldExit = false;
MainLINQ main = new MainLINQ();
do
{
	breakLine();
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
			printList(main.ReturnMaleMembers());
			break;
		case 2:
			printOne(main.ReturnOldestMember());
			break;
		case 3:
			foreach (string s in main.ReturnFullNameList())
				Console.WriteLine($"{s}");
			break;
		case 4:
			return3List();
			break;
		case 5:
			Member? member = main.ReturnFirstMemberInHanoi();
			if (member != null)
			{
				printOne(member);
				break;
			}
			Console.WriteLine("No member is from Ha Noi");
			break;
		default:
			Console.WriteLine($"No option is signed to {option}");
			break;
	}
} while (shouldExit == false);

void printMenu()
{
	Console.WriteLine("Welcome to my program");
	Console.WriteLine("1. Return a list of members who is Male");
	Console.WriteLine("2. Return the oldest member");
	Console.WriteLine("3. Return a new list that contains Full Name only");
	Console.WriteLine("4. Return 3 lists");
	Console.WriteLine("5. Return the first member who was born in Ha Noi.");
	Console.WriteLine("0. Exit");
}

void return3List()
{
	Console.WriteLine("1. Return a list of members who has birth year above the provided year");
	Console.WriteLine("2. Return a list of members who has birth year is the provided year");
	Console.WriteLine("3. Return a list of members who has birth year less the provided year");
	Console.WriteLine("0. Exit");

	breakLine();
	int option;
	if (!int.TryParse(Console.ReadLine(), out option))
	{
		Console.WriteLine("Invalid input");
		printMenu();
		return;
	}

	switch (option)
	{
		case 0:
			break;
		case 1:
			Console.WriteLine("Please choose a year");
			if (!int.TryParse(Console.ReadLine(), out year))
			{
				Console.WriteLine("Invalid input");
				break;
			}
			printList(main.ReturnMembersAboveDobYear(year));
			break;
		case 2:
			Console.WriteLine("Please choose a year");
			if (!int.TryParse(Console.ReadLine(), out year))
			{
				Console.WriteLine("Invalid input");
				break;
			}
			printList(main.ReturnMembersAboveDobYear(year));
			break;
		case 3:
			Console.WriteLine("Please choose a year");
			if (!int.TryParse(Console.ReadLine(), out year))
			{
				Console.WriteLine("Invalid input");
				break;
			}
			printList(main.ReturnMembersAboveDobYear(year));
			break;
		default:
			Console.WriteLine($"No option is signed to {option}");
			break;
	}
	printMenu();
}

void breakLine()
{
	Console.WriteLine("====================================================");
	Console.WriteLine("Please choose an option");
}

void printList(List<Member> members)
{
	if (members.Count == 0) Console.WriteLine("List is empty");
	foreach (Member member in members)
	{
		printOne(member);
	}
}

void printOne(Member member)
{
	Console.WriteLine(member.ToString());
}

