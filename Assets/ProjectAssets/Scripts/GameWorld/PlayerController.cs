using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HunterTank
{
	[RequireComponent(typeof(VehicleController))]
	public class PlayerController : MonoBehaviour
	{
		[SerializeField]
		private VehicleController _vehicleController;

		void Start()
		{

		}

		void Update()
		{
			Vector3 direction = Vector3.zero;
			direction.x = Input.GetAxisRaw(Constants.MoveXAxisName);
			direction.z = Input.GetAxisRaw(Constants.MoveYAxisName);
			_vehicleController.TargetDirection = direction;
		}
	}
}