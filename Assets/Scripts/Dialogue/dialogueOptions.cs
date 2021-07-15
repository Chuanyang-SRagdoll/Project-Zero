using System.Collections;
using UnityEngine;

public class dialogueOptions : MonoBehaviour
{
   [SerializeField] string optionName;
   [SerializeField] int optionIndexNum;
    // Start is called before the first frame update
    
    public void ShowOptionInfo ( )
    {
        DialogueWindow.Instance.SetOptionIndex ( optionIndexNum );
    }




}
