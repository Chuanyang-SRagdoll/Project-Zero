using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class NPCModeller : MonoBehaviour
{
    [SerializeField] private DialogueWindow dialogueWindow;

    private static NPCModeller instance;

    /// <summary>
    /// Propertites
    /// </summary> 
    public static NPCModeller Instance                  //Singleton Instance
    {
        get
        {
            if ( instance == null )
            { instance = FindObjectOfType<NPCModeller> ( ); }
            return instance;
        }
    }           

    public DialogueWindow GetDialogueWindow { get => dialogueWindow; private set => dialogueWindow =  value ; }

    private void Start ( )
    {
        if ( Input.GetKeyDown(KeyCode.S ))
        {
            SpawnObjects ( );
        }
        

    }


    public void TestResult ( )
    {
        Debug.LogWarning ( "this singleton is working, yeah." );
    }
    public void SpawnObjects ( )
    {
        int count = 5;
        for ( int i = 0; i < count; i++ )
        {
            var go = Instantiate ( GameObject.FindGameObjectWithTag ( "NPC" ),
                  new Vector3 ( i * 5, 0, 10 ), Quaternion.identity );
            go.name = "Bob Cloned - " + i;
        }
    }


}
