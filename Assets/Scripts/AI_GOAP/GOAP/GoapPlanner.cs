using System.Collections.Generic;
using UnityEngine;

public class Node
{
	public Node parent;
	public float runningCost;
	public Dictionary<string, int> stateCollection;
	public GoapAction action;

    #region Constructors
    public Node(Node parent, float runningCost, Dictionary<string, int> state, GoapAction action)
	{
		this.parent = parent;
		this.runningCost = runningCost;
		this.stateCollection = state;
		this.action = action;
	}

    #endregion
}
/**
 * Plans what actions can be completed in order to fulfill a goal state.
 */
public class GoapPlanner
{
	/**
	 * Plan what sequence of actions can fulfill the goal.
	 * Returns null if a plan could not be found, or a list of the actions
	 * that must be performed, in order, to fulfill the goal. */
	public Queue<GoapAction> Plan(GameObject agent,
								  HashSet<GoapAction> availableActions,
								  Dictionary<string, int> worldState,
								  Dictionary<string, int> goal)
	{
		// reset the actions so we can start fresh with them
		foreach(GoapAction a in availableActions)
		{
			a.DoReset( );
		}

		// check what actions can run using their checkProceduralPrecondition
		HashSet<GoapAction> usableActions = new HashSet<GoapAction>( );
		foreach(GoapAction a in availableActions)
		{
			if(a.CheckProceduralPrecondition( agent ))
				usableActions.Add( a );                                     // we now have all actions that can run, stored in usableActions
		}


		// build up the tree and record the leaf nodes that provide a solution to the goal.
		List<Node> leaves = new List<Node>( );

		// build graph
		Node start = new Node( null, 0, worldState, null );
		bool success = BuildGraph( start, leaves, usableActions, goal );

		if(!success)
		{
			// oh no, we didn't get a plan
			return null;
		}

		// get the cheapest leaf
		Node cheapest = null;
		foreach(Node leaf in leaves)
		{
			if(cheapest == null)
				cheapest = leaf;
			else
			{
				if(leaf.runningCost < cheapest.runningCost)
					cheapest = leaf;
			}
		}

		// get its node and work back through the parents
		List<GoapAction> result = new List<GoapAction>( );
		Node n = cheapest;
		while(n != null)
		{
			if(n.action != null)
			{
				result.Insert( 0, n.action ); // insert the action in the front
			}
			n = n.parent;
		}      // we now have this action list in correct order

		Queue<GoapAction> queue = new Queue<GoapAction>( );
		foreach(GoapAction a in result)
		{
			queue.Enqueue( a );
		}
		return queue;           // hooray we have a plan!
	}

	/**
	 * Returns true if at least one solution was found.
	 * The possible paths are stored in the leaves list. Each leaf has a
	 * 'runningCost' value where the lowest cost will be the best action
	 * sequence.
	 */
	private bool BuildGraph(Node parent, List<Node> leaves, HashSet<GoapAction> usableActions, Dictionary<string, int> goal)
	{
		bool foundPath = false;

		// go through each action available at this node and see if we can use it here
		foreach(GoapAction action in usableActions)
		{
			// if the parent state has the conditions for this action's preconditions, we can use it here
			if(InState( action.Preconditions, parent.stateCollection ))
			{
				// apply the action's effects to the parent state
				Dictionary<string, int> currentState = new Dictionary<string, int>( parent.stateCollection );

				foreach(KeyValuePair<string, int> eff in action.Effects)                        //add the effects of this node to reflect change and effects added to WdStates 
				{
					if(!currentState.ContainsKey( eff.Key ))
					{
						currentState.Add( eff.Key, eff.Value );
					}
				}

				//Debug.Log(GoapAgent.prettyPrint(currentState));
				Node node = new Node( parent, parent.runningCost + action.cost, currentState, action );

				if(InState( goal, currentState ))
				{
					// we found a solution!
					leaves.Add( node );
					foundPath = true;
				}
				else
				{
					// not at a solution yet, so test all the remaining actions and branch out the tree
					HashSet<GoapAction> subset = ActionSubset( usableActions, action );
					bool found = BuildGraph( node, leaves, subset, goal );
					if(found)
						foundPath = true;
				}
			}
		}

		return foundPath;
	}

	/**
	 * Create a subset of the actions excluding the removeMe one. Creates a new set.
	 */
	private HashSet<GoapAction> ActionSubset(HashSet<GoapAction> actions, GoapAction removeMe)
	{
		HashSet<GoapAction> subset = new HashSet<GoapAction>( );
		foreach(GoapAction a in actions)
		{
			if(!a.Equals( removeMe ))
				subset.Add( a );
		}
		return subset;
	}


	/// <summary>
	/// Check that all of current action's Preconditions are matched in WorldState conditions. If just one does not match then this returns false.
	/// </summary>
	private bool InState (Dictionary<string,int> preconditions, Dictionary<string, int> worldStates)
    {
		foreach(KeyValuePair<string, int> p in preconditions)
		{

			if(!worldStates.ContainsKey( p.Key ))
			{
				return false;
			}
		}
		return true;
	}



}


