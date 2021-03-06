﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HunterTank
{
	public class Constants
	{
		public const float CutoffValue = 0.01f;
		public const float MinAngleBetweenDirections = 1f;
		public const string MoveXAxisName = "Horizontal";
		public const string MoveYAxisName = "Vertical";
		public const string SwitchGunAxisName = "SwitchGun";
		public const string FireAxisName = "Fire1";

		public const float MinSpawnInterval = 1f;
		public const int MaxEnemies = 10;

		public const float MaxProjectileFlightTime = 20f;
		public const float RayLengthForAiming = 1000f;

		public const string GameOverScoreFormat = "You scored {0} frags";
		public const string ScoreFormat = "<b>{0}</b>";
	}
}
