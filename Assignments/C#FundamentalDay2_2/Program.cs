// See https://aka.ms/new-console-template for more information
using C_FundamentalDay2_2;
do
{
	Console.WriteLine("Enter start and end to find primes between them! (start < end)");
	Console.Write("start = ");
	int start;
	while (!int.TryParse(Console.ReadLine(), out start))
	{
		Console.WriteLine("Invalid input!");
		Console.Write("start = ");
	}

	Console.Write("end = ");
	int end;
	while (!int.TryParse(Console.ReadLine(), out end))
	{
		Console.WriteLine("Invalid input!");
		Console.Write("end = ");
	}

	Main main = new Main();
	List<int> primes = await main.ListAllPrimeAsync(start, end);
	Console.WriteLine(primes.Count);

	//foreach (int prime in primes)
	//{
	//	Console.Write($"{prime} ");
	//}
	Console.WriteLine();
} while (true);