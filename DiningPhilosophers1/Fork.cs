using System;

namespace DiningPhilosophers1
{
	public class Fork
	{
		public Fork(int name)
		{
			Name = name;
			IsInUse = false;
		}

		public int Name { get; }

		public bool IsInUse;
	}
}