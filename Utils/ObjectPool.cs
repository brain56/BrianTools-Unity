using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BrianTools
{
	/// <summary>
	/// This utility class provides facilities
	/// for the creation and recycling of 
	/// duplicate or cloned game objects to eliminate
	/// the overhead of having to create copies of an object
	/// multiple times when previous instances can be
	/// restarted and re-used.
	/// </summary>
	public class ObjectPool
	{

		/// <summary>
		/// Clones line up in a queue and take turns for spawning
		/// </summary>
		private Queue _objectQueue;

		/// <summary>
		/// A list of all objects which belong to this pool. Used for bookkeeping.
		/// </summary>
		private List<GameObject> _objectsInPool;

		/// <summary>
		/// The prefab or the original game object upon which the clones in the pool are based.
		/// </summary>
		private GameObject _originalPrefab;

		/// <summary>
		/// The parent game object of the pool for 
		/// organization in the Unity Editor hierarchy.
		/// </summary>
		private GameObject _poolParent;

		/// <summary>
		/// If set to true, the pool will create new 
		/// clones as demanded if there are not enough 
		/// clones in the pool. If false, the pool will just 
		/// return null if it has no clone to spare.
		/// </summary>
		private bool _doesExpand;

		/// <summary>
		/// If set to true, pool instances will persist
		/// across scenes assuming that they're returned
		/// to the pool correctly before transitioning to another
		/// scene.
		/// </summary>
		private bool _dontDestroyClonesOnLoad;

		public ObjectPool(GameObject pOriginalPrefab, uint startSize = 10, bool pDoesExpand = true, bool dontDestroyClonesOnLoad = false)
		{
			_dontDestroyClonesOnLoad = dontDestroyClonesOnLoad;
			_doesExpand = pDoesExpand;
			_objectQueue = new Queue();
			_objectsInPool = new List<GameObject>();
			_originalPrefab = pOriginalPrefab;

			_poolParent = new GameObject();

			if(_dontDestroyClonesOnLoad)
			{
				GameObject.DontDestroyOnLoad(_poolParent);
			}

			_poolParent.name = this._originalPrefab.name + " Pool";

			for (int i = 0; i < startSize; i++)
			{
				CreateClone();
			}
		}


		/// <summary>
		/// Creates one clone in this pool
		/// </summary>
		private void CreateClone()
		{
			GameObject clone = GameObject.Instantiate(this._originalPrefab);

			if (_dontDestroyClonesOnLoad)
			{
				GameObject.DontDestroyOnLoad(clone);
			}

			clone.name = this._originalPrefab.name;
			_objectsInPool.Add(clone);
			ReturnToPool(clone);
			clone.transform.parent = this._poolParent.transform;

		}

		/// <summary>
		/// Gets one clone from the pool for usage. When a new gameobject reference
		/// is received, the game object is deactivated and management/re-initialization 
		/// of it is delegated to the receiver.
		/// </summary>
		/// <returns>A clone</returns>
		public GameObject GetClone()
		{
			if (this._objectQueue.Count == 0)
			{
				if (!this._doesExpand)
				{
					return null;
				}

				CreateClone();
				Debug.Log("Pool empty. Creating more " + this._originalPrefab.name + "clones. New clone count: " + this._objectsInPool.Count);
			}

			GameObject clone = this._objectQueue.Dequeue() as GameObject;
			return clone;
		}

		/// <summary>
		/// Returns the gameObject to this pool,
		/// but only if that gameObject belongs to this pool.
		/// 
		/// The game object is deactivated when it returns
		/// to the object pool.
		/// </summary>
		/// <param name="pGameObject">The GameObject to return.</param>
		public void ReturnToPool(GameObject pGameObject)
		{
			if (!this._objectsInPool.Contains(pGameObject))
			{
				Debug.LogError("The gameObject " + pGameObject.name + " does not belong in this pool of " + this._originalPrefab.name + "s!");
			}

			pGameObject.SetActive(false);
			this._objectQueue.Enqueue(pGameObject);
		}
	}
}