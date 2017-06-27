using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HunterTank
{
	public class HealthView:MonoBehaviour
	{
		[SerializeField]
		private Color _startColor;
		[SerializeField]
		private Color _endColor;
		[SerializeField]
		private Vector2 _offset;
		[SerializeField]
		private Vector2 _barStartSize;
		[SerializeField]
		private Vector2 _barPivot;
		[SerializeField]
		private GameObject _panel;

		public Color GetColor(float fill)
		{
			return Color.Lerp(_endColor, _startColor, fill);
		}

		public Vector2 GetSize(float fill)
		{
			return Vector2.Lerp(new Vector2(0, _barStartSize.y), _barStartSize, fill);
		}

		public Vector2 Offset { get { return new Vector2(_offset.x - _barStartSize.x / 2, _offset.y); } }
		public Vector2 BarStartSize { get { return _barStartSize; } }
		public Vector2 BarPivot { get { return _barPivot; } }
		public GameObject Panel { get { return _panel; } }
	}
}
