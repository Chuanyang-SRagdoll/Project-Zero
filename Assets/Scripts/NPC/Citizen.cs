using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Citizen : NPC
{
    /// <summary>
    /// NPC has IInteractiable interface, which open a window.
    /// </summary>
    /// 
    [SerializeField] private Dialogue dialogue;

     new void Start ( )
    {
        base.Start ( );
        dialogue = this.GetComponent<Dialogue> ( );
    }

    public override void Interact()     // To get called in GameManager
    {
        base.Interact();
        DialogueWindow.Instance.SetDialogue(dialogue);
    }
}
