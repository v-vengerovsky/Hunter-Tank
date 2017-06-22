using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HunterTank
{
	public class UiRoot : MonoBehaviour
	{
		private static List<UiRoot> _instances = new List<UiRoot>();

		public static V GetView<V>() where V : ViewBase
		{
			V result;

			foreach (var item in _instances)
			{
				result = item.GetComponentInChildren<V>();

				if (result != null)
				{
					return result;
				}
			}

			return null;
		}

		private void Awake()
		{
			_instances.Add(this);
		}

		private void OnDestroy()
		{
			_instances.Remove(this);
		}
	}
}