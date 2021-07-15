using UnityEngine;
using System.Collections;

public interface FSMState 
{
	
	void Update (FStateMachine fsm, GameObject gameObject);
}

