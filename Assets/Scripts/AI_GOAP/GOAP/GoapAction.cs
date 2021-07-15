
using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class GoapAction : MonoBehaviour
{
	private bool inRange = false;

	public float cost = 1f;                                         // The cost of performing the action.  
	public float actionDuration;
	public GameObject target;                               // An action often has to perform on an object. Can be null.
	protected float countTimer;

	public WorldState[] preconditionsArray;
	public WorldState[] effectsArrary;
	private Dictionary<string, int> preconditions;
	private Dictionary<string, int> effects;

	#region Property
	public Dictionary<string, int> Preconditions { get => preconditions; }
	public Dictionary<string, int> Effects { get => effects; }
	#endregion

	public GoapAction( )
	{
		preconditions = new Dictionary<string, int>( );
		effects = new Dictionary<string, int>( );
	}

	private void Awake( )
	{
		LoadConditionsFromEditor( );
		//get beliefs and inventory down here as well
	}

	void LoadConditionsFromEditor( )
	{
		if(preconditionsArray != null)
		{
			foreach(WorldState p in preconditionsArray)
			{
				preconditions.Add( p.key, p.value );
			}
		}

		if(effectsArrary != null)
		{
			foreach(WorldState e in effectsArrary)
			{
				effects.Add( e.key, e.value );
			}
		}
	}

	public void DoReset( )
	{
		inRange = false;
		target = null;
		Reset( );
	}

	// Reset any variables that need to be reset before planning happens again.
	public abstract void Reset( );

	// Is the action done?
	public abstract bool IsDone( );

	protected abstract void  Load_PreconditionEffect( );
	/// <summary>
	/// Check if we can find a target for this action. (similar function to the:  bool PrePerform() method in Penny's GOAP)
	/// </summary>
	public abstract bool CheckProceduralPrecondition(GameObject agent);

	protected bool FindTargetByClosestDistance<T>(GameObject agent, out List<T> targetList)
		where T : MonoBehaviour
	{
		// find the nearest supply pile
		T[] targetObjects = FindObjectsOfType( typeof( T ) ) as T[];
		T closest = default;
		float closestDist = 0;

		foreach(var targetObject in targetObjects)
		{
			if(closest == null)
			{
				// first one, so choose it for now
				closest = targetObject;
				closestDist = (targetObject.gameObject.transform.position - agent.transform.position).magnitude;
			}
			else
			{
				// is this one closer than the last?
				float dist = (targetObject.gameObject.transform.position - agent.transform.position).magnitude;
				if(dist < closestDist)
				{
					// we found a closer one, use it
					closest = targetObject;
					closestDist = dist;
				}
			}
		}

		if(closest == null)
		{
			targetList = null;
			return false;
		}

		List<T> temp_targetList = new List<T>( );
		temp_targetList.Add( closest );
		target = temp_targetList[0].gameObject;
		targetList = temp_targetList;

		return closest != null;
	}

	/// <summary>
	/// Run the action.
	/// Returns True if the action performed successfully or false
	/// if something happened and it can no longer perform.In this case
	/// the action queue should clear out and the goal cannot be reached. 
	/// </summary>
	public abstract bool Perform(GameObject agent);
	public abstract bool ActionToBeInvoked(GameObject agent);
	public abstract bool PostPerformConditions( );

	/// <summary>
	/// Need to move to target? True for most of the cases
	/// </summary>
	public abstract bool RequiresInRange( );

	/// <summary>
	/// Are we in range of the target?
	/// The MoveTo state will set this and it gets reset each time this action is performed.
	/// </summary>
	public bool IsInRange( )
	{
		return inRange;
	}

	public void SetInRange(bool inRange)
	{
		this.inRange = inRange;
	}

	#region Add and Remove Preconditions & Effects
	public void AddPrecondition(string key, int value)
	{
		if(!preconditions.ContainsKey( key ))
		{
			preconditions.Add( key, value );
		}
	}
	public void RemovePrecondition(string key)
	{
		preconditions.Remove( key );
	}

	public void AddEffect(string key, int value)
	{
		if(!effects.ContainsKey( key ))
		{
			effects.Add( key, value );
		}
	}
	public void RemoveEffect(string key)
	{
		effects.Remove( key );
	}
	#endregion
}