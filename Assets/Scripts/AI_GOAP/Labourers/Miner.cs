using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Miner : Agent_DataProvider
{
	/**
	 * Our only goal will ever be to mine ore.
	 * The MineOreAction will be able to fulfill this goal.
	 */
	public override Dictionary<string, int> CreateGoalState( )
	{
		Dictionary<string, int> goal = new Dictionary<string, int>( );

		goal.Add( "collectOres", 0 );
		return goal;
	}

	public override string GetAgentTypeName( )
    {
		string agentName = typeof( Miner ).ToString( );
		return agentName;
	}
}

