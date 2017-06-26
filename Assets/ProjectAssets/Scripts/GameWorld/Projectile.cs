using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HunterTank
{
	public class Projectile : MonoBehaviour,ICollidable
	{
		[SerializeField]
		private float _damage;
		[SerializeField]
		private float _startSpeed;
		[SerializeField]
		private float _acceleration;
		[SerializeField]
		private float _maxSpeed;
		[SerializeField]
		private Rigidbody _rigidbody;
		[SerializeField]
		private Collider _collider;

		private float _flightTime;

		public float Damage { get { return _damage; } }

		public void Collide(ICollidable other)
		{
			Destroy(gameObject);
		}

		private void Start()
		{
			_rigidbody.velocity = transform.forward * _startSpeed;
			_collider.isTrigger = false;
		}

		private void FixedUpdate()
		{
			float currentSpeed = Mathf.Clamp( _rigidbody.velocity.magnitude + Time.fixedDeltaTime * _acceleration, _startSpeed, _maxSpeed);
			_rigidbody.velocity = transform.forward * currentSpeed;

			_flightTime += Time.fixedDeltaTime;

			if (_flightTime > Constants.MaxProjectileFlightTime)
			{
				Destroy(gameObject);
			}
		}

		//private void OnTriggerExit(Collider other)
		//{
		//	var player = other.GetComponent<PlayerController>();

		//	if (player != null)
		//	{
		//		_collider.isTrigger = false;
		//	}
		//}

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