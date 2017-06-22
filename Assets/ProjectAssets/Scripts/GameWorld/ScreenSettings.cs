using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HunterTank
{
	public class ScreenSettings
	{
		private const float _screenOffsetNormalized = 0.1f;

		private float _screenWith;
		private float _screenHeight;
		private float _screenOffset;

		public float ScreenWidth { get { return _screenWith; } }
		public float ScreenHeight { get { return _screenHeight; } }
		public float ScreenOffset { get { return _screenOffset; } }

		public ScreenSettings(Camera camera, GameObject ground,GameObject goToSetZ)
		{
			Vector3 targetPos = camera.transform.position;
			targetPos.z = ground.transform.position.z;
			ground.transform.position = targetPos;

			if (goToSetZ != null)
			{
				goToSetZ.transform.position = ground.transform.position;
			}

			float depth = ground.transform.position.z - camera.transform.position.z;
			Vector3 topRight = camera.ViewportToWorldPoint(new Vector3(1, 1, depth));
			Vector3 bottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, depth));
			_screenWith = topRight.x - bottomLeft.x;
			_screenHeight = topRight.y - bottomLeft.y;
			_screenOffset = Math.Max(_screenWith, _screenHeight) * _screenOffsetNormalized;
		}
	}
}
