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
	private int _priorityRating;

	/// <summary>
	/// Smaller == higher!
	/// </summary>
	public int PriorityRating
	{
		get { return _priorityRating; }
	}
}
