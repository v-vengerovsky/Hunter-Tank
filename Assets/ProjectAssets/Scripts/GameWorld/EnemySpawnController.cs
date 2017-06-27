using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using IPlayerNotifierEvent = HunterTank.INotifierEvent<HunterTank.IPlayerNotifier>;
using IEnemyPosNotifiable = HunterTank.IEnemyNotifiable<HunterTank.IPlayerNotifier>;

namespace HunterTank
{
	public class EnemySpawnController : SpawnController<EnemyController>
	{
		private IEnumerable<IEnemyPosNotifiable> _enemyNotifiables;
		private IPlayerNotifierEvent _playerNotifier;

		public EnemySpawnController(ISpawnItemSource<EnemyController> spawnItemSource, IPlayerNotifierEvent playerNotifier, IEnumerable<IEnemyPosNotifiable> enemyNotifiables) : base(spawnItemSource)
		{
			_enemyNotifiables = enemyNotifiables;
			_playerNotifier = playerNotifier;
		}

		protected override void ProcessSpawnedItem(EnemyController itemToProcess)
		{
			base.ProcessSpawnedItem(itemToProcess);

			_playerNotifier.OnNotify += itemToProcess.SetPlayerPosition;

			foreach (var item in _enemyNotifiables)
			{
				itemToProcess.OnNotify += item.Notify;
				item.OnSpawn(itemToProcess);
			}

			itemToProcess.OnDestroyed += OnDestroyed;
		}

		protected override void OnDestroyed(EnemyController destroyedItem, ICollidable other)
		{
			base.OnDestroyed(destroyedItem, other);

			_playerNotifier.OnNotify -= destroyedItem.SetPlayerPosition;
			foreach (var item in _enemyNotifiables)
			{
				destroyedItem.OnNotify += item.Notify;
				item.OnDestroy(destroyedItem, other);
			}

			destroyedItem.OnDestroyed -= OnDestroyed;
		}
	}
}
