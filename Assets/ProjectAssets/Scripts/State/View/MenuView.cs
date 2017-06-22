using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HunterTank
{
	public class MenuView : ViewBase
	{
		[SerializeField]
		private Button _startBtn;

		private event Action _onStartPressed;

		public event Action OnStartPressed
		{
			add { _onStartPressed += value; }
			remove { _onStartPressed -= value; }
		}

		private void Awake()
		{
			_startBtn.onClick.AddListener(StartPressed);
		}

		private void StartPressed()
		{
			if (_onStartPressed != null)
			{
				_onStartPressed.Invoke();
			}
		}
	}
}