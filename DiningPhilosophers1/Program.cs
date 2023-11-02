using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiningPhilosophers1
{
	class Program
	{
		static void Main()
		{
			var philosophers = new Philosophers().InitializePhilosophers();
			var phEatTasks = new List<Task>();

			using (var stopDiningTokenSource = new CancellationTokenSource())
			{
				var stopDiningToken = stopDiningTokenSource.Token;
				foreach (var ph in philosophers)
					phEatTasks.Add(
						Task.Factory.StartNew(() => ph.EatingHabit(stopDiningToken), stopDiningToken));

				Task.Delay(60000).Wait();
				try
				{
					stopDiningTokenSource.Cancel();
					Task.WaitAll(phEatTasks.ToArray());
				}
				catch (AggregateException ae)
				{
				
				}
			}

			Console.WriteLine("Done.");

			Console.WriteLine();
			var totalEatCount = philosophers.Sum(p => p.EatCount);
			foreach (var ph in philosophers)
				Console.WriteLine($"Philosopher Ph{ph.Name} ate {ph.EatCount,3} times.");

			Console.ReadKey();
		}
	}
}


