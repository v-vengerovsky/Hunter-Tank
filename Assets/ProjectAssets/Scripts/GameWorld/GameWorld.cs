using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HunterTank
{
	public class GameWorld
	{
		private GameData _gameData;
		private ScoreSystem _scoreSysytem;

		public event Action<int> OnScore
		{
			add { _scoreSysytem.OnScore += value; }
			remove { _scoreSysytem.OnScore -= value; }
		}

		public GameWorld(GameData gameData)
		{
			_gameData = gameData;
			_scoreSysytem = new ScoreSystem();
			_scoreSysytem.OnLoose += Lost;
		}

		public void Update()
		{
			_scoreSysytem.Update();
		}

		public void Dispose()
		{
			
		}

		private void Lost()
		{
			Approot.Instance.SetState(new GameOverState(_scoreSysytem.Score));
		}
	}
}