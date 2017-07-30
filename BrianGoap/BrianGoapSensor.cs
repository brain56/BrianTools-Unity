using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for the GoapSensors. You may think of a 
/// BrianGoapSensor component as a "sense" that the agent has,
/// such as sight, hearing, taste, feel, etc.
/// 
/// Sensors are also used to listen in on the world's state, such 
/// as checking for an enemy presence.
/// </summary>
[RequireComponent(typeof(BrianGoapAgentMemory))]
public class BrianGoapSensor : MonoBehaviour
{
	/// <summary>
	/// Reference to the agent's memory component,
	/// cached on Awake().
	/// </summary>
	protected BrianGoapAgentMemory _memory;

	protected virtual void Awake()
	{
		_memory = GetComponent<BrianGoapAgentMemory>();
	}

	/// <summary>
	/// Called every frame to make sure that the sensors
	/// properly update the agent of the world state.
	/// 
	/// Add your "listen" or "watch" code here, where the _memory
	/// values are modified.
	/// </summary>
	public virtual void UpdateSensor()
	{

	}
}
