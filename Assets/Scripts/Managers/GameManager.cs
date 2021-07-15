using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    private Camera mainCamera;
    public CharaController charaController;

    private static GameManager instance;

    // A reference to the player object
    //[SerializeField] private Player player;

    [SerializeField] private LayerMask clickableLayer, groundLayer;

    //private Enemy currentTarget;

    private int targetIndex;

    #region Propertities
    public static GameManager Instance
    {
        get
        {
            if ( instance == null )
            {
                instance = FindObjectOfType<GameManager> ( );
            }
            return instance;
        }
    }

    #endregion

    void Start ()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        ClickTarget ( );
    }

    private void ClickTarget ( )
    {
        //Left Mouse button down
        if ( Input.GetMouseButtonDown ( 0 ) )
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay ( Input.mousePosition );

            //Hit nothing, just ignore below
            if( !Physics.Raycast (ray, out hit  ) ||EventSystem.current.IsPointerOverGameObject ( ) )     
            { return; }

            var hitObj = hit.transform.gameObject;
            // Hit the ground and Move Character
            if( hitObj.CompareTag ( "Walkable" ))                                                         
            {             
                var targetPos = new Vector3 ( hit.point.x, 0, hit.point.z );
                charaController.MoveTo ( targetPos );
                return;
            }
            // Hit on Clickable NPC
            IInteractable go = hitObj.GetComponent<IInteractable> ( );                                     
            if ( go != null && hitObj.CompareTag ( "NPC") )
            {
                go.Interact ( );                                            //open Dialogue Window
                Debug.Log ( "hit on Clickable NPC." );
            }
        }

    }
}
