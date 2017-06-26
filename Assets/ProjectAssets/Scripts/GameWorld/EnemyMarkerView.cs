//EnemyMarkerView
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace HunterTank
{
	public class EnemyMarkerView : MonoBehaviour
	{
		[SerializeField]
		private Sprite _arrow;
		[SerializeField]
		private float _arrowOffsetAngle;
		[SerializeField]
		private Vector2 _arrowSize;
		[SerializeField]
		private Vector2 _arrowPivot;
		[SerializeField]
		private GameObject _panel;

		public Sprite Arrow { get { return _arrow; } }
		public float ArrowOffsetAngle { get { return _arrowOffsetAngle; } }
		public Vector2 ArrowSize { get { return _arrowSize; } }
		public Vector2 ArrowPivot { get { return _arrowPivot; } }
		public GameObject Panel { get { return _panel; } }
	}
}