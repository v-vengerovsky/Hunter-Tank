using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HunterTank
{
	public class GameData : MonoBehaviour
	{
		[SerializeField]
		private List<PlayerController> _playerControllers;
		[SerializeField]
		private List<EnemyController> _enemyControllers;

		private int _currentPlayercontrollerIndex = 0;

		private static GameData _instance;

		public static GameData Instance
		{
			get { return _instance; }
		}

		public PlayerController GetNextPlayerController(Transform parent = null)
		{
			_currentPlayercontrollerIndex++;
						
			return GetPlayerControllerAt(_currentPlayercontrollerIndex,parent);
		}

		public PlayerController GetPreviousPlayerController(Transform parent = null)
		{
			_currentPlayercontrollerIndex--;

			return GetPlayerControllerAt(_currentPlayercontrollerIndex, parent);
		}

		public PlayerController GetDefaultPlayerController(Transform parent = null)
		{
			return GetPlayerControllerAt(0, parent);
		}

		public EnemyController GetRandomEnemyController(Transform parent = null)
		{
			int randomIndex = Random.Range(0, _playerControllers.Count);

			var result = Instantiate<EnemyController>(_enemyControllers[randomIndex]);

			if (parent != null)
			{
				result.transform.SetParent(parent);
			}

			return result;
		}

		private void Awake()
		{
			_instance = this;

		}

		private PlayerController GetPlayerControllerAt(int index, Transform parent)
		{			
			index = (int)(Mathf.Repeat(index, _playerControllers.Count));

			var result = Instantiate<PlayerController>(_playerControllers[index]);

			if (parent != null)
			{
				result.transform.SetParent(parent);
			}

			return result;

		}
	}
}