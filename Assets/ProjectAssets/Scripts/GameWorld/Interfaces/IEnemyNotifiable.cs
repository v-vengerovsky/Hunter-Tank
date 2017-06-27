using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//using IPosNotifier = HunterTank.INotifier<UnityEngine.Vector3>;
//using IFloatNotifier = HunterTank.INotifier<float>;

namespace HunterTank
{
	public interface IEnemyNotifiable<T> where T: INotifier
	{
		void OnSpawn(T enemy);
		void OnDestroy(T enemy);
		void Notify(T enemy);
	}
}
