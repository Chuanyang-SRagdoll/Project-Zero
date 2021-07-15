
using System;
using System.Collections.Generic;
using UnityEngine;

public class ChopTreeAction : GoapAction
{
	private bool chopped = false;
	private TreeComponent targetTree; // where we get the logs from

    private void Start( )
    {
		Load_PreconditionEffect( );
    }
	protected override void Load_PreconditionEffect( )
	{
		AddPrecondition( "hasTool", 0 ); // we need a tool to do this
		AddEffect( "hasLog", 0 );
	}

	public override void Reset( )
	{
		chopped = false;
		targetTree = null;
	}

	public override bool IsDone( )
	{
		return chopped;
	}

	public override bool RequiresInRange( )
	{
		return true; 
	}

	public override bool CheckProceduralPrecondition(GameObject agent)
	{
		bool found = FindTargetByClosestDistance<TreeComponent>( agent, out List<TreeComponent> targetList );
		targetTree = targetList[0];
		return found;
	}

	public override bool Perform(GameObject agent)
	{
		countTimer += Time.deltaTime;
		if(countTimer > actionDuration)
		{
			ActionToBeInvoked( agent );
			countTimer = 0f;
		}

		return true;

	}
    public override bool ActionToBeInvoked(GameObject agent)
    {
		// finished chopping 
		BackpackComponent backpack = agent.GetComponent<BackpackComponent>( );
		backpack.numLogs += 1;
		chopped = true;

		ToolComponent tool = backpack.tool.GetComponent<ToolComponent>( );
		tool.use( 0.36f );
		if(tool.destroyed( ))
		{
			Destroy( backpack.tool );
			backpack.tool = null;
		}
		return true;
	}

	public override bool PostPerformConditions( )
	{
		return true;
	}

 
}