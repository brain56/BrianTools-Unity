using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BrianTools.Ui
{
	public class UiDataUpdater
	{

		#region Singleton
		private static UiDataUpdater INSTANCE;

		public static UiDataUpdater Instance
		{
			get
			{
				if (INSTANCE == null)
				{
					INSTANCE = new UiDataUpdater();
				}
				return INSTANCE;
			}
		}

		private UiDataUpdater()
		{
			this.updateListeners = new Dictionary<string, List<UiDataUpdateListener>>();
			this.textDictionary = new Dictionary<string, string>();
		}
		#endregion

		private Dictionary<string, List<UiDataUpdateListener>> updateListeners;
		private Dictionary<string, string> textDictionary;


		public void AddToListeners(UiDataUpdateListener listener)
		{
			if (!updateListeners.ContainsKey(listener.ListenerKey))
			{
				updateListeners.Add(listener.ListenerKey, new List<UiDataUpdateListener>());
			}
			updateListeners[listener.ListenerKey].Add(listener);
		}

		public void RemoveFromListeners(UiDataUpdateListener listener)
		{
			if (!this.updateListeners.ContainsKey(listener.ListenerKey))
			{
				return;
			}
			this.updateListeners[listener.ListenerKey].Remove(listener);
		}

		public void RefreshText(string key)
		{
			if (this.textDictionary.ContainsKey(key))
			{
				UpdateText(key, this.textDictionary[key]);
			}
			else
			{
				UpdateText(key, "");
			}
		}

		public void UpdateText(string key, string text)
		{
			if (!this.updateListeners.ContainsKey(key))
			{
				this.updateListeners.Add(key, new List<UiDataUpdateListener>());
				this.textDictionary.Add(key, text);
			}
			else
			{
				this.textDictionary[key] = text;
			}
			for (int i = 0; i < this.updateListeners[key].Count; i++)
			{
				this.updateListeners[key][i].UpdateText(text);
			}

		}
	}
}