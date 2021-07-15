using System.Collections.Generic;

//make the dictionary elements their own serializable class
//so we can edit them in the inspector
[System.Serializable]
public struct WorldState
{
    public string key;
    public int value;

    public WorldState(string _key, int _value )
    {
        key = _key;
        value = _value;
    }
}

public class WorldStatesHandle
{
    public Dictionary<string, int> states;
    // Constructor
    public WorldStatesHandle( )
    {
        states = new Dictionary<string, int>( );
    }

    /************** Helper funtions ****************/
    // Check for a key
    public bool HasState(string key)
    {
        return states.ContainsKey( key );
    }

    // Add to our dictionary
    private void AddState(string key, int value)
    {
        states.Add( key, value );
    }

    /// <summary>
    /// Add up or reduce state's value by the "value" specified; If not exist, create a state.
    /// </summary>
    public void ModifyState(string key, int value)
    {
        // If it contains this key
        if(HasState( key ))
        {
            // Add the value to the state
            states[key] += value;
            // If it's less than zero then remove it
            if(states[key] <= 0)
            {
                // Call the RemoveState method
                RemoveState( key );
            }
        }
        else
        {
            AddState( key, value );
        }
    }

    // Method to remove a state
    public void RemoveState(string key)
    {
        // Check if it frist exists
        if(HasState( key ))
        {
            states.Remove( key );
        }
    }

/// <summary>
/// Sets the state's value by the "value" specified. If not exist, create the state.
/// </summary>
    public void SetState(string key, int value)
    {
        // Check if it exists
        if(HasState( key ))
        {
            states[key] = value;
        }
        else
        {
            AddState( key, value );
        }
    }


    /// <returns> The sum of World states</returns>
    public Dictionary<string, int> GetStates( )
    {
        return states;
    }
}
