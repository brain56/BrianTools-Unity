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
				}
				return sInstance;
			}
		}

		[SerializeField]
		private List<GameObject> _managers;

		public GameObject GetManagerPrefab<T>() where T : Component
		{
			GameObject prefab = _managers.Find(x => x.GetComponent<T>() != null);
			return prefab;
		}

#if UNITY_EDITOR
		[ContextMenu("AlphabetizeManagersList()")]
		public void AlphabetizeManagersList()
		{
			_managers.Sort(delegate(GameObject a, GameObject b)
			{
				return a.name.CompareTo(b.name);
			});
		}
#endif

	}
}