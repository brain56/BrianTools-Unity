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
			while (transform.childCount > 0)
			{
				Transform child = transform.GetChild(0);
				child.SetParent(null);
				GameObject.Destroy(child.gameObject);
			}
		}

		/// <summary>
		/// An implementation of the Fisher-Yates Shuffle (https://stackoverflow.com/a/1262619)
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		public static void ShuffleList<T>(this IList<T> list)
		{
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = UnityEngine.Random.Range(0, n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		public static bool IsValidIndex<T>(this IList<T> list, int index)
		{
			return index < list.Count && index >= 0;
		}

		/// <summary>
		/// From https://answers.unity.com/questions/799616/unity-46-beta-19-how-to-convert-from-world-space-t.html
		/// </summary>
		/// <param name="uiElement"></param>
		/// <param name="worldPosition"></param>
		/// <param name="camera"></param>
		public static void SetToWorldPosition(this RectTransform uiElement, Vector3 worldPosition, Camera camera, Canvas canvas)
		{
			//first you need the RectTransform component of your canvas
			RectTransform canvasRect = canvas.GetComponent<RectTransform>();

			//then you calculate the position of the UI element
			//0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.
			Vector2 viewportPosition = camera.WorldToViewportPoint(worldPosition);
			Vector2 WorldObject_ScreenPosition = new Vector2(
			((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
			((viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

			//now you can set the position of the ui element
			uiElement.anchoredPosition = WorldObject_ScreenPosition;
		}

		public static bool IsNullOrEmpty(this string inString)
		{
			return inString == "" || inString == null;
		}

	}
}