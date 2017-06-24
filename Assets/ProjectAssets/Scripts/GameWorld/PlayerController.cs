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
		[SerializeField]
		private GunController _gunController;

		private float _oldSwitchGunAxis;
		private float _oldFireGunAxis;

		private void Start()
		{

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

	}
}