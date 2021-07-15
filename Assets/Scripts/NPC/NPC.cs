using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private Window window;

    public bool IsInteracting { get; set; }

    public void Start ( )
    {
        if ( window == null )
        { window = NPCModeller.Instance.GetDialogueWindow; } //window = DialogueWindow.MyInstance;
    }

    public virtual void Interact()
    {
        if (!IsInteracting)
        {
            IsInteracting = true;
            window.Open(this);
        }
    }

    public virtual void StopInteract()
    {
        if (IsInteracting)
        {
            IsInteracting = false;
            window.Close();
        }
    }
}
