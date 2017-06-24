using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HunterTank
{
	public class Projectile : MonoBehaviour
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

		private void Start()
		{
			_rigidbody.velocity = transform.forward * _startSpeed;
			_collider.isTrigger = true;
		}

		private void Update()
		{
			float currentSpeed = Mathf.Clamp( _rigidbody.velocity.magnitude + Time.deltaTime * _acceleration, _startSpeed, _maxSpeed);
			_rigidbody.velocity = transform.forward * currentSpeed;
		}

		private void OnTriggerExit(Collider other)
		{
			var player = other.GetComponent<PlayerController>();

			if (player != null)
			{
				_collider.isTrigger = false;
			}
		}		
	}
}