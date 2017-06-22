using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HunterTank
{
	public class GameData : MonoBehaviour
	{
		private static GameData _instance;

		public static GameData Instance
		{
			get { return _instance; }
		}



		private void Awake()
		{
			_instance = this;

		}
	}
}