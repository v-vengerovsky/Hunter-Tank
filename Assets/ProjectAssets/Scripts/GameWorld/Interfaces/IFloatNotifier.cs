using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HunterTank
{
	public interface IFloatNotifier: INotifier
	{
		RangeFloat RangeFloat { get; }
	}

	public struct RangeFloat
	{
		private float _min;
		private float _max;
		private float _value;

		public RangeFloat(float value,float max,float min = 0)
		{
			_value = value;
			_min = min;
			_max = max;
		}

		public float Min { get { return _min; } }
		public float Max { get { return _max; } }
		public float Value { get { return _value; } }
	}
}
