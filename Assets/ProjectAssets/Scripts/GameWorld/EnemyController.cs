using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IPlayerNotifierEvent = HunterTank.INotifierEvent<HunterTank.IPlayerNotifier>;

namespace HunterTank
{
	public class EnemyController : MonoBehaviour, IPlayerNotifier, IPlayerNotifierEvent, ICollidable
	{
		[SerializeField]
		private VehicleController _vehicleController;
		[SerializeField]
		private float _damage;
		[SerializeField]
		private float _health;
		[SerializeField]
		[Range(0f,1f)]
		private float _armor;

		private float _currentHealth;

		private event Action<EnemyController> _onDestroyed;

		public event Action<EnemyController> OnDestroyed
		{
			add { _onDestroyed += value; }
			remove { _onDestroyed -= value; }
		}

		public int Id { get { return gameObject.GetInstanceID(); } }

		public Vector3 Position { get { return _vehicleController.Position; } }

		public RangeFloat RangeFloat { get { return new RangeFloat(_currentHealth,_health); } }

		private event Action<IPlayerNotifier> _onNotify;

		public event Action<IPlayerNotifier> OnNotify
		{
			add { _onNotify += value; }
			remove { _onNotify -= value; }
		}

		public void SetPlayerPosition(IPlayerNotifier notifier)
		{
			Vector3 targetDir = notifier.Position - transform.position;
			targetDir.Normalize();
			_vehicleController.TargetDirection = targetDir;
		}

		public float Damage { get { return _damage; } }

		public void Collide(ICollidable other)
		{
			_currentHealth -= other.Damage * (1 - _armor);

			if (_currentHealth <= 0)
			{
				if (_onDestroyed != null)
				{
					_onDestroyed.Invoke(this);
				}

				Destroy(gameObject);
			}
		}

		private void Notify()
		{
			if (_onNotify != null)
			{
				_onNotify(this);
			}
		}

		private void Awake()
		{
			_currentHealth = _health;
			_vehicleController.OnNotify += Notify;
		}

		private void Reset()
		{
			_vehicleController = GetComponent<VehicleController>();
		}

		private void OnCollisionEnter(Collision collision)
		{
			ICollidable other = collision.collider.GetComponent<ICollidable>();

			if (other != null)
			{
				other.Collide(this);
			}
		}
	}
}