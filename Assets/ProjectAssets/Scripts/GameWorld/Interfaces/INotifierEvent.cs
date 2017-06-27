using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HunterTank
{
	public interface INotifierEvent<T> where T : INotifier
	{
		event Action<T> OnNotify;
	}
}
