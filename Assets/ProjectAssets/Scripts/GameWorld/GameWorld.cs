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
		private PlayerController _playerController;
		private EnemySpawnController _enemySpawController;

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
			_playerController = _gameData.GetPlayerController();
			_playerController.OnPositionChange += _gameData.SetPosition;
			_enemySpawController = new EnemySpawnController(gameData,_playerController);
			_enemySpawController.OnSpawn += _gameData.Spawn;
			_enemySpawController.SpawnCondition += NeedToSpawn;

			//_playerController.OnPositionChange += _gameData.SetPosition;
		}

		public void Update()
		{
			_scoreSysytem.Update();
			_enemySpawController.Update();
		}

		public void Dispose()
		{
			_playerController.OnPositionChange -= _gameData.SetPosition;
			_scoreSysytem.OnLoose -= Lost;
			_enemySpawController.OnSpawn -= _gameData.Spawn;
			_enemySpawController.SpawnCondition -= NeedToSpawn;
		}

		private void Lost()
		{
			Approot.Instance.SetState(new GameOverState(_scoreSysytem.Score));
		}

		private bool NeedToSpawn(SpawnController<EnemyController> enemySpawController)
		{
			return enemySpawController.ItemsCount < Constants.MaxEnemies;
		}
	}
}