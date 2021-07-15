using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
///  IGoap is impemented. You should implement
///  the createGoalState( )  that will populate the goal for the GOAP planner.
/// </summary>
public abstract class Agent_DataProvider : MonoBehaviour, IGoap
{
	public BackpackComponent backpack;
	public float moveSpeed = 1;


	void Start( )
	{
		if(backpack == null)
			backpack = gameObject.AddComponent<BackpackComponent>( );
        if(backpack.tool == null)
        {
            GameObject prefab = Resources.Load<GameObject>( backpack.toolType );
            GameObject tool = Instantiate( prefab, transform.position, transform.rotation );
            backpack.tool = tool;
            tool.transform.parent = transform; // attach the tool
        }
    }

	/**
	 * This is actually the individual states or beliefs about the underlying NPC, 
	 *  and these will feed the GOAP actions and system while planning.
	 */
	public  Dictionary<string, int> GetWorldState( )
	{
		Dictionary<string, int> worldData = new Dictionary<string, int>( );

        if(backpack.tool == null) { worldData.Add( "needTool", 0 ); } else worldData.Add( "hasTool", 0 );

		//worldData.Add( new KeyValuePair<string, object>( "hasLogs", (backpack.numLogs > 0) ) );
		//worldData.Add( new KeyValuePair<string, object>( "hasFirewood", (backpack.numFirewood > 0) ) );
		//worldData.Add( new KeyValuePair<string, object>( "hasTool", backpack.tool != null ) );
		//GWorld.Instance.GetWorldStatesHandle( ).GetStates( );
		return worldData;
	}

	//Implement in subclasses
	public abstract Dictionary<string, int> CreateGoalState( );

	public void PlanFailed(Dictionary<string, int> failedGoal)
	{
		/* Not handling this here since we are making sure our goals will always succeed.
		 *But normally you want to make sure the world state has changed before running
		 *the same goal again, or else it will just fail. */
	}

	public abstract string GetAgentTypeName( );

    public void PlanFound(Dictionary<string, int> goal, Queue<GoapAction> actions)
	{
		// Yay we found a plan for our goal
		Debug.Log(string.Format( "<color=cyan> {0}:  </color> " + "<color=green>Plan found (idleSate) </color> {1} ", GetAgentTypeName( ),  GoapAgent.PrettyPrint( actions ) ) );
	}

	public void ActionsFinished( )
	{
		// Everything is done, we completed our actions for this gool. Hooray!
		Debug.Log( "<color=yellow>Actions completed</color>" );
	}

	public void PlanAborted(GoapAction aborter)
	{
		// An action bailed out of the plan. State has been reset to plan again.
		// Take note of what happened and make sure if you run the same goal again
		// that it can succeed.
		Debug.Log( "<color=red>Plan Aborted -----------> </color> " + GoapAgent.PrettyPrint( aborter ) );
	}

	public bool MoveAgent(GoapAction currentAction)
	{
		// move towards the NextAction's target
		float step = moveSpeed * Time.deltaTime;
		gameObject.transform.position = Vector3.MoveTowards( gameObject.transform.position, currentAction.target.transform.position, step );

		var distance = Vector3.Distance( transform.position, currentAction.target.transform.position );
		if(distance< 2f)
		{
			// we are at the target location, we are done
			currentAction.SetInRange( true );
			return true;
		}
		else
			return false;
	}

  
}

