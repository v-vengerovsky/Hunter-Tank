using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HunterTank
{
	public class SplashDamageProjectile:Projectile
	{
		[SerializeField]
		private float _triggerRadius;
		[SerializeField]
		private float _splashRadius;
		[SerializeField]
		private float _zOffset;

		private SphereCollider _triggerSplash;

		protected override void Start()
		{
			base.Start();
			_triggerSplash = gameObject.AddComponent<SphereCollider>();
			_triggerSplash.center = new Vector3(0f,0f,_zOffset);
			_triggerSplash.isTrigger = true;
			_triggerSplash.radius = _triggerRadius;
		}

		private void OnTriggerEnter(Collider other)
		{
			_triggerSplash.isTrigger = false;
			_triggerSplash.radius = _splashRadius;
		}
	}
}
