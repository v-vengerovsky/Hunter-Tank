using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using IEnemyPosNotifiable = HunterTank.IEnemyNotifiable<HunterTank.IPlayerNotifier>;

namespace HunterTank
{
	public class ScoreSystem:IEnemyPosNotifiable
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

		public void OnSpawn(IPlayerNotifier enemy)
		{
			
		}

		public void OnDestroy(IPlayerNotifier enemy, ICollidable other)
		{
			if (other is Projectile)
			{
				_score++;

				if (_onScore != null)
				{
					_onScore(Score);
				}
			}
		}

		public void Notify(IPlayerNotifier enemy)
		{
			
		}
	}
}
