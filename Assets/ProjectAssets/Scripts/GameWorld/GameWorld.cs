﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using IEnemyPosNotifiable = HunterTank.IEnemyNotifiable<HunterTank.IPlayerNotifier>;

namespace HunterTank
{
	public class GameWorld
	{
		private GameData _gameData;
		private ScoreSystem _scoreSysytem;
		private PlayerController _playerController;
		private EnemySpawnController _enemySpawController;
		private EnemyMarkerController _enemyMarkerController;
		private HealthController _healthController;

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
			_playerController.OnNotify += _gameData.SetPosition;
			_enemyMarkerController = new EnemyMarkerController(_gameData.EnemyMarkerView, _gameData.Camera);
			_playerController.OnNotify += _enemyMarkerController.NotifyPlayer;
			_healthController = new HealthController(_gameData.HealthView,_gameData.Camera);
			_healthController.OnSpawn(_playerController);
			_playerController.OnNotify += _healthController.Notify;
			_enemySpawController = new EnemySpawnController(gameData, _playerController, new IEnemyPosNotifiable[]{ _enemyMarkerController, _healthController , _scoreSysytem });
			_enemySpawController.OnSpawn += _gameData.Spawn;
			_enemySpawController.SpawnCondition += NeedToSpawn;
			_playerController.OnDestroyed += PlayerDestroyed;
		}

		public void Update()
		{
			_scoreSysytem.Update();
			_enemySpawController.Update();
		}

		public void Dispose()
		{
			_playerController.OnNotify -= _gameData.SetPosition;
			_scoreSysytem.OnLoose -= Lost;
			_enemySpawController.OnSpawn -= _gameData.Spawn;
			_enemySpawController.SpawnCondition -= NeedToSpawn;
			_enemySpawController.Dispose();
		}

		private void PlayerDestroyed(PlayerController playerController, ICollidable other)
		{
			if (playerController != _playerController)
			{
				return;
			}

			Approot.Instance.SetState(new GameOverState(_scoreSysytem.Score));
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