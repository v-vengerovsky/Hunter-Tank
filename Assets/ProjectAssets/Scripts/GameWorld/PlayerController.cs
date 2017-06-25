using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HunterTank
{
	[RequireComponent(typeof(VehicleController))]
	public class PlayerController : MonoBehaviour,IPosNotifier,ICollidable
	{
		[SerializeField]
		private VehicleController _vehicleController;
		[SerializeField]
		private GunController _gunController;
		[SerializeField]
		private float _health;

		private float _currentHealth;

		private float _oldSwitchGunAxis;
		private float _oldFireGunAxis;
		private event Action<PlayerController> _onDestroyed;

		public event Action<Vector3> OnPositionChange
		{
			add { _vehicleController.OnPositionChange += value; }
			remove { _vehicleController.OnPositionChange -= value; }
		}

		public event Action<PlayerController> OnDestroyed
		{
			add { _onDestroyed += value; }
			remove { _onDestroyed -= value; }
		}

		public float Damage { get { return float.MaxValue; } }

		public void Collide(ICollidable other)
		{
			_currentHealth -= other.Damage;

			if (_currentHealth <= 0)
			{
				Destroy(gameObject);
			}
		}

		private void Awake()
		{
			_currentHealth = _health;
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

		private void OnDestroy()
		{
			if (_onDestroyed != null)
			{
				_onDestroyed.Invoke(this);
			}
		}
	}
}