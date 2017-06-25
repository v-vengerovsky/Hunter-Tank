using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HunterTank
{
	public class EnemyController : MonoBehaviour
	{
		[SerializeField]
		private VehicleController _vehicleController;

		public void SetPlayerPosition(Vector3 playerPos)
		{
			Vector3 targetDir = playerPos - transform.position;
			targetDir.Normalize();
			_vehicleController.TargetDirection = targetDir;
		}

		private void Reset()
		{
			_vehicleController = GetComponent<VehicleController>();
		}
	}
}