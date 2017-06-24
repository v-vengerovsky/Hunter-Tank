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
		private float _timeToReload;

		public void NextGun()
		{
			_currentGunIndex++;
			ShowGunAt(_currentGunIndex);
		}

		public void PreviousGun()
		{
			_currentGunIndex--;
			ShowGunAt(_currentGunIndex);
		}

		public void TryFireGun()
		{
			if (_timeToReload <= 0)
			{
				FireGun();
			}
		}

		private void FireGun()
		{
			var projectile = Instantiate<Projectile>(CurrentGun.ProjectilePrefab);
			projectile.transform.position = CurrentGun.FireTransform.position;
			projectile.transform.rotation = CurrentGun.FireTransform.rotation;
			//projectile.transform.localScale = Vector3.one;
			_timeToReload = CurrentGun.ReloadTime;
		}

		private void ShowGunAt(int index)
		{
			index = (int)(Mathf.Repeat(index, _guns.Count));

			foreach (var item in _guns)
			{
				item.GunGo.SetActive(_guns[index] == item);
			}
		}

		private void Update()
		{
			if (_timeToReload > 0)
			{
				_timeToReload = Math.Max(0f, _timeToReload - Time.deltaTime);
			}
		}

		private Gun CurrentGun
		{
			get
			{
				_currentGunIndex = (int)(Mathf.Repeat(_currentGunIndex, _guns.Count));
				return _guns[_currentGunIndex];
			}
		}

		[Serializable]
		class Gun
		{
			[SerializeField]
			private float _reloadTime;

			[SerializeField]
			private GameObject _gunGo;

			[SerializeField]
			private Projectile _projectilePrefab;

			[SerializeField]
			private List<Transform> _fireTransforms;

			private int _currentFireTransformIndex;

			public GameObject GunGo { get { return _gunGo; } }

			public Projectile ProjectilePrefab { get { return _projectilePrefab; } }

			public float ReloadTime { get { return _reloadTime; } }

			public Transform FireTransform
			{
				get
				{
					_currentFireTransformIndex++;
					_currentFireTransformIndex = (int)(Mathf.Repeat(_currentFireTransformIndex, _fireTransforms.Count));
					return _fireTransforms[_currentFireTransformIndex];
				}
			}
		}
	}
}