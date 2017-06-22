using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HunterTank
{
	public abstract class StateBase
	{
		private string _sceneName;
		private ViewBase _viewBase;
		private bool _activated = false;

		public bool Activated { get { return _activated; } }

		public string SceneName
		{
			get { return _sceneName; }
		}

		public StateBase(string sceneName)
		{
			_sceneName = sceneName;
		}

		public virtual void OnActivate()
		{
			_viewBase = GetView();
			_activated = true;
		}

		public virtual void OnDeactivate()
		{
			_activated = false;
		}

		public virtual ViewBase GetView()
		{
			return null;
		}

		public virtual void Update()
		{

		}
	}
}