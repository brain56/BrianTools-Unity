using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrianTools.Ui
{
	public class UiBlackboard
	{
		#region Singleton
		private static UiBlackboard INSTANCE;

		public static UiBlackboard Instance
		{
			get
			{
				if (INSTANCE == null)
				{
					INSTANCE = new UiBlackboard();
				}
				return INSTANCE;
			}
		}

		UiBlackboard()
		{
			_intBlackboard = new List<UiBlackBoardNode<int>>();
			_floatBlackboard = new List<UiBlackBoardNode<float>>();
			_stringBlackboard = new List<UiBlackBoardNode<string>>();
		}
		#endregion

		List<UiBlackBoardNode<int>> _intBlackboard;

		List<UiBlackBoardNode<float>> _floatBlackboard;

		List<UiBlackBoardNode<string>> _stringBlackboard;
		
		public delegate void ValueUpdatedDelegate<T>(T value);

		public class UiBlackBoardNode<T>
		{
			public UiBlackBoardNode(string key, T value, UiBlackBoardNode<T> parent)
			{
				_key = key;
				_value = value;
				_parent = parent;
				_children = new List<UiBlackBoardNode<T>>();
				_valueChangeDelegates = new List<ValueUpdatedDelegate<T>>();
			}

			UiBlackBoardNode<T> _parent;

			List<UiBlackBoardNode<T>> _children;

			public List<UiBlackBoardNode<T>> Children
			{
				get { return _children; }
			}

			string _key;
			public string Key
			{
				get { return _key; }
			}


			T _value;
			public T Value
			{
				get
				{
					return _value;
				}
				set
				{
					_value = value;
				}

			}

			List<ValueUpdatedDelegate<T>> _valueChangeDelegates;
			public List<ValueUpdatedDelegate<T>> ValueChangeDelegates
			{
				get { return _valueChangeDelegates; }
			}
		}

		public void AddIntDelegate(string nodePath, ValueUpdatedDelegate<int> newDelegate)
		{
			List<string> tokenizedNodePath = new List<string>(TokenizeNodePath(nodePath));
			var firstNode = _intBlackboard.Find(x => x.Key == tokenizedNodePath[0]);

			if(firstNode == null)
			{
				firstNode = new UiBlackBoardNode<int>(tokenizedNodePath[0], default(int), null);
			}

			tokenizedNodePath.RemoveAt(0);

			AddDelegate<int>(firstNode, new List<string>(tokenizedNodePath), newDelegate);
		}
	
		public void AddDelegate<T>(UiBlackBoardNode<T> startingNode, List<string> nodePath, ValueUpdatedDelegate<T> newDelegate)
		{
			if (startingNode == null || nodePath.Count == 0)
			{
				Debug.LogError("Invalid node!");
				return;
			}

			string currentNodePathKey = nodePath[0];

			if (currentNodePathKey == startingNode.Key)
			{
				startingNode.ValueChangeDelegates.Add(newDelegate);
				return;
			}

			var nextKey = nodePath[0];
			var nextNode = startingNode.Children.Find(x => x.Key == nextKey);

			if (nextNode == null)
			{
				// Add new node here
				var newChild = new UiBlackBoardNode<T>(nextKey, default(T), startingNode);
				newChild.ValueChangeDelegates.Add(newDelegate);
				startingNode.Children.Add(newChild);
				return;
			}
			else if(nextNode.Key == nextKey)
			{
				nextNode.ValueChangeDelegates.Add(newDelegate);
				return;
			}

			nodePath.RemoveAt(0);
			AddDelegate<T>(nextNode, nodePath, newDelegate);
		}

		public void RemoveIntDelegate(string nodePath, object delegateTargetObject)
		{
			string[] tokenizedNodePath = TokenizeNodePath(nodePath);
			var firstNode = _intBlackboard.Find(x => x.Key == tokenizedNodePath[0]);
			RemoveDelegate<int>(firstNode, new List<string>(tokenizedNodePath), delegateTargetObject);
		}

		void RemoveDelegate<T>(UiBlackBoardNode<T> startingNode, List<string> nodePath, object delegateTargetObject)
		{
			if(startingNode == null || nodePath.Count == 0)
			{
				Debug.LogError("Invalid node!");
				return;
			}

			string currentNodePathKey = nodePath[0];

			if (currentNodePathKey == startingNode.Key)
			{
				startingNode.ValueChangeDelegates.RemoveAll( x=> x.Target == delegateTargetObject);
				return;
			}

			nodePath.RemoveAt(0);

			var nextKey = nodePath[0];
			var nextNode = startingNode.Children.Find(x => x.Key == nextKey);

			if (nextNode == null)
			{
				Debug.LogError("Invalid node path " + nodePath);
				return;
			}
			else
			{
				RemoveDelegate<T>(nextNode, nodePath, delegateTargetObject);
			}

		}

		public void SetIntValue(string nodePath, int value)
		{
			string[] tokenizedNodePath = TokenizeNodePath(nodePath);
			var firstNode = _intBlackboard.Find(x => x.Key == tokenizedNodePath[0]);
			
			if(firstNode == null)
			{
				firstNode = new UiBlackBoardNode<int>(tokenizedNodePath[0], default(int), null);
				_intBlackboard.Add(firstNode);
			}
			
			if(tokenizedNodePath.Length == 1)
			{
				firstNode.Value = value;
				return;
			}

			SetValue<int>(firstNode, new List<string>(tokenizedNodePath), value);
			
		}

		public int GetIntValue(string nodePath)
		{
			List<string> tokenizedNodePath = new List<string>(TokenizeNodePath(nodePath));
			var firstNode = _intBlackboard.Find(x => x.Key == tokenizedNodePath[0]);

			if (firstNode == null)
			{
				firstNode = new UiBlackBoardNode<int>(tokenizedNodePath[0], 0, null);
			}

			if(tokenizedNodePath.Count == 1)
			{
				return firstNode.Value;
			}

			tokenizedNodePath.RemoveAt(0);

			return GetValue<int>(firstNode, new List<string>(tokenizedNodePath));
		}

		string[] TokenizeNodePath(string nodePath)
		{
			string[] tokenizedNodePath = nodePath.Split('.');

			if (tokenizedNodePath.Length == 0)
			{
				Debug.LogError("Invalid entry path \"" + nodePath + "\" received!");
			}
			return tokenizedNodePath;
		}

		void InvokeValueChangeDelegates<T>(UiBlackBoardNode<T> node)
		{
			if(node == null)
			{
				Debug.LogError("Null node!");
				return;
			}

			for(int i = 0; i < node.ValueChangeDelegates.Count; ++i)
			{
				var valueChangeDelegate = node.ValueChangeDelegates[i];
				// TODO: Do we provide the paths as well???
				valueChangeDelegate.Invoke(node.Value);
			}
		}

		void SetValue<T>(UiBlackBoardNode<T> startingNode, List<string> nodePath, T value)
		{
			string currentNodePathKey = nodePath[0];

			if (currentNodePathKey == startingNode.Key && nodePath.Count == 1)
			{
				// If we're at the last node
				startingNode.Value = value;
				InvokeValueChangeDelegates<T>(startingNode);
				return;
			}

			nodePath.RemoveAt(0);

			var nextKey = nodePath[0];
			var nextNode = startingNode.Children.Find(x => x.Key == nextKey);

			if (nextNode == null)
			{
				var newChild = new UiBlackBoardNode<T>(nextKey, value, startingNode);
				startingNode.Children.Add(newChild);
				return;
			}
			else
			{
				SetValue<T>(nextNode, nodePath, value);
			}
		}

		T GetValue<T>(UiBlackBoardNode<T> startingNode, List<string> nodePath)
		{
			T value = default(T);

			string currentNodePathKey = nodePath[0];

			var nextKey = nodePath[0];
			var nextNode = startingNode.Children.Find(x => x.Key == nextKey);

			if (nextNode == null)
			{
				Debug.LogError("Invalid entry path \"" + nodePath + "\" received!");
				return default(T);
			}
			else if (currentNodePathKey == nextNode.Key)
			{
				return nextNode.Value;
			}

			nodePath.RemoveAt(0);

			return GetValue<T>(nextNode, nodePath);
		}

	}
}