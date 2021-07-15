using System.Collections.Generic;
using UnityEngine;
using System.Collections;

/**
 * Stack-based Finite State Machine.
 * Push and pop states to the FSM.
 * 
 * States should push other states onto the stack 
 * and pop themselves off.
 */
using System;

public class FStateMachine
{
	private Stack<FSMState> stateStack = new Stack<FSMState>( );

	public delegate void FSMState(FStateMachine fsm, GameObject gameObject);

	/// <summary>
	/// stateStack.Peek( ).Invoke( this, gameObject );
	/// </summary>
	/// <param name="gameObject"></param>
	public void Update(GameObject gameObject)
	{
		if(stateStack.Peek( ) != null)
			stateStack.Peek( ).Invoke( this, gameObject );
	}

	public void pushState(FSMState state)
	{
		stateStack.Push( state );
	}

	public void popState( )
	{
		stateStack.Pop( );
	}
}
