using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HunterTank
{
	public class EnemySpawnController : SpawnController<EnemyController>
	{
		private IPosNotifier _posNotifier;
		private Action<IPosNotifier,Vector3> _notify;
		private Action<IPosNotifier> _onSpawn;
		private Action<IPosNotifier> _onDestroy;

		public EnemySpawnController(ISpawnItemSource<EnemyController> spawnItemSource,IPosNotifier posNotifier, Action<IPosNotifier,Vector3> notify, Action<IPosNotifier> onSpawn, Action<IPosNotifier> onDestroy) : base(spawnItemSource)
		{
			_posNotifier = posNotifier;
			_notify = notify;
			_onSpawn = onSpawn;
			_onDestroy = onDestroy;
		}

		protected override void ProcessSpawnedItem(EnemyController itemToProcess)
		{
			base.ProcessSpawnedItem(itemToProcess);

			_posNotifier.OnPositionChange += itemToProcess.SetPlayerPosition;
			itemToProcess.OnPositionChange += _notify;
			itemToProcess.OnDestroyed += OnDestroyed;

			if (_onSpawn != null)
			{
				_onSpawn.Invoke(itemToProcess);
			}
		}

		protected override void OnDestroyed(EnemyController destroyedItem)
		{
			base.OnDestroyed(destroyedItem);

			_posNotifier.OnPositionChange -= destroyedItem.SetPlayerPosition;
			destroyedItem.OnPositionChange -= _notify;
			destroyedItem.OnDestroyed -= OnDestroyed;

			if (_onDestroy != null)
			{
				_onDestroy.Invoke(destroyedItem);
			}
		}

		public override void Dispose()
		{
			base.Dispose();
			_notify = null;
		}
	}
}
