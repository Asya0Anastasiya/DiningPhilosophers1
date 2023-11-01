using System.Collections.Generic;
using System.Linq;

namespace DiningPhilosophers1
{
	public class Philosophers : List<Philosopher>
	{
		//private readonly int _philosopherCount = ConfigValue.Inst.PhilosopherCount;
		//private readonly int _forkCount = ConfigValue.Inst.ForkCount;
		private readonly int _philosopherCount = 5;
		private readonly int _forkCount = 5;

		public Philosophers InitializePhilosophers()
		{
			// Initialize the Forks
			// We need the forks because each philosopher needs to 
			// acquire both right and left forks in order to eat.
			//
			var forks = new List<Fork>();
			Enumerable.Range(0, _forkCount).ToList().ForEach(name => forks.Add(new Fork(name)));

			// Initialize the philosophers
			// Philosopher[i] needs 
			//		Fork[(i - 1) % 5] as her/his left fork
			//		Fork[i] as her/his right fork
			//
			int LeftForkName(int phName) => (_forkCount + phName - 1) % _forkCount;
			int RightForkName(int phName) => phName;
			Enumerable.Range(0, _philosopherCount).ToList().ForEach(name =>
				Add(new Philosopher(name, forks[LeftForkName(name)], forks[RightForkName(name)], this)));

			return this;
		}
	}
}
