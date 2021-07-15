using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorUI_Component : MonoBehaviour
{
    //[SerializeField] private string Editor_Tool = "UI";

    [SerializeField] static EditorUI_Component instance;
    public static EditorUI_Component Instance                               //Singleton
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EditorUI_Component>();
            }
            return instance;
        }
    }




}
