using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HunterTank
{
	public partial class Approot : MonoBehaviour
	{
		private static Approot _instance;

		public static Approot Instance
		{
			get { return _instance; }
		}

		private void Awake()
		{
			_instance = this;
		}

		private void Start()
		{
			SetState(new MenuState());
		}

		private void Update()
		{
			if (_currentState.Activated)
			{
				_currentState.Update();
			}
		}
	}
}