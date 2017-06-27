using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HunterTank
{
	public class Collidable : MonoBehaviour, ICollidable
	{
		public virtual float Damage
		{
			get
			{
				return 0f;
			}
		}

		public virtual void Collide(ICollidable other)
		{
			
		}

		private void OnCollisionEnter(Collision collision)
		{
			ICollidable other = collision.collider.GetComponent<ICollidable>();

			if (other != null)
			{
				other.Collide(this);
			}
		}
	}
}
