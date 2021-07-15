
using System;
using System.Collections.Generic;
using UnityEngine;

public class DropOffLogsAction : GoapAction
{
	private bool droppedOffLogs = false;
	private List<SupplyPileComponent> targetSupplyPileList = new List<SupplyPileComponent>( ); // where we drop off the logs

	private void Start( )
	{
		Load_PreconditionEffect( );
    }
	protected override void Load_PreconditionEffect( )
	{
		AddPrecondition( "hasLog", 0 );
		AddEffect( "collectLogs", 1 ); // we collected logs     //this matchs the Goal
	}

	public override void Reset( )
	{
		droppedOffLogs = false;
		targetSupplyPileList = null;
	}

	public override bool IsDone( )
	{
		return droppedOffLogs;
	}

	public override bool RequiresInRange( )
	{
		return true; // yes we need to be near a supply pile so we can drop off the logs
	}

	public override bool CheckProceduralPrecondition(GameObject agent)
	{
		bool foundTarget = FindTargetByClosestDistance<SupplyPileComponent>( agent,  out List<SupplyPileComponent> targetList );
		targetSupplyPileList = targetList;
		return foundTarget;
	}
	
	public override bool Perform(GameObject agent)
	{
		bool performed = true;
		countTimer += Time.deltaTime;
        if(countTimer> actionDuration)
        {
		    performed = ActionToBeInvoked( agent );
			countTimer = 0f;
        }
		return performed;
	}

	public override bool ActionToBeInvoked(GameObject agent)
	{
		BackpackComponent backpack = agent.GetComponent<BackpackComponent>( );
		targetSupplyPileList[0].numLogs += backpack.numLogs;
		droppedOffLogs = true;
		backpack.numLogs = 0;
		return true;
	}

	public override bool PostPerformConditions( )
	{
		Debug.LogWarning( "i fking dropped off logs -------> by PostPerformConditions() " );
		return true;
	}

 
}
