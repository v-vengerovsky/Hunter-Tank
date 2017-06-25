using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HunterTank
{
	public interface ICollidable
	{
		void Collide(ICollidable other);
		float Damage { get; }
	}
}
