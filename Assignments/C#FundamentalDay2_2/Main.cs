using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace C_FundamentalDay2_2
{
	internal class Main
	{
		public async Task<List<int>> ListAllPrimeAsync(int start, int end)
		{
			List<int> primes = new List<int>();
			//This is not at all optimized. All tasks are running on a single thread under
			//Concurrency methods. No tasks are truly running in parallel.
			List<Task> tasks = new List<Task>();
			object lockObj = new object();
			for (int i = start;  i <= end; i++)
			{
				tasks.Add(IsPrimeAsync(i, primes, lockObj));
			}

			await Task.WhenAll(tasks);

			return primes;
		}
		private static async Task IsPrimeAsync(int x, List<int> primes, object lockObj)
		{
			if (x <= 1) return;

			for (int i = 2; i <= Math.Sqrt(x); i++)
			{
				if (x % i == 0) return;
			}
			//Simulating long task
			await Task.Delay(1000);
			lock(lockObj) {
				primes.Add(x);
			}
		}
	}
}
