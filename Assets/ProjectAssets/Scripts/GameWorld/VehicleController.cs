using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HunterTank
{
	[RequireComponent(typeof(VehicleAnimator))]
	public class VehicleController : MonoBehaviour
	{
		[SerializeField]
		private float _maxSpeed = 30f;
		[SerializeField]
		private float _speedChangeSpeed = 10f;
		[SerializeField]
		private float _turningSpeed = 1f;
		[SerializeField]
		private Rigidbody _rigidBody;

		private Vector3 _targetDirection;

		public Vector3 TargetDirection
		{
			get { return _targetDirection; }
			set
			{
				if (!value.Equals(Vector3.zero))
				{
					_targetDirection = value.normalized;
				}
				else
				{
					_targetDirection = value;
				}
			}
		}

		private Vector3 CurrentDirection
		{
			get { return transform.forward; }
			set
			{
				if (Vector3.Angle(value, transform.forward) < Constants.CutoffValue)
				{
					return;
				}

				value.y = 0;
				transform.forward = value.normalized;
			}
		}

		private Vector3 CurrentVelocity
		{
			get { return _rigidBody.velocity; }
			set
			{
				//value.y = _rigidBody.velocity.y;
				_rigidBody.velocity = value;
			}
		}

		public bool Accelerate
		{
			get { return !_targetDirection.Equals(Vector3.zero); }
		}

		private void Start()
		{

		}

		private void Update()
		{
			UpdateDirection();
			UpdateSpeed();
		}

		private void UpdateDirection()
		{
			float angleBetweenDirections = Vector3.Angle(CurrentDirection, _targetDirection);

			if (Vector3.Angle(CurrentDirection, _targetDirection) > Constants.MinAngleBetweenDirections)
			{
				float turningSpeed = _turningSpeed * (1 - angleBetweenDirections / 180f);
				CurrentDirection = Vector3.Slerp(CurrentDirection, _targetDirection, turningSpeed);
			}
		}

		private void UpdateSpeed()
		{
			float targetSpeed = 0;

			if (Accelerate)
			{
				targetSpeed = _maxSpeed;
			}

			Vector3 temp = CurrentVelocity;
			temp.y = 0;
			float currentSpeed = temp.magnitude;
			float lerpSpeed = _speedChangeSpeed * Mathf.Abs((targetSpeed - currentSpeed) / _maxSpeed);
			float newSpeed = Mathf.Lerp(currentSpeed, targetSpeed, lerpSpeed);

			if (newSpeed > Constants.CutoffValue)
			{
				CurrentVelocity = CurrentDirection * newSpeed;
			}
			else
			{
				CurrentVelocity = Vector3.zero;
			}
		}
	}
}