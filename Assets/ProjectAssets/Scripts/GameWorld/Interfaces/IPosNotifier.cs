using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HunterTank
{
	public interface IPosNotifier
	{
		int Id { get; }
		event Action<IPosNotifier, Vector3> OnPositionChange;
	}
}
