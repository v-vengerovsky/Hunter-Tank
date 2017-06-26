using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HunterTank
{
	public class EnemyController : MonoBehaviour, IPosNotifier, ICollidable
	{
		[SerializeField]
		private VehicleController _vehicleController;
		[SerializeField]
		private float _damage;
		[SerializeField]
		private float _health;

		private float _currentHealth;

		private event Action<EnemyController> _onDestroyed;

		public event Action<EnemyController> OnDestroyed
		{
			add { _onDestroyed += value; }
			remove { _onDestroyed -= value; }
		}

		public int Id { get { return gameObject.GetInstanceID(); } }

		public event Action<IPosNotifier,Vector3> OnPositionChange
		{
			add { _vehicleController.OnPositionChange += value; }
			remove { _vehicleController.OnPositionChange -= value; }
		}

		public void SetPlayerPosition(IPosNotifier notifier, Vector3 playerPos)
		{
			Vector3 targetDir = playerPos - transform.position;
			targetDir.Normalize();
			_vehicleController.TargetDirection = targetDir;
		}

		public float Damage { get { return _damage; } }

		public void Collide(ICollidable other)
		{
			_currentHealth -= other.Damage;

			if (_currentHealth <= 0)
			{
				if (_onDestroyed != null)
				{
					_onDestroyed.Invoke(this);
				}

				Destroy(gameObject);
			}
		}

		private void Awake()
		{
			_currentHealth = _health;
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