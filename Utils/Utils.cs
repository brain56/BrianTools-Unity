using UnityEngine;
using System.Collections.Generic;
using System;

namespace BrianTools
{
	public struct Tuple<T, U>
	{

		public T x;
		public U y;

		public Tuple(T x, U y)
		{
			this.x = x;
			this.y = y;
		}

	}

	public static class UtilExtensions
	{
		public static void SafeInvoke(this Action action)
		{
			if (action != null)
			{
				action();
			}
		}

		public static void Normalize(this Transform trans)
		{
			trans.localPosition = Vector3.zero;
			trans.localEulerAngles = Vector3.zero;
			trans.localScale = Vector3.one;
		}

		public static void AddUnique<T>(this List<T> list, T item)
		{
			if (!list.Contains(item))
			{
				list.Add(item);
			}
		}

		public static void SetLayerRecursively(this GameObject gameObject, int layer)
		{
			gameObject.layer = layer;
			foreach (Transform t in gameObject.transform)
			{
				t.gameObject.SetLayerRecursively(layer);
			}
		}
		
		public static void DestroyChildren(this Transform transform)
		{
			while(transform.childCount > 0)
			{
				Transform child = transform.GetChild(0);
				child.SetParent(null);
				GameObject.Destroy(child.gameObject);
			}
		}
	}
}