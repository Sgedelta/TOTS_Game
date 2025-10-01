/** Carl Browning
 *  Summary: This script handles all input from the player as it relates to movement
 */
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PLayerController : MonoBehaviour
{
    /// <summary>
    /// How fast the player moves
    /// </summary>
    [SerializeField] private float speed;

    [SerializeField] private bool canMove = true;

    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private GameObject sprite;

    private float horizontal;
    private float vertical;

    
    private float spinDirection = 1;
    private float currentAngle = 0;

    /// <summary>
    /// How fast the walk animation is
    /// </summary>
    [SerializeField] private float spinSpeed;

    /// <summary>
    /// How far the player will rotate suring one "step" before rotating the other way
    /// </summary>
    [SerializeField] private float maxWalkAngle;

    /// <summary>
    /// How close the player has to be to an NPC to talk to them
    /// </summary>
    [SerializeField] private float talkRadius;

    /// <summary>
    /// The NPC you are currently talking to.
    /// </summary>
    [SerializeField] private NPC talkPartner;

    // Update is called once per frame
    void Update()
    {
        if(canMove)
            rb.linearVelocity = new Vector2(horizontal * speed, vertical * speed);

        //If you walk away from the npc, you stop talking
        if(talkPartner != null && (talkPartner.gameObject.transform.position - transform.position).magnitude > talkRadius){
            talkPartner.Silence();
            talkPartner = null;
        }
    }

    /// <summary>
    /// Whenever the player presses a WASD key, this method updates the direction they move accordingly
    /// </summary>
    /// <param name="context">The key pressed event invoked by the Player Input system</param>
    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
        vertical = context.ReadValue<Vector2>().y;
    }


    public void Interact(InputAction.CallbackContext context)
    {
        List<Collider2D> colliders = new List<Collider2D>();
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.layerMask = LayerMask.GetMask("Talk");
        contactFilter.useLayerMask = true;

        //Get all the things you can talk to within talkRadius
        Physics2D.OverlapCircle(transform.position, talkRadius, contactFilter, colliders);

        if (colliders.Count == 0)
            return;
        //Find the closest NPC
        Collider2D closest = colliders[0];
        float closestDistance = Mathf.Abs((transform.position - colliders[0].transform.position).magnitude);
        for (int i = 1; i < colliders.Count; i++)
        {
            if (Mathf.Abs((transform.position - colliders[i].transform.position).magnitude) < closestDistance)
            {
                closest = colliders[i];
                closestDistance = Mathf.Abs((transform.position - colliders[i].transform.position).magnitude);
            }
        }

        //Talk to said closest NPC
        if (closest != null)
        {
            talkPartner = closest.gameObject.GetComponent<NPC>();
            talkPartner.Talk();
        }
    }
    public void WalkAnimation()
    {
        //If player is moving, do the rotation
        if (rb.linearVelocity.x != 0 || rb.linearVelocityY != 0)
        {
           if (Mathf.Abs(currentAngle) > maxWalkAngle)
            {
                spinDirection *= -1;
            }
            currentAngle += spinDirection * spinSpeed;
            sprite.transform.Rotate( new Vector3(0,0,spinDirection * spinSpeed));
        }
        //Otherwise slowly return the player to the upright position
        else if (Mathf.Abs(sprite.transform.rotation.z) > spinSpeed)
        {
            if (sprite.transform.rotation.z < 0)
            {
                currentAngle += spinSpeed;
                sprite.transform.Rotate(new Vector3(0, 0, spinSpeed));
            }
            else
            {
                currentAngle -= spinSpeed;
                sprite.transform.Rotate(new Vector3(0, 0, -spinSpeed));
            }
        }
        //This was added to prevent a bug where a non-moving player kept twitching between 1 spinSpeed unit of rotation and 0 rotation
        else
        {
            sprite.transform.rotation = new Quaternion(0, 0, 0, 0);
            currentAngle = 0;
        }
    }
}
