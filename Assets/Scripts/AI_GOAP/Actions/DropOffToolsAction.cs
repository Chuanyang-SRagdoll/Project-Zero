
using System;
using UnityEngine;

public class DropOffToolsAction : GoapAction
{
	private bool droppedOffTools = false;
	private SupplyPileComponent targetSupplyPile; // where we drop off the  tools

	//public DropOffToolsAction () {
	//	AddPrecondition ("hasNewTools", true); // can't drop off tools if we don't already have some
	//	AddEffect ("hasNewTools", false); // we now have no tools
	//	AddEffect ("collectTools", true); // we collected tools
	//}
	protected override void Load_PreconditionEffect( )
	{
		throw new NotImplementedException( );
	}

	public override void Reset ()
	{
		droppedOffTools = false;
		targetSupplyPile = null;
	}
	
	public override bool IsDone ()
	{
		return droppedOffTools;
	}
	
	public override bool RequiresInRange ()
	{
		return true; // yes we need to be near a supply pile so we can drop off the tools
	}
	
	public override bool CheckProceduralPrecondition (GameObject agent)
	{
		// find the nearest supply pile that has spare tools
		SupplyPileComponent[] supplyPiles = (SupplyPileComponent[]) UnityEngine.GameObject.FindObjectsOfType ( typeof(SupplyPileComponent) );
		SupplyPileComponent closest = null;
		float closestDist = 0;
		
		foreach (SupplyPileComponent supply in supplyPiles) {
			if (closest == null) {
				// first one, so choose it for now
				closest = supply;
				closestDist = (supply.gameObject.transform.position - agent.transform.position).magnitude;
			} else {
				// is this one closer than the last?
				float dist = (supply.gameObject.transform.position - agent.transform.position).magnitude;
				if (dist < closestDist) {
					// we found a closer one, use it
					closest = supply;
					closestDist = dist;
				}
			}
		}
		if (closest == null)
			return false;

		targetSupplyPile = closest;
		target = targetSupplyPile.gameObject;
		
		return closest != null;
	}
	
	public override bool Perform (GameObject agent)
	{
		targetSupplyPile.numTools += 2;
		droppedOffTools = true;
		//TODO play effect, change actor icon
		
		return true;
	}

    public override bool PostPerformConditions( )
    {
        throw new NotImplementedException( );
    }

    public override bool ActionToBeInvoked(GameObject agent)
    {
        throw new NotImplementedException( );
    }

 
}