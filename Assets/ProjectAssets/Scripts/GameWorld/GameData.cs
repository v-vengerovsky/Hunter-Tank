using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HunterTank
{
	public class GameData : MonoBehaviour,ISpawnItemSource<EnemyController>
	{
		[SerializeField]
		private Camera _camera;
		[SerializeField]
		private Transform _spawnPoint;
		[SerializeField]
		private PlayerController _playerControllerPrefab;
		[SerializeField]
		private List<EnemyController> _enemyControllersPrefabs;
		[SerializeField]
		private List<SpawnVolume> _spawnVolumes;

		private int _currentPlayercontrollerIndex = 0;

		private static GameData _instance;

		public static GameData Instance
		{
			get { return _instance; }
		}

		public T Spawn<T>(T original) where T : Component
		{
			int index = UnityEngine.Random.Range(0, _spawnVolumes.Count);
			return _spawnVolumes[index].Spawn<T>(original);
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

		public EnemyController GetItem()
		{
			int randomIndex = Random.Range(0, _enemyControllersPrefabs.Count);

			return _enemyControllersPrefabs[randomIndex];
		}

		public void SetPosition(Vector3 position)
		{
			position.y = _camera.transform.position.y;
			_camera.transform.position = position;
		}

		private void Awake()
		{
			_instance = this;

		}
	}
}