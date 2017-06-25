using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HunterTank
{
	public interface IObserver
	{
		void Notify();
		void Notify(object obj);
	}
}
