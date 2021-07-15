using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EditorUI_Component))]
[CanEditMultipleObjects]
public class EditorUIControl_Tool : Editor
{
    [SerializeField] private GameObject[] dialogueOptionPanels;

    private GameObject[] buttons= new GameObject[4];  
    private bool[] buttonBoolean = new bool[4];

    private void OnEnable()
    {
        dialogueOptionPanels = DialogueWindow.Instance.dialogueOptions_Panel;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Toggle();       

    }

    private void Toggle()
    {
        bool toggle;
        for (int i = 0; i < dialogueOptionPanels.Length; i++)
        {
            toggle = dialogueOptionPanels[i].activeSelf;
            
            buttonBoolean[i] = GUILayout.Button("¡¾DialogePanel: " + i + " ¡¿");

            switch (buttonBoolean[i])
            {
                case true :
                    dialogueOptionPanels[i].SetActive(!toggle);
                    break;

                default:
                    break;
            }
        }
    }


}
