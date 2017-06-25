using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HunterTank
{
	public interface ISpawnItemSource<T> where T:Component
	{
		T GetItem();
	}
}
