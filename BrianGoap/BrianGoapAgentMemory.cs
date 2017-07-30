using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is my brain. 
/// </summary>
public class BrianGoapAgentMemory : MonoBehaviour
{
	/// <summary>
	/// Dictionary of values to store data
	/// into the agent's memory
	/// </summary>
	[SerializeField]
	private BrianGoapData _worldState = new BrianGoapData();
	public BrianGoapData WorldState
	{
		get { return _worldState; }
	}

	public void SetWorldState(string key, bool value)
	{
		_worldState[key] = value;
	}

	public bool GetWorldState(string key)
	{
		return _worldState[key];
	}

	public bool ContainsKey(string key)
	{
		return _worldState.ContainsKey(key);
	}
}
