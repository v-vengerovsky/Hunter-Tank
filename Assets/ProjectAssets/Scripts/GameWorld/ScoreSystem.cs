using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HunterTank
{
	public class ScoreSystem
	{
		private int _score = 0;

		private event Action<int> _onScore;
		private event Action _onLoose;

		public event Action<int> OnScore
		{
			add { _onScore += value; }
			remove { _onScore -= value; }
		}

		public event Action OnLoose
		{
			add { _onLoose += value; }
			remove { _onLoose -= value; }
		}

		public int Score { get { return _score; } }

		public ScoreSystem()
		{

		}

		public void Update()
		{

		}
	}
}
