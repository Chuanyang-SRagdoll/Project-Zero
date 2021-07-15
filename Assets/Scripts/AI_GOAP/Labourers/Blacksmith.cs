using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Blacksmith : Agent_DataProvider
{
	public override string GetAgentTypeName( )
	{
		string agentName = typeof ( Blacksmith ).ToString( );
		return agentName;
	}

	/**
	 * Our only goal will ever be to make tools.
	 * The ForgeTooldAction will be able to fulfill this goal.
	 */
	public override Dictionary<string, int> CreateGoalState( )
	{
		Dictionary<string, int> goal = new Dictionary<string, int>( );

		goal.Add( "collectTools", 0 );
		return goal;
	}


}

