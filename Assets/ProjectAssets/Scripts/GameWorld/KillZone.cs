using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HunterTank
{
	public class KillZone : Collidable
	{
		public override float Damage
		{
			get
			{
				return float.MaxValue;
			}
		}

		public override void Collide(ICollidable other)
		{
			
		}
	}
}
