using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WoodCutter : Agent_DataProvider
{
	/**
	 * Our only goal will ever be to chop logs.
	 * The ChopFirewoodAction will be able to fulfill this goal.
	 */
	public override Dictionary<string, int> CreateGoalState( )
	{
		Dictionary<string, int> goal = new Dictionary<string, int>( );

		goal.Add( "collectFireWood", 0 );
		return goal;
	}

	public override string GetAgentTypeName( )
    {
		string agentName = typeof( WoodCutter ).ToString( );
		return agentName;
	}
}

