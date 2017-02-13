using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace BrianTools.ManagersManager
{
	public class ManagersManager : MonoBehaviour
	{

		private static ManagersManager sInstance;

		public static ManagersManager Instance
		{
			get
			{
				if (sInstance == null)
				{
					GameObject prefab = Resources.Load("Prefabs/Managers/ManagersManager") as GameObject;
					GameObject instance = GameObject.Instantiate(prefab);
					sInstance = instance.GetComponent<ManagersManager>();
					DontDestroyOnLoad(sInstance.gameObject);
				}
				return sInstance;
			}
		}

		[SerializeField]
		private List<GameObject> _managers;

		[SerializeField]
		private GameObject _primevalManager;

		public GameObject GetManagerPrefab<T>() where T : Component
		{
			GameObject prefab = _managers.Find(x => x.GetComponent<T>() != null);
			return prefab;
		}

		private void Awake()
		{
			if (sInstance != null)
			{
				Debug.LogWarning("Error! Duplicate ManagersManager exists! Destroying new instance;");
				GameObject.Destroy(gameObject);
			}
			else
			{
				sInstance = this;
				DontDestroyOnLoad(gameObject);

				if(_primevalManager)
				{
					GameObject instance = GameObject.Instantiate(_primevalManager);
					DontDestroyOnLoad(instance.gameObject);
				}
			}
		}

#if UNITY_EDITOR
		[ContextMenu("AlphabetizeManagersList()")]
		public void AlphabetizeManagersList()
		{
			_managers.Sort(delegate (GameObject a, GameObject b)
			{
				return a.name.CompareTo(b.name);
			});
		}
#endif
	}
}