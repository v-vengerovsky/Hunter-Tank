using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HunterTank
{
	public class GameOverState : StateBase
	{
		private GameOverView _view;
		private int _score;

		public GameOverState(int score) : base(Approot.Instance.SceneLoader.Scenes.Find(scene => scene.SceneName == "GameOver").SceneName)
		{
			_score = score;
		}

		public override void OnActivate()
		{
			base.OnActivate();
			_view.OnRestartPressed += Restart;
		}

		public override void OnDeactivate()
		{
			base.OnDeactivate();
			_view.OnRestartPressed -= Restart;
		}

		public override ViewBase GetView()
		{
			_view = UiRoot.GetView<GameOverView>();
			return _view;
		}

		private void Restart()
		{
			Approot.Instance.SetState(new PlayState());
		}
	}
}