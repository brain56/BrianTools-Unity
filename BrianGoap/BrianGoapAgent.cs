using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BrianGoapAgentMemory))]
public class BrianGoapAgent : MonoBehaviour
{
	private List<BrianGoapGoal> _goals;
	private List<BrianGoapAction> _actions;
	private List<BrianGoapSensor> _sensors;
	private BrianGoapAgentMemory _memory;

	protected virtual void Awake()
	{
		_goals = new List<BrianGoapGoal>(GetComponents<BrianGoapGoal>());
		_actions = new List<BrianGoapAction>(GetComponents<BrianGoapAction>());
		_sensors = new List<BrianGoapSensor>(GetComponents<BrianGoapSensor>());
		_memory = GetComponent<BrianGoapAgentMemory>();
	}

	private bool _isActing;

	//[SerializeField]
	//private bool _currentGoalUnattainable;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (!_isActing)
		{
			StartCoroutine(ResolveGoals());
		}
		UpdateSensors();
	}

	private void UpdateSensors()
	{
		foreach (var sensor in _sensors)
		{
			sensor.UpdateSensor();
		}
	}

	private IEnumerator ResolveGoals()
	{
		var unsatisfiedGoals = new List<BrianGoapGoal>();
		foreach (var goalComponent in _goals)
		{
			foreach (var goalData in goalComponent.GoalData.Data)
			{
				// If the memory is oblivious about that goal's world state, we don't consider it unsatisfied.
				// Just ignore it.
				if (_memory.ContainsKey(goalData.Key) && goalData.Value != _memory.GetWorldState(goalData.Key))
				{
					unsatisfiedGoals.Add(goalComponent);
				}
			}
		}

		if (unsatisfiedGoals.Count == 0)
		{
			// No goal to resolve!
			yield break;
		}

		unsatisfiedGoals.Sort(delegate (BrianGoapGoal x, BrianGoapGoal y)
		{
			return x.PriorityRating.CompareTo(y.PriorityRating);
		});

		var goalToResolve = unsatisfiedGoals[0];
		List<BrianGoapAction> actionCandidates = new List<BrianGoapAction>();

		// Find action to fix goal
		foreach (var actionComponent in _actions)
		{
			if (!actionComponent.ArePreconditionsMet())
			{
				continue;
			}

			var actionEffects = actionComponent.GetEffects();
			int matchCount = 0;
			foreach (var goalData in goalToResolve.GoalData.Data)
			{
				if (actionEffects.ContainsKey(goalData.Key) && goalData.Value == actionEffects[goalData.Key])
				{
					matchCount++;
				}
			}
			if (matchCount == goalToResolve.GoalData.Data.Count)
			{
				actionCandidates.Add(actionComponent);
			}
		}

		if (actionCandidates.Count == 0)
		{
			// Nothing we can do to meet goal!
			//_currentGoalUnattainable = true;
			yield break;
		}

		//_currentGoalUnattainable = false;

		actionCandidates.Sort(delegate (BrianGoapAction x, BrianGoapAction y)
		{
			return -x.ActionCost.CompareTo(y.ActionCost);
		});

		var candidateAction = actionCandidates[0];

		_isActing = true;
		yield return candidateAction.PerformAction();
		// TODO: Check for interruptions
		_isActing = false;
	}

}
