using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HunterTank
{
	public class SpawnController<T> where T:Component
	{
		public delegate T SpawnAction(T item);

		private List<T> _items = new List<T>();
		private float _timeToSpawn;
		ISpawnItemSource<T> _spawnItemSource;

		private event SpawnAction _onSpawn;
		private event Predicate<SpawnController<T>> _spawnCondition;

		public event SpawnAction OnSpawn
		{
			add { _onSpawn += value; }
			remove { _onSpawn -= value; }
		}

		public event Predicate<SpawnController<T>> SpawnCondition
		{
			add { _spawnCondition += value; }
			remove { _spawnCondition -= value; }
		}

		public int ItemsCount { get { return _items.Count; } }

		public SpawnController(ISpawnItemSource<T> spawnItemSource)
		{
			_spawnItemSource = spawnItemSource;
		}

		public void Update()
		{
			if (_onSpawn == null || _spawnCondition == null)
			{
				return;
			}

			if (_timeToSpawn > 0)
			{
				_timeToSpawn = Math.Max(0f, _timeToSpawn - Time.deltaTime);
			}
			else if(_spawnCondition(this))
			{
				Spawn(_spawnItemSource.GetItem());
			}
		}

		public virtual void Dispose()
		{

		}

		private void Spawn(T itemToSpawnn)
		{
			if (_onSpawn != null)
			{
				_items.Add(_onSpawn.Invoke(itemToSpawnn));
				ProcessSpawnedItem(_items[_items.Count - 1]);
			}

			_timeToSpawn = Constants.MinSpawnInterval;
		}

		protected virtual void ProcessSpawnedItem(T itemToProcess)
		{

		}

		protected virtual void OnDestroyed(T destroyedItem)
		{
			_items.Remove(destroyedItem);
		}
	}
}
