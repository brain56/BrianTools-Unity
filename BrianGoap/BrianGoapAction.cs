using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BrianGoapAgentMemory))]
public class BrianGoapAction : MonoBehaviour
{
	protected BrianGoapAgentMemory _memory;

	[SerializeField]
	private int _actionCost = 1;
	public int ActionCost
	{
		get { return _actionCost; }
	}

	[SerializeField]
	protected BrianGoapData _effects;

	[SerializeField]
	protected BrianGoapData _preconditions;

	protected virtual void Awake()
	{
		_memory = GetComponent<BrianGoapAgentMemory>();
	}

	public virtual IEnumerator PerformAction()
	{
		yield return null;
	}

	public virtual BrianGoapData GetPreconditions()
	{
		return _preconditions;
	}

	public bool ArePreconditionsMet()
	{
		foreach(var data in _preconditions.Data)
		{
			if(data.Value != _memory.GetWorldState(data.Key))
			{
				return false;
			}
		}
		return true;
	}

	public virtual BrianGoapData GetEffects()
	{
		return _effects;
	}
}
