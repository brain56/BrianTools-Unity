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

		UiBlackBoardNode<T> FindNode<T>(UiBlackBoardNode<T> startingNode, List<string> nodePath, bool shouldAddMissingNode = false)
		{
			if (nodePath.Count == 0)
			{
				return null;
			}

			if (nodePath.Count == 1)
			{
				string lastKey = nodePath[0];
				if (startingNode.Key == lastKey)
				{
					return startingNode;
				}

				var lastChildNode = startingNode.Children.Find(x => x.Key == lastKey);
				if (lastChildNode == null)
				{
					if (shouldAddMissingNode)
					{
						var newChild = new UiBlackBoardNode<T>(lastKey, default(T), startingNode);
						startingNode.Children.Add(newChild);
						return newChild;
					}
				}
				else
				{
					return lastChildNode;
				}
			}

			string nextChildNodeKey = nodePath[0];
			nodePath.RemoveAt(0);

			UiBlackBoardNode<T> nextChildNode = null;

			// Find the next node in the starting node or its children
			if (startingNode.Key == nextChildNodeKey)
			{
				nextChildNode = startingNode;
			}
			else
			{
				nextChildNode = startingNode.Children.Find(x => x.Key == nextChildNodeKey);
			}

			if (nextChildNode == null)
			{
				// Next node could not be found
				if (shouldAddMissingNode)
				{
					// Start the creation of a new branch
					var newChild = new UiBlackBoardNode<T>(nextChildNodeKey, default(T), startingNode);
					startingNode.Children.Add(newChild);
					return FindNode<T>(newChild, nodePath, shouldAddMissingNode);
				}
				else
				{
					// Nothing more we can do!
					return null;
				}
			}
			else
			{
				// Continue traversal down the child nodes
				return FindNode<T>(nextChildNode, nodePath, shouldAddMissingNode);
			}
		}

		public void AddIntDelegate(string nodePath, ValueUpdatedDelegate<int> newDelegate)
		{
			List<string> tokenizedNodePath = new List<string>(TokenizeNodePath(nodePath));
			var firstNode = _intBlackboard.Find(x => x.Key == tokenizedNodePath[0]);

			if (firstNode == null)
			{
				firstNode = new UiBlackBoardNode<int>(tokenizedNodePath[0], default(int), null);
				_intBlackboard.Add(firstNode);
			}

			var foundNode = FindNode<int>(firstNode, tokenizedNodePath, true);
			foundNode.ValueChangeDelegates.Add(newDelegate);
		}

		public void RemoveIntDelegate(string nodePath, object delegateTargetObject)
		{
			List<string> tokenizedNodePath = new List<string>(TokenizeNodePath(nodePath));
			var firstNode = _intBlackboard.Find(x => x.Key == tokenizedNodePath[0]);

			if (firstNode != null)
			{
				var foundNode = FindNode<int>(firstNode, tokenizedNodePath, false);
				foundNode.ValueChangeDelegates.RemoveAll(x => x.Target == delegateTargetObject);
			}
		}

		public void SetIntValue(string nodePath, int value)
		{
			List<string> tokenizedNodePath = new List<string>(TokenizeNodePath(nodePath));
			var firstNode = _intBlackboard.Find(x => x.Key == tokenizedNodePath[0]);

			if (firstNode == null)
			{
				firstNode = new UiBlackBoardNode<int>(tokenizedNodePath[0], default(int), null);
				_intBlackboard.Add(firstNode);
			}

			var foundNode = FindNode<int>(firstNode, tokenizedNodePath, true);
			foundNode.Value = value;
			InvokeValueChangeDelegates<int>(foundNode);
		}

		public int GetIntValue(string nodePath)
		{
			List<string> tokenizedNodePath = new List<string>(TokenizeNodePath(nodePath));
			var firstNode = _intBlackboard.Find(x => x.Key == tokenizedNodePath[0]);

			if (firstNode == null)
			{
				firstNode = new UiBlackBoardNode<int>(tokenizedNodePath[0], default(int), null);
				_intBlackboard.Add(firstNode);
			}

			var foundNode = FindNode<int>(firstNode, tokenizedNodePath, false);
			if (foundNode != null)
			{
				return foundNode.Value;
			}
			return 0;
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
			if (node == null)
			{
				Debug.LogError("Null node!");
				return;
			}

			for (int i = 0; i < node.ValueChangeDelegates.Count; ++i)
			{
				var valueChangeDelegate = node.ValueChangeDelegates[i];
				// TODO: Do we provide the paths as well???
				valueChangeDelegate.Invoke(node.Value);
			}
		}
	}
}