/** Carl Browning
 *  Summary: This script handles all input from the player as it relates to movement
 */
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn;
using Yarn.Unity;
public class PLayerController : MonoBehaviour
{
    /// <summary>
    /// How fast the player moves
    /// </summary>
    [SerializeField] private float speed;

    [SerializeField] private bool canMove = true;
    public bool CanMove { 
        get { return canMove; } 
        set { canMove = value; }
    }

    [SerializeField] private bool canInteract = true;
    public bool CanInteract
    {
        get { return canInteract; }
        set { canInteract = value; }
    }

    [SerializeField] private Rigidbody2D rb;
    private float horizontal;
    private float vertical;

    /// <summary>
    /// The NPC you are currently talking to.
    /// </summary>
    [SerializeField] private NPC talkPartner;


    private void Start()
    {
        GameManager.instance.DialogueSystem.GetComponent<DialogueRunner>().onDialogueComplete.AddListener(EndDialogue);

        GetComponent<PlayerInput>().currentActionMap.FindAction("Attack", true).performed += Inventory.instance.OnClick;
        GetComponent<PlayerInput>().currentActionMap.FindAction("Attack", true).canceled += Inventory.instance.OnClick;

        Debug.Log("Rebound Attack");
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            rb.linearVelocity = new Vector2(horizontal * speed, vertical * speed);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }    
    
    }

   

    /// <summary>
    /// Whenever the player presses a WASD key, this method updates the direction they move accordingly
    /// </summary>
    /// <param id="context">The key pressed event invoked by the Player Input system</param>
    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
        vertical = context.ReadValue<Vector2>().y;
    }


    public void Interact(InputAction.CallbackContext context)
    {
        if(!canInteract)
            return;
        List<Collider2D> colliders = new List<Collider2D>();

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(mousePosition, new Vector2(0, 1), 0.01f, LayerMask.GetMask("Talk"));

        if (!hit)
        {
            hit = Physics2D.Raycast(mousePosition, new Vector2(0, 1), 0.01f, LayerMask.GetMask("Door"));
            Debug.Log(hit.collider);

            if (hit)
            {
                hit.collider.gameObject.GetComponent<TriggerSceneChange>().GoToScene(this);
            }
        }
        else
        {
            talkPartner = hit.collider.gameObject.GetComponent<NPC>();
            talkPartner.Talk();

            canMove = false;
        }

    }

    public void EndDialogue()
    {
        canMove = true;
        talkPartner = null;
    }
}
