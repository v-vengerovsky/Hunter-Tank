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

		private void FixedUpdate()
		{
			if (_timeToReload > 0)
			{
				_timeToReload = Math.Max(0f, _timeToReload - Time.fixedDeltaTime);
			}

			TryToAim();
		}

		private void Awake()
		{
			foreach (var item in _guns)
			{
				item.Init();
			}
		}

		private void TryToAim()
		{
			Ray ray = new Ray(CurrentGun.RaycastTransform.position, CurrentGun.RaycastTransform.forward);
			//Debug.DrawRay(CurrentGun.RaycastTransform.position, CurrentGun.RaycastTransform.forward * Constants.RayLengthForAiming,Color.red,5f);
			RaycastHit[] hits = Physics.RaycastAll(ray, Constants.RayLengthForAiming);
			EnemyController enemy;

			for (int i = 0; i < hits.Length; i++)
			{
				enemy = hits[i].collider.GetComponent<EnemyController>();

				if (enemy != null)
				{
					CurrentGun.RotationTransform.forward = hits[i].point - CurrentGun.RotationTransform.position;
					return;
				}
			}

			//CurrentGun.RotationTransform.forward = CurrentGun.RotationTransform.parent.TransformDirection(CurrentGun.DefaultGunForward);
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

			[SerializeField]
			private Transform _raycastTransform;

			[SerializeField]
			private Transform _rotationTransform;

			private int _currentFireTransformIndex;
			private Vector3 _defaultGunForward;

			public GameObject GunGo { get { return _gunGo; } }

			public Projectile ProjectilePrefab { get { return _projectilePrefab; } }

			public float ReloadTime { get { return _reloadTime; } }

			public Transform RaycastTransform { get { return _raycastTransform; } }

			public Transform RotationTransform { get { return _rotationTransform; } }

			public Vector3 DefaultGunForward { get { return _defaultGunForward; } }

			public Transform FireTransform
			{
				get
				{
					_currentFireTransformIndex++;
					_currentFireTransformIndex = (int)(Mathf.Repeat(_currentFireTransformIndex, _fireTransforms.Count));
					return _fireTransforms[_currentFireTransformIndex];
				}
			}

			public void Init()
			{
				_defaultGunForward = RotationTransform.parent.InverseTransformDirection(RotationTransform.forward);
			}
		}
	}
}