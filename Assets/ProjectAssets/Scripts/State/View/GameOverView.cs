using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HunterTank
{
	public class GameOverView : ViewBase
	{
		[SerializeField]
		private Button _restartBtn;

		private event Action _onRestartPressed;

		public event Action OnRestartPressed
		{
			add { _onRestartPressed += value; }
			remove { _onRestartPressed -= value; }
		}

		private void Awake()
		{
			_restartBtn.onClick.AddListener(RestartPressed);
		}

		private void RestartPressed()
		{
			if (_onRestartPressed != null)
			{
				_onRestartPressed.Invoke();
			}
		}
	}
}