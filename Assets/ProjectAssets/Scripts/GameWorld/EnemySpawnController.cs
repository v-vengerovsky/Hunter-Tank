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

		public EnemySpawnController(ISpawnItemSource<EnemyController> spawnItemSource,IPosNotifier posNotifier) : base(spawnItemSource)
		{
			_posNotifier = posNotifier;
		}

		protected override void ProcessSpawnedItem(EnemyController itemToProcess)
		{
			base.ProcessSpawnedItem(itemToProcess);

			_posNotifier.OnPositionChange += itemToProcess.SetPlayerPosition;
			itemToProcess.OnDestroyed += OnDestroyed;
		}

		protected override void OnDestroyed(EnemyController destroyedItem)
		{
			base.OnDestroyed(destroyedItem);

			_posNotifier.OnPositionChange -= destroyedItem.SetPlayerPosition;
			destroyedItem.OnDestroyed -= OnDestroyed;
		}
	}
}
