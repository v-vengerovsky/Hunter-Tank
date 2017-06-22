using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HunterTank
{
	public class MenuState : StateBase
	{
		private MenuView _view;

		public MenuState() : base(Approot.Instance.SceneLoader.Scenes.Find(scene => scene.SceneName == "Menu").SceneName)
		{

		}

		public override void OnActivate()
		{
			base.OnActivate();
			_view.OnStartPressed += StartGame;
		}

		public override void OnDeactivate()
		{
			base.OnDeactivate();
			_view.OnStartPressed -= StartGame;
		}

		public override ViewBase GetView()
		{
			_view = UiRoot.GetView<MenuView>();
			return _view;
		}

		public override void Update()
		{
			base.Update();

		}

		private void StartGame()
		{
			Approot.Instance.PushState(new PlayState());
		}
	}
}