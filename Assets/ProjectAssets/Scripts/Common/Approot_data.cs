using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HunterTank
{
	public partial class Approot
	{
		[SerializeField]
		private SceneLoader _sceneLoader;

		public SceneLoader SceneLoader
		{
			get { return _sceneLoader; }
		}
	}
}
