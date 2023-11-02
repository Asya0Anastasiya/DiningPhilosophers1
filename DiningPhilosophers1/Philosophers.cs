using System.Collections.Generic;
using System.Linq;

namespace DiningPhilosophers1
{
	public class Philosophers : List<Philosopher>
	{
		private readonly int _philosopherCount = 5;
		private readonly int _forkCount = 5;

		public Philosophers InitializePhilosophers()
		{
			var forks = new List<Fork>();
			Enumerable.Range(0, _forkCount).ToList().ForEach(name => forks.Add(new Fork(name)));

			int LeftForkName(int phName) => (_forkCount + phName - 1) % _forkCount;
			int RightForkName(int phName) => phName;
			Enumerable.Range(0, _philosopherCount).ToList().ForEach(name =>
				Add(new Philosopher(name, forks[LeftForkName(name)], forks[RightForkName(name)], this)));

			return this;
		}
	}
}
