using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiningPhilosophers1
{
	public class Philosopher
	{
		public Philosopher(int name, Fork leftFork, Fork rightFork, Philosophers allPhilosophers)
		{
			Name = name;
			LeftFork = leftFork;
			RightFork = rightFork;

			_rnd = new Random(Name);
		}

		static readonly SemaphoreSlim AquireEatPermissionSlip = new(2);

		private static readonly object Sync = new();

		public int Name { get; }

		private Fork LeftFork { get; }

		private Fork RightFork { get; }

		public int EatCount { get; private set; }

		private readonly Random _rnd;

		private readonly int _maxThinkDuration = 1000;
		private readonly int _minThinkDuration = 50;

		public void EatingHabit(CancellationToken stopDining)
		{
			var durationBeforeRequstEatPermission = 20;

			for (var i = 0;; ++i)
			{
				if (stopDining.IsCancellationRequested)
				{
					stopDining.ThrowIfCancellationRequested();
				}

				try
				{
					AquireEatPermissionSlip.WaitAsync().Wait();

					bool isOkToEat;
					lock (Sync)
					{
						isOkToEat = IsForksAvailable();
						if (isOkToEat)
							AquireForks();
					}

					if (isOkToEat)
					{
						PhilosopherEat();
						ReleaseForks(); 
					}
				}
				catch (Exception ex)
				{
					throw;
				}
				finally
				{
					AquireEatPermissionSlip.Release();
				}
				Task.Delay(durationBeforeRequstEatPermission).Wait();
			}
		}

		private bool IsForksAvailable()
		{
			lock (Sync)
			{
				if (LeftFork.IsInUse)
				{
					return false;
				}

				if (RightFork.IsInUse)
				{
					return false;
				}
			}

			return true;
		}

		private void PhilosopherEat()
		{
			var eatingDuration = _rnd.Next(_maxThinkDuration) + _minThinkDuration;

			Thread.Sleep(eatingDuration);

			++EatCount;
		}

		private void AquireForks()
		{
			lock (Sync)
			{
				LeftFork.IsInUse = true;
				RightFork.IsInUse = true;
			}
		}

		void ReleaseForks()
		{
			lock (Sync)
			{
				LeftFork.IsInUse = false;
				RightFork.IsInUse = false;
			}
		}
	}
}