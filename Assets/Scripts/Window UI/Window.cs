using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent ( typeof ( CanvasGroup ) )]
public class Window : MonoBehaviour 
{
    [SerializeField] private CanvasGroup canvasGroup;

    private NPC npc;

    public void Start ( )
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public virtual void Open ( NPC npc )
    {
        this.npc = npc;
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }

    public virtual void Close ( )
    {
        npc.IsInteracting = false;
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        npc = null;
    }
}
