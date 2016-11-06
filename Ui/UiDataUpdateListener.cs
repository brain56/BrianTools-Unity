using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace BrianTools.Ui
{
	[RequireComponent(typeof(Text))]
	public class UiDataUpdateListener : MonoBehaviour
	{
		private Text _textComponent;

		[SerializeField]
		private string _listenerKey;
		public string ListenerKey
		{
			get { return this._listenerKey; }
		}
		
		void Start()
		{
			_textComponent = GetComponent<Text>();
			
			UiDataUpdater.Instance.AddToListeners(this);
			UiDataUpdater.Instance.RefreshText(_listenerKey);
		}

		public void UpdateText(string text)
		{
			_textComponent.text = text;
		}

		void OnDestroy()
		{
			UiDataUpdater.Instance.RemoveFromListeners(this);
		}
	}
}