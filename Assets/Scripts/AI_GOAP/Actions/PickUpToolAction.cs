using System;
using System.Collections.Generic;
using UnityEngine;

public class PickUpToolAction : GoapAction
{
	private bool hasTool = false;
	private SupplyPileComponent targetSupplyPile; // where we get the tool from

	private void Start( )
	{
		Load_PreconditionEffect( );
	}

	protected override void Load_PreconditionEffect( )
	{
		AddPrecondition( "needTool", 0 ); // don't get a tool if we already have one
		AddEffect( "hasTool", 1 ); // we now have a tool
	}

	public override void Reset( )
	{
		hasTool = false;
		targetSupplyPile = null;
	}

	public override bool IsDone( )
	{
		return hasTool;
	}

	public override bool RequiresInRange( )
	{
		return true; // yes we need to be near a supply pile so we can pick up the tool
	}

	// Check if we can find a target for this action. (similar function to the:  bool PrePerform() method in Penny's GOAP)
	public override bool CheckProceduralPrecondition(GameObject agent)
	{
		bool foundTarget = FindTargetByClosestDistance<SupplyPileComponent>( agent, out List<SupplyPileComponent> targetList );
		targetSupplyPile = targetList[0];
		return foundTarget;
	}

	public override bool Perform(GameObject agent)
	{
		//bool performed =ActionToBeInvoked(  agent );
		//return performed;
		bool performed = true;
		countTimer += Time.deltaTime;
		if(countTimer > actionDuration)
		{
			performed = ActionToBeInvoked( agent );
			countTimer = 0f;
		}
		return performed;
	}

    public override bool ActionToBeInvoked(GameObject agent)
    {
		if(targetSupplyPile.numTools > 0)
		{
			targetSupplyPile.numTools -= 1;

			// create the tool and add it to the agent
			BackpackComponent backpack = agent.GetComponent<BackpackComponent>( );
			GameObject prefab = Resources.Load<GameObject>( backpack.toolType );
			GameObject tool = Instantiate( prefab, transform.position, transform.rotation );
			backpack.tool = tool;
			tool.transform.parent = transform; // attach the tool

			hasTool = true;
			return true;
		}
		else
		{
			Debug.LogWarning( " we arrive target location, but there was no tool available. ---> Cannot perform action" );
			return false;
		}
	}

	public override bool PostPerformConditions( )
	{
		return true;
	}
}


