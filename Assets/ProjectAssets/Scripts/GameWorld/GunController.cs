using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HunterTank
{
	public class GunController : MonoBehaviour
	{
		[SerializeField]
		private List<Gun> _guns;

		private int _currentGunIndex;

		public void NextGun()
		{
			_currentGunIndex++;
		}

		public void PreviousGun()
		{
			_currentGunIndex--;
		}

		public void FireGun()
		{
			var projectile = Instantiate<Projectile>(CurrentGun.ProjectilePrefab);
			projectile.transform.position = CurrentGun.FireTransform.position;
			projectile.transform.rotation = CurrentGun.FireTransform.rotation;
		}

		private void ShowGunAt(int index)
		{
			index = (int)(Mathf.Repeat(index, _guns.Count));

			foreach (var item in _guns)
			{
				item.GunGo.SetActive(_guns[index] == item);
			}
		}

		private Gun CurrentGun { get { return _guns[_currentGunIndex]; } }

		[Serializable]
		class Gun
		{
			[SerializeField]
			private GameObject _gunGo;

			[SerializeField]
			private Projectile _projectilePrefab;

			[SerializeField]
			private Transform _fireTransform;

			public GameObject GunGo { get { return _gunGo; } }

			public Projectile ProjectilePrefab { get { return _projectilePrefab; } }

			public Transform FireTransform { get { return _fireTransform; } }
		}
	}
}