using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HunterTank
{
	public class GameData : MonoBehaviour
	{
		[SerializeField]
		private Camera _camera;
		[SerializeField]
		private Transform _spawnPoint;
		[SerializeField]
		private PlayerController _playerControllerPrefab;
		[SerializeField]
		private List<EnemyController> _enemyControllers;

		private int _currentPlayercontrollerIndex = 0;

		private static GameData _instance;

		public static GameData Instance
		{
			get { return _instance; }
		}

		public PlayerController GetPlayerController(Transform parent = null)
		{
			var result = Instantiate<PlayerController>(_playerControllerPrefab);

			if (parent != null)
			{
				result.transform.SetParent(parent);
			}

			result.transform.localScale = Vector3.one;
			result.transform.position = _spawnPoint.position;
			return result;
		}

		public EnemyController GetRandomEnemyController(Transform parent = null)
		{
			int randomIndex = Random.Range(0, _enemyControllers.Count);

			var result = Instantiate<EnemyController>(_enemyControllers[randomIndex]);

			if (parent != null)
			{
				result.transform.SetParent(parent);
			}

			result.transform.localScale = Vector3.one;
			return result;
		}

		private void Awake()
		{
			_instance = this;

		}
	}
}