using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IPlayerNotifierEvent = HunterTank.INotifierEvent<HunterTank.IPlayerNotifier>;

namespace HunterTank
{
	[RequireComponent(typeof(VehicleController))]
	public class PlayerController : MonoBehaviour, IPlayerNotifier, IPlayerNotifierEvent, ICollidable
	{
		[SerializeField]
		private VehicleController _vehicleController;
		//[SerializeField]
		//private health
		[SerializeField]
		private GunController _gunController;
		[SerializeField]
		private float _health;
		[SerializeField]
		[Range(0f, 1f)]
		private float _armor;

		private float _currentHealth;

		private float _oldSwitchGunAxis;
		private float _oldFireGunAxis;

		public int Id { get { return gameObject.GetInstanceID(); } }

		public Vector3 Position { get { return _vehicleController.Position; } }

		public RangeFloat RangeFloat { get { return new RangeFloat(_currentHealth, _health); } }

		private event Action<IPlayerNotifier> _onNotify;

		public event Action<IPlayerNotifier> OnNotify
		{
			add { _onNotify += value; }
			remove { _onNotify -= value; }
		}

		private event Action<PlayerController, ICollidable> _onDestroyed;

		public event Action<PlayerController, ICollidable> OnDestroyed
		{
			add { _onDestroyed += value; }
			remove { _onDestroyed -= value; }
		}

		public float Damage { get { return float.MaxValue; } }

		public void Collide(ICollidable other)
		{
			_currentHealth -= other.Damage * (1 - _armor);

			if (_currentHealth <= 0)
			{
				if (_onDestroyed != null)
				{
					_onDestroyed.Invoke(this, other);
				}

				Destroy(gameObject);
			}
		}

		private void Notify()
		{
			if (_onNotify != null)
			{
				_onNotify.Invoke(this);
			}
		}

		private void Awake()
		{
			_currentHealth = _health;
			_vehicleController.OnNotify += Notify;
		}

		private void Update()
		{
			Vector3 direction = Vector3.zero;
			direction.x = Input.GetAxis(Constants.MoveXAxisName);
			direction.z = Input.GetAxis(Constants.MoveYAxisName);
			_vehicleController.TargetDirection = direction;

			if (Input.GetAxis(Constants.SwitchGunAxisName) > 0 && _oldSwitchGunAxis <= 0)
			{
				_gunController.NextGun();
			}
			else if (Input.GetAxis(Constants.SwitchGunAxisName) < 0 && _oldSwitchGunAxis >= 0)
			{
				_gunController.PreviousGun();
			}

			_oldSwitchGunAxis = Input.GetAxis(Constants.SwitchGunAxisName);

			if (Input.GetAxis(Constants.FireAxisName) > 0)
			{
				_gunController.TryFireGun();
			}

			_oldFireGunAxis = Input.GetAxis(Constants.FireAxisName);
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