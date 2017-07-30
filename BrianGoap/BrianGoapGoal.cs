using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrianGoapGoal : MonoBehaviour
{
	[SerializeField]
	private string GoalId;

	[SerializeField]
	private BrianGoapData _goals;
	public BrianGoapData GoalData
	{
		get { return _goals; }
	}

	[SerializeField]
	[Tooltip("The priority rating of this goal. A smaller number means it has higher priority, and so will be resolved first versus another goal with a lower priority.")]
	private int _priorityRating = 1;

	/// <summary>
	/// The priority rating of this goal. A smaller number means it has higher
	/// priority, and so will be resolved first versus another goal with a lower priority.
	/// </summary>
	public int PriorityRating
	{
		get { return _priorityRating; }
	}
}
