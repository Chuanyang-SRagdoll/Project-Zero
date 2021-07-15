using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Logger : Agent_DataProvider
{
    public override string GetAgentTypeName( )
    {
		string agentName = typeof( Logger ).ToString( );
        return agentName ;
    }

    public override Dictionary<string, int> CreateGoalState () {
		Dictionary<string, int> goal = new Dictionary<string, int>();
		
		goal.Add("collectLogs", 0 );
		return goal;
	}

}

