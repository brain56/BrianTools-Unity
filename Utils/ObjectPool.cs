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
		Queue objectQueue;

		/// <summary>
		/// A list of all objects which belong to this pool. Used for bookkeeping.
		/// </summary>
		List<GameObject> objectsInPool;

		/// <summary>
		/// The prefab or the original game object upon which the clones in the pool are based.
		/// </summary>
		GameObject originalPrefab;

		/// <summary>
		/// The parent game object of the pool for 
		/// organization in the Unity Editor hierarchy.
		/// </summary>
		GameObject poolParent;

		/// <summary>
		/// If set to true, the pool will create new 
		/// clones as demanded if there are not enough 
		/// clones in the pool. If false, the pool will just 
		/// return null if it has no clone to spare.
		/// </summary>
		bool doesExpand;

		public ObjectPool(GameObject pOriginalPrefab, uint startSize = 10, bool pDoesExpand = true)
		{
			this.doesExpand = pDoesExpand;
			this.objectQueue = new Queue();
			this.objectsInPool = new List<GameObject>();
			this.originalPrefab = pOriginalPrefab;

			this.poolParent = new GameObject();
			this.poolParent.name = this.originalPrefab.name + " Pool";

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
			GameObject clone = GameObject.Instantiate(this.originalPrefab);
			clone.name = this.originalPrefab.name;
			objectsInPool.Add(clone);
			ReturnToPool(clone);
			clone.transform.parent = this.poolParent.transform;

		}

		/// <summary>
		/// Gets one clone from the pool for usage.
		/// </summary>
		/// <returns>A clone</returns>
		public GameObject GetClone()
		{
			if (this.objectQueue.Count == 0)
			{
				if (!this.doesExpand)
				{
					return null;
				}

				CreateClone();
				Debug.Log("Pool empty. Creating more " + this.originalPrefab.name + "clones. New clone count: " + this.objectsInPool.Count);
			}

			GameObject clone = this.objectQueue.Dequeue() as GameObject;
			return clone;
		}

		/// <summary>
		/// Returns the gameObject to this pool,
		/// but only if that gameObject belongs to this pool.
		/// </summary>
		/// <param name="pGameObject">The GameObject to return.</param>
		public void ReturnToPool(GameObject pGameObject)
		{
			if (!this.objectsInPool.Contains(pGameObject))
			{
				Debug.LogError("The gameObject " + pGameObject.name + " does not belong in this pool of " + this.originalPrefab.name + "s!");
			}

			pGameObject.SetActive(false);
			this.objectQueue.Enqueue(pGameObject);
		}
	}
}