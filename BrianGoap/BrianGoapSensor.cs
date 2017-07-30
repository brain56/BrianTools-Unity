using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BrianGoapAgentMemory))]
public class BrianGoapSensor : MonoBehaviour
{

	protected BrianGoapAgentMemory _memory;

	protected virtual void Awake()
	{
		_memory = GetComponent<BrianGoapAgentMemory>();
	}

	public virtual void UpdateSensor()
	{

	}

}
