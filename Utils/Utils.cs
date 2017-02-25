using UnityEngine;
using System.Collections.Generic;
using System;

namespace BrianTools
{
	public sealed class Constants
	{
		private static Vector3 sVector3Zero = Vector3.zero;
		public static Vector3 Vector3Zero
		{
			get { return sVector3Zero; }
		}
	}

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
			trans.localPosition = BrianTools.Constants.Vector3Zero;
			trans.localEulerAngles = BrianTools.Constants.Vector3Zero;
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