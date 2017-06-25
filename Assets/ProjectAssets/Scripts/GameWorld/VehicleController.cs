using System;
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
		private Vector3 _velocity;
		private event Action<Vector3> _onPositionChange;

		public event Action<Vector3> OnPositionChange
		{
			add { _onPositionChange += value; }
			remove { _onPositionChange -= value; }
		}

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
			get { return _velocity; }
			set
			{
				_velocity = value;
			}
		}

		private Vector3 Position
		{
			get { return transform.position; }
			set { transform.position = value; }
		}

		public bool Accelerate
		{
			get { return !_targetDirection.Equals(Vector3.zero); }
		}

		private void Start()
		{

		}

		private void FixedUpdate()
		{
			UpdateDirection();
			UpdateSpeed();
			UpdatePosition();
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
			float direction =  targetSpeed - currentSpeed;
			float newSpeed = currentSpeed + _speedChangeSpeed * Mathf.Sign(direction) * Time.fixedDeltaTime;

			if (newSpeed > Constants.CutoffValue)
			{
				CurrentVelocity = CurrentDirection * newSpeed;
			}
			else
			{
				CurrentVelocity = Vector3.zero;
			}
			//Debug.LogWarningFormat("name:{0} currentSpeed:{1}, targetSpeed:{2}, direction:{3}, newSpeed:{4},CurrentVelocity: {5}", name, currentSpeed, targetSpeed, direction, newSpeed, CurrentVelocity);
		}

		private void UpdatePosition()
		{
			Position = Position + CurrentVelocity * Time.fixedDeltaTime;

			if (_onPositionChange != null)
			{
				_onPositionChange(Position);
			}
		}

		private void Reset()
		{
			_rigidBody = GetComponent<Rigidbody>();
		}
	}
}