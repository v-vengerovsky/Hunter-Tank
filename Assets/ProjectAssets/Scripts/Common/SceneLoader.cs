using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class SceneData
{
	public UnityEngine.Object SceneRef;
	public string SceneName;
}

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class SceneLoader : MonoBehaviour
{
	private readonly List<string> _excludeSceneNames = new List<string>() { "Main.unity"};
	private readonly List<string> _excludeScenePaths = new List<string>() {};
	private readonly List<string> _exclusiveScenePaths = new List<string>() { "Assets/ProjectAssets/Scenes/" };

	[SerializeField]
	private List<SceneData> _scenes;
	[SerializeField]
	private bool _debug;

	private int _buildSceneCount;

	public List<SceneData> Scenes { get { return _scenes; } }
    public event Action<string> OnSceneLoadCompleted; 

#if UNITY_EDITOR
	private static SceneLoader _instance;
	public static SceneLoader Instance { get { return _instance; } }
	public void Awake()
	{
		_instance = this;
	}

	static SceneLoader()
	{
		EditorApplication.update += EditorUpdateStatic;
	}
#endif

#if UNITY_EDITOR
	private static void EditorUpdateStatic()
	{
		if (_instance != null)
		{
			_instance.EditorUpdate();
		}
	}

	private void EditorUpdate()
	{
		if (_buildSceneCount != EditorBuildSettings.scenes.Count())
		{
			Init();
		}

		_buildSceneCount = EditorBuildSettings.scenes.Count();
	}

	public void Reset()
	{
		Init();
	}

	public void OnValidate()
	{
		Init();
	}

	public void Init()
	{
		_instance = this;

		if (_scenes == null)
		{
			_scenes = new List<SceneData>();
		}

		List<SceneAsset> allScenes = new List<SceneAsset>();
		string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();
		List<SceneAsset> scenesToRemove = new List<SceneAsset>();

		// get all scene assets, meeting conditions in lists
		foreach (var item in allAssetPaths)
		{
			if (_debug)
			{
				Debug.LogWarningFormat("asset:{0}",item);
			}

			if (!item.Contains("Assets"))
			{
				continue;
			}

			var assetType = AssetDatabase.GetMainAssetTypeAtPath(item);

			if (assetType == typeof(SceneAsset))
			{
				bool isExclusivePath = false;
				bool isInExcludePaths = false;
				int lastSlashInPath = item.LastIndexOf('/');
				var sceneNameWithUnity = item.Substring(lastSlashInPath + 1);
				//var sceneName = sceneNameWithUnity.Replace(".unity", "");

				foreach (var item2 in _excludeScenePaths)
				{
					if (item.Contains(item2))
					{
						isInExcludePaths = true;
						break;
					}
				}

				foreach (var item2 in _exclusiveScenePaths)
				{
					if (item.Contains(item2))
					{
						isExclusivePath = true;
						break;
					}
				}

				if (isExclusivePath && !isInExcludePaths && !_excludeSceneNames.Contains(sceneNameWithUnity))
				{
					allScenes.Add(AssetDatabase.LoadAssetAtPath<SceneAsset>(item));
				}
				else
				{
					scenesToRemove.Add(AssetDatabase.LoadAssetAtPath<SceneAsset>(item));
				}
			}
		}

		if (_debug)
		{
			foreach (var item in allScenes)
			{
				Debug.LogWarningFormat("all scenes {0}", item.name);
			}
			foreach (var item in scenesToRemove)
			{
				Debug.LogWarningFormat("scenes to remove {0}", item.name);
			}
			foreach (var item in _scenes)
			{
				Debug.LogWarningFormat("scenes count before edit {0}", item.SceneRef.name);
			}
		}

		int oldScenesCount = _scenes.Count;
		bool removedDuplicateFromLastIndex = false;
		int removedItemsCount = 0;

		for (int i = _scenes.Count - 1; i >= 0; i--)
		{
			int index = i - removedItemsCount;

			if (index < 0)
			{
				break;
			}

			//missing reference scenes or non-scenes replaced by 
			if (_scenes[index].SceneRef.IsMissingReference() || !(_scenes[index].SceneRef is SceneAsset))
			{
				//_scenes.RemoveAt(index);
				_scenes[i] = new SceneData();
				continue;
			}

			if (_scenes[index] != null && _scenes[index].SceneRef != null)
			{
				//remove scenes with forbidden path or name
				var sceneAsset = scenesToRemove.Find(s => s.name == _scenes[index].SceneRef.name);

				if (sceneAsset != null)
				{
					_scenes.RemoveAt(index);
				}

				//remove duplicated scenes
				var scenes = _scenes.FindAll(r => r.SceneRef != null);
				var sceneAssets = scenes.FindAll(s => s.SceneRef.name == _scenes[index].SceneRef.name);

				if (sceneAssets.Count > 0 && sceneAssets[0].SceneRef != null)
				{
					if (sceneAssets.Contains(_scenes[_scenes.Count - 1]) && sceneAssets.Count > 1)
					{
						removedDuplicateFromLastIndex = true;
					}

					for (int j = sceneAssets.Count - 1; j > 0; j--)
					{
						_scenes.Remove(sceneAssets[j]);
						removedItemsCount++;
					}
				}
			}
		}

		if (removedDuplicateFromLastIndex)
		{
			for (int i = oldScenesCount - _scenes.Count; i > 0; i--)
			{
				_scenes.Add(new SceneData());
			}
		}

		if (_debug)
		{
			foreach (var item in _scenes)
			{
				Debug.LogWarningFormat("scenes count after remove {0}", item.SceneRef.name);
			}
		}

		//add scenes from build settings if neccessary to _scenes
		for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
		{
			var scenePath = SceneUtility.GetScenePathByBuildIndex(i).Replace(@"\",@"/");
			int lastSlashInPath = scenePath.LastIndexOf('/');
			var sceneNameWithUnity = scenePath.Substring(lastSlashInPath+1);
			var sceneName = sceneNameWithUnity.Replace(".unity","");
			var scene = SceneManager.GetSceneByPath(scenePath);

			if (!_excludeSceneNames.Contains(sceneNameWithUnity))
			{
				var scenes = _scenes.FindAll(r => r.SceneRef != null);
				SceneData sceneItem = scenes.Find(r => r.SceneRef.name == sceneName);

				if (sceneItem == null)
				{
					var sceneAsset = allScenes.Find(s => s.name == sceneName);
					if (sceneAsset != null)
					{
						_scenes.Add(new SceneData() { SceneRef = sceneAsset,SceneName = sceneAsset.name});
					}
				}
			}
		}

		foreach (var item in _scenes)
		{
			if (item.SceneRef != null && item.SceneName != item.SceneRef.name)
			{
				item.SceneName = item.SceneRef.name;
			}
		}

		if (_debug)
		{
			foreach (var item in _scenes)
			{
				Debug.LogWarningFormat("scenes count after add {0}", item.SceneRef.name);
			}
		}
	}

	[MenuItem("LoadScenes/All(Additively)")]
	static void LoadRange()
	{
		if (_instance == null)
		{
			if (SceneManager.GetActiveScene().rootCount > 0)
			{
				_instance = SceneManager.GetActiveScene().GetRootGameObjects()[0].AddComponent<SceneLoader>();
			}
			else
			{
				_instance = new GameObject().AddComponent<SceneLoader>();
			}
		}

		foreach (var item in _instance.Scenes)
		{
			//_instance.LoadScene(item,null,false);
			string path = AssetDatabase.GetAssetPath(item.SceneRef);
			EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
		}
	}
#endif

	public void LoadScene(SceneData scene, Action callback)
	{
		LoadScene(scene, callback, true);
	}

	public void LoadScene(SceneData scene, Action callback, bool unloadOtherScenes)
	{
		LoadScene(scene.SceneName, callback, unloadOtherScenes);
	}

	public void LoadScene(string sceneName, Action callback)
	{
		LoadScene(sceneName, callback, true);
	}
	
	public void LoadScene(string sceneName, Action callback, bool unloadOtherScenes)
	{
		var  loadedScenes = new List<Scene>();
		var  loadedScenesCount = SceneManager.sceneCount;
		SceneData sceneAsset;

		if (unloadOtherScenes)
		{
			for (int i = 0; i < loadedScenesCount; i++)
			{
				var scene = SceneManager.GetSceneAt(i);
				sceneAsset = _scenes.Find(r => r.SceneName == scene.name);

				if (sceneAsset != null)
				{
					StartCoroutine(UnloadSceneCoroutine(scene));
				}
			}
		}

		sceneAsset = _scenes.Find(r => r.SceneName == sceneName);

		if (sceneAsset != null)
		{
			StartCoroutine(LoadSceneCoroutine(sceneAsset.SceneName,callback));
		}
	}

	private IEnumerator UnloadSceneCoroutine(Scene scene)
	{
		yield return SceneManager.UnloadSceneAsync(scene);
	}

	private IEnumerator LoadSceneCoroutine(string sceneName, Action callback)
	{
		yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

		SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

		if (callback != null)
		{
			callback.Invoke();
		}

        if ( OnSceneLoadCompleted != null )
		{
            OnSceneLoadCompleted(sceneName);
        }
	}
}
