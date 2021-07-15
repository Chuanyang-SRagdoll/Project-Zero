using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public sealed class GoapAgent : MonoBehaviour
{
	private FStateMachine stateMachine;
	private FStateMachine.FSMState idleState; // finds something to do
	private FStateMachine.FSMState moveToState; // moves to a target
	private FStateMachine.FSMState performActionState; // performs an action

	private HashSet<GoapAction> availableActions;
	private Queue<GoapAction> actionsQueue;
	public GoapAction currentAction;
	public IGoap dataProvider; // this is the implementing class that provides our world data and listens to feedback on planning

	private GoapPlanner planner;


	void Start( )
	{
		stateMachine = new FStateMachine( );
		availableActions = new HashSet<GoapAction>( );
		actionsQueue = new Queue<GoapAction>( );
		planner = new GoapPlanner( );
		FindDataProvider( );
		CreateIdleState( );
		CreateMoveToState( );
		CreatePerformActionState( );
		stateMachine.pushState( idleState );        // Set to idleState initially
		LoadActions( );
	}


	void Update( )
	{
		stateMachine.Update( gameObject );
	}


	public void AddAction(GoapAction a)
	{
		availableActions.Add( a );
	}

	public GoapAction GetAction(Type action)
	{
		foreach(GoapAction g in availableActions)
		{
			if(g.GetType( ).Equals( action ))
				return g;
		}
		return null;
	}

	public void RemoveAction(GoapAction action)
	{
		availableActions.Remove( action );
	}
	/// <summary>
	/// Cheeck if actionsQueue.Count > 0
	/// </summary>
	/// <returns> bool </returns>
	private bool HasActionPlan( )
	{
		return actionsQueue.Count > 0;
	}

	private void CreateIdleState( )
	{
		idleState = (fsm, gameOb) => {
			// GOAP planning
			Debug.Log( "<color=yellow> NPC is in idleState......</color>" );
			// get the world state and the goal we want to plan for
			Dictionary<string, int> worldState = dataProvider.GetWorldState( );
			Dictionary<string, int> goal = dataProvider.CreateGoalState( );

			// Plan
			Queue<GoapAction> actions_from_plan = planner.Plan( gameObject, availableActions, worldState, goal );
			if(actions_from_plan != null)
			{
				// we have a plan, hooray!
				actionsQueue = actions_from_plan;
				dataProvider.PlanFound( goal, actions_from_plan );              //  print: Found Plan and actions
				fsm.popState( );
				fsm.pushState( performActionState );             // switch to PerformAction state
				Debug.Log( "<color=cyan>Got Plan, switch to PerformActionState: </color>" + PrettyPrint( actionsQueue.Peek( ) ) );
			}
			else
			{
				// ugh, we couldn't get a plan
				Debug.Log( "<color=orange>No Plan, Stays in IdleState: </color>" + PrettyPrint( goal ) );
				dataProvider.PlanFailed( goal );
				fsm.popState( );                                    // move back to IdleAction state
				fsm.pushState( idleState );
			}

		};
	}

	private void CreateMoveToState( )
	{
		moveToState = (fsm, gameObj) => {
			Debug.Log( "<color=yellow> NPC is in moveToState......</color>" );
			// move the game object
			currentAction = actionsQueue.Peek( );
			if(currentAction.RequiresInRange( ) && currentAction.target == null)
			{
				Debug.Log( "<color=red>Fatal error:</color> Action requires a target but has none. Planning failed. " +
									"You did not assign the target in your Action.checkProceduralPrecondition()" );
				fsm.popState( ); // move
				fsm.popState( ); // perform
				fsm.pushState( idleState );
				return;
			}

			// get the agent to move itself
			if(dataProvider.MoveAgent( currentAction ))
			{
				fsm.popState( );    //Once MoveToState pops out,  stateMachine executes PerformActionState
			}

			/*NOTE: 
			 * Consider to add a logic when MoveAgent() is unable to be true. Otherwise AI gets stuck in the MoveToState, unable to switch state.
			 */

			#region Not needed atm
			/*MovableComponent movable = (MovableComponent) gameObj.GetComponent(typeof(MovableComponent));
			if (movable == null) {
				Debug.Log("<color=red>Fatal error:</color> Trying to move an Agent that doesn't have a MovableComponent. Please give it one.");
				fsm.popState(); // move
				fsm.popState(); // perform
				fsm.pushState(idleState);
				return;
			}

			float step = movable.moveSpeed * Time.deltaTime;
			gameObj.transform.position = Vector3.MoveTowards(gameObj.transform.position, action.target.transform.position, step);

			if (gameObj.transform.position.Equals(action.target.transform.position) ) {
				// we are at the target location, we are done
				action.setInRange(true);
				fsm.popState();
			}*/
			#endregion
		};
	}

	private void CreatePerformActionState( )
	{
		performActionState = (fsm, gameObj) => {
			Debug.Log( "<color=yellow> NPC is in PerformActionState......</color>" );

			//All actions have been performed (actionQueue == 0). Time to remove current goal? 
			if(!HasActionPlan( ) && actionsQueue != null)
			{
				Debug.Log( "<color=orange>All actions in Queue have been performed. ----> Switch to idleState</color> " );
				fsm.popState( );
				fsm.pushState( idleState );             // starts a new plan
														//dataProvider.ActionsFinished( );
				return;
			}

			// If the action is done. Remove it from the actionQueue so we can perform the next one
			if(currentAction != null)
			{
				if(currentAction.IsDone( ))
				{
					currentAction.PostPerformConditions( );
					actionsQueue.Dequeue( );
				}
			}

			if(HasActionPlan( ) && actionsQueue != null)
			{
				// perform the next action
				currentAction = actionsQueue.Peek( );

				bool inRange = currentAction.RequiresInRange( ) ? currentAction.IsInRange( ) : true;
				//Arrive at target
				if(inRange)
				{
					// we are in range, so perform the action. But the actionPerformed may not be successfully done.
					bool actionPerformed = currentAction.Perform( gameObj );

					if(!actionPerformed)
					{
						// action failed, we need to plan again
						fsm.popState( );
						fsm.pushState( idleState );
						dataProvider.PlanAborted( currentAction );         //print message
						Debug.LogWarning( "actionPerformed is false.  Switch to idleState" );
					}
				}
				//Has not arrived yet
				else { fsm.pushState( moveToState ); }              // we need to move there first
			}
		};
	}

	private void FindDataProvider( )
	{
		IGoap Igoap = GetComponent<IGoap>( );
		{
			if(Igoap != null)
			{
				//Debug.LogWarning( Igoap.GetType( ) );
				dataProvider = Igoap;
				return;
			}
		}
	}

	private void LoadActions( )
	{
		GoapAction[] actions = gameObject.GetComponents<GoapAction>( );
		foreach(GoapAction a in actions)
		{
			availableActions.Add( a );
		}
		Debug.Log( "Found loaded actions: " + PrettyPrint( actions ) );
	}

	#region Pretty Print Goal, actions etc..

	/// <summary>
	///  print the Goal of agent
	/// </summary>
	public static string PrettyPrint(Dictionary<string, int> state)
	{
		String s = "";
		foreach(KeyValuePair<string, int> kvp in state)
		{
			s += kvp.Key + ":" + kvp.Value.ToString( );
			s += ", ";
		}
		return "The Goal is:  " + s;
	}

	//Found plan
	public static string PrettyPrint(Queue<GoapAction> actions)
	{
		String s = "";
		foreach(GoapAction a in actions)
		{
			s += a.GetType( ).Name;
			s += "-> ";
		}
		s += "GOAL";
		return s;
	}

	//Found loaded Actions
	public static string PrettyPrint(GoapAction[] actions)
	{
		String s = "";
		foreach(GoapAction a in actions)
		{
			s += a.GetType( ).Name;
			s += ", ";
		}
		return s;
	}

	public static string PrettyPrint(GoapAction action)
	{
		String s = "" + action.GetType( ).Name;
		return s;
	}
	#endregion

}
