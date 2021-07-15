using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SupplyPileComponent : MonoBehaviour
{
	public int numTools; // for mining ore and chopping logs
	public int numLogs; // makes firewood
	public int numFirewood; // what we want to make
	public int numOre; // makes tools

    public static implicit operator List<object>(SupplyPileComponent v)
    {
        throw new NotImplementedException( );
    }
}

