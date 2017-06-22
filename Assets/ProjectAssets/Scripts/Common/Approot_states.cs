using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HunterTank
{
	public partial class Approot
	{
		private StateBase _currentState;
		private Stack<StateBase> _states = new Stack<StateBase>();

		public void PushState(StateBase state)
		{
			if (_currentState != null)
			{
				_currentState.OnDeactivate();
			}

			_states.Push(_currentState);
			_currentState = state;
			PrepareState(_currentState);
		}

		public void SetState(StateBase state)
		{
			if (_currentState != null)
			{
				_currentState.OnDeactivate();
			}

			_states.Clear();
			_currentState = state;
			PrepareState(_currentState);
		}

		public void PopState()
		{
			_currentState.OnDeactivate();
			_currentState = _states.Pop();
			PrepareState(_currentState);
		}

		private void PrepareState(StateBase state)
		{
			SceneLoader.LoadScene(state.SceneName, state.OnActivate);
		}
	}
}