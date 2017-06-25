using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HunterTank
{
	public interface IObserverable
	{
		void AddObserver(IObserver observer);
		void RemoveObserver(IObserver observer);
		void ClearObservers();
	}
}
