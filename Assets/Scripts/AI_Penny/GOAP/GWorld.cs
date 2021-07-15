using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceQueue
{
    public Queue<GameObject> que = new Queue<GameObject>( );
    public string tag;
    public string stateKey;

    public ResourceQueue(string _tag, string  _modifyStateKey, WorldStatesHandle _WorldStatesHandle)
    {
        tag = _tag;
        stateKey = _modifyStateKey;
        if(tag != string.Empty)
        {
            GameObject[] resources = GameObject.FindGameObjectsWithTag( tag );       

            foreach(GameObject r in resources)
            {
                que.Enqueue( r );
            }
        }

        if(stateKey != string.Empty)
        {
            _WorldStatesHandle.ModifyState( stateKey, que.Count );
        }
    }

    // Add the resource
    public void AddResource(GameObject r)
    {
        que.Enqueue( r );
    }


    /// <returns>que.Dequeue( )</returns>
    public GameObject RemoveResource( )
    {
        // If no items in the Resources Queue
        if(que.Count == 0) return null;
        return que.Dequeue( );
    }

    /// <summary>
    ///  Overloaded RemoveResource
    /// </summary>
    public void RemoveResource(GameObject r)
    {
        // Put everything in a new queue except 'r' and copy it back to que
        que = new Queue<GameObject>( que.Where( p => p != r ) );
    }
}

public sealed class GWorld
{
    // Our GWorld instance
    private static readonly GWorld instance = new GWorld( );
    // Our world states
    private static WorldStatesHandle worldStatesHandle;
    // Queue of patients
    private static ResourceQueue Character_01;
    // Queue of cubicles
    private static ResourceQueue cubicles;
    // Queue of offices
    private static ResourceQueue offices;
    // Queue of toilets
    private static ResourceQueue toilets;
    // Queue for the puddles
    private static ResourceQueue puddles;

    // Storage for all resources
    private static Dictionary<string, ResourceQueue> resources = new Dictionary<string, ResourceQueue>( );

    /// <summary>
    /// Constructors
    /// </summary>
    private GWorld( )
    {
    }

    public static GWorld Instance
    {
        get { return instance; }
    }
    static GWorld( )
    {
        // Create our world
        worldStatesHandle = new WorldStatesHandle( );

        // Create patients array
        Character_01 = new ResourceQueue( string.Empty, string.Empty, worldStatesHandle );
        // Add to the resources Dictionary
        resources.Add( "Character_Tier1", Character_01 );

        //// Create cubicles array          
        //cubicles = new ResourceQueue( "Cubicle", "FreeCubicle", worldStatesHandle );
        //// Add to the resources Dictionary
        //resources.Add( "cubicles", cubicles );

        //// Create offices array
        //offices = new ResourceQueue( "Office", "FreeOffice", worldStatesHandle );
        //// Add to the resources Dictionary
        //resources.Add( "offices", offices );

        //// Create toilet array
        //toilets = new ResourceQueue( "Toilet", "FreeToilet", worldStatesHandle );
        //// Add to the resources Dictionary
        //resources.Add( "toilets", toilets );
        //// Create puddles array
        //puddles = new ResourceQueue( "Puddle", "FreePuddle", worldStatesHandle );
        //// Add to the resources Dictionary
        //resources.Add( "puddles", puddles );
    }

    public ResourceQueue GetQueue(string type)
    {
        return resources[type];
    }

    public WorldStatesHandle GetWorldStatesHandle( )
    {
        return worldStatesHandle;
    }
}
