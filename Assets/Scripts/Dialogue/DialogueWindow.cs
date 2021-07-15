using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueWindow : Window
{
    private static DialogueWindow instance;

    [SerializeField] private TextMeshProUGUI dialogueDisplay_Text;

    [SerializeField] private float speed;
    private int optionIndex;

    private Dialogue dialogue;
    private DialogueNode currentNode;
    private Queue<string> sentencesQ;

    public GameObject[] dialogueOptions_Panel;

    [SerializeField] private GameObject dialogueActionsButtonPrefab;
    [SerializeField] private Transform DialogueActionTrans;

    private List<DialogueNode> dialogueActions = new List<DialogueNode>();
    private List<GameObject> buttons = new List<GameObject>();

  //Dialogue Window singleton
    public static DialogueWindow Instance  
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DialogueWindow>();
            }
            return instance;
        }
    }

    new void Start()
    {
        base.Start();
        sentencesQ = new Queue<string>();
    }

    public void SetDialogue(Dialogue dialogue)
    {
        dialogueDisplay_Text.text = string.Empty;
        sentencesQ.Clear();

        this.dialogue = dialogue;

        this.currentNode = dialogue.Nodes[0];                          // the root node

        foreach(string sentence in currentNode.Sentences)
        {
            sentencesQ.Enqueue(sentence);
        }

        DisplayNextSentence();
        
    }

    //Click Next button to show
    public void DisplayNextSentence()
    {
        ClearText ( );
        if (sentencesQ.Count == 0)
        {
            CloseDialogue( );
            return;
        }

        string sentence = sentencesQ.Dequeue();
        StartCoroutine(TypeSentence(sentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        for (int i = 0; i < sentence.Length; i++)
        {
            dialogueDisplay_Text.text += sentence[i];
            yield return new WaitForSeconds(speed);
        }
    }

    private void ShowdialogueActions()
    {
        dialogueActions.Clear();

        foreach (DialogueNode node in dialogue.Nodes)
        {
            if (node.Parent == currentNode.NodeName)
            {
                dialogueActions.Add(node);                          // add all the corresponding  child nodes to the dialogueAtions list
            }
        }

        if (dialogueActions.Count > 0)
        {
            DialogueActionTrans.gameObject.SetActive(true);

            foreach (DialogueNode node in dialogueActions)
            {
                GameObject go = Instantiate(dialogueActionsButtonPrefab, DialogueActionTrans);
                buttons.Add(go);
                //go.GetComponentInChildren<TextMeshProUGUI>( ).text = node.DialogueAction;
                go.GetComponent<Button>().onClick.AddListener(delegate { PickActions(node); });
            }
        }
        //else
        //{
        //    DialogueActionTrans.gameObject.SetActive(true);
        //    GameObject go = Instantiate(dialogueActionsButtonPrefab, DialogueActionTrans);
        //    buttons.Add(go);
        //    go.GetComponentInChildren<TextMeshProUGUI>().text = "Close";
        //    go.GetComponent<Button>().onClick.AddListener(delegate { CloseDialogue(); });
        //}
    }

    private void PickActions(DialogueNode node)
    {
        //this.currentNode = node;                                                                   //currentNode updates to child node
        //Clear();

        StartCoroutine(TypeSentence(currentNode.Sentences[0]));
    }

    public void CloseDialogue()
    {
        Close();
        ClearText ( );
    }

    public void ExpandDialogueOptions ( )
    {
        bool toggle = dialogueOptions_Panel[optionIndex].activeSelf;

        dialogueOptions_Panel[optionIndex].SetActive( !toggle );
    }
 
    public void  SetOptionIndex (int _optionIndex )
    {
        optionIndex = _optionIndex;
    }

    private void ClearText()
    {
        dialogueDisplay_Text.text = string.Empty;


        //DialogueActionTrans.gameObject.SetActive(false);
        //foreach (GameObject gameObject in buttons)
        //{
        //    gameObject.SetActive ( false ); 
        //}
        //buttons.Clear();
    }
}
