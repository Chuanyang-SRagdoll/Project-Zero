using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DialogueNode
{

    [SerializeField] private string nodeName;
    [SerializeField] private string parent;
    //[SerializeField] private string dialogueaction;
    //[SerializeField] private string text;
    [SerializeField] private string[] sentences;        //added

    public string NodeName { get => nodeName; set => nodeName = value; }
    public string Parent { get => parent; set => parent = value; }
    //public string DialogueAction { get => dialogueaction; set => dialogueaction = value; }
    //public string Text { get => text; set => text = value; }
    public string [ ] Sentences { get => sentences; set => sentences =  value ; }
}
