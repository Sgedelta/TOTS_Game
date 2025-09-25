/** Carl Browning
 *  Summary: This script handles all input from the player as it relates to movement
 */
using UnityEngine;
using UnityEngine.InputSystem;
public class PLayerController : MonoBehaviour
{
    /// <summary>
    /// How fast the player moves
    /// </summary>
    [SerializeField] private float speed;

    [SerializeField] private Rigidbody2D rb;

    private float horizontal;
    private float vertical;

    private float spinDirection = 1;

    /// <summary>
    /// How fast the walk animation is
    /// </summary>
    [SerializeField] private float spinSpeed;

    /// <summary>
    /// How far the player will rotate suring one "step" before rotating the other way
    /// </summary>
    [SerializeField] private float maxWalkAngle;

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new Vector2(horizontal * speed, vertical * speed);

        WalkAnimation();
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

    public void WalkAnimation()
    {
        //If player is moving, do the rotation
        if (rb.linearVelocity.x != 0 || rb.linearVelocityY != 0)
        {
            if (Mathf.Abs(rb.rotation) > maxWalkAngle)
                spinDirection *= -1;
            rb.rotation += spinDirection * spinSpeed;
        }
        //Otherwise slowly return the player to the upright position
        else if (Mathf.Abs(rb.rotation) > spinSpeed)
        {
            if(rb.rotation < 0)
                rb.rotation += spinSpeed;
            else
                rb.rotation -= spinSpeed;
        }
        //This was added to prevent a bug where a non-moving player kept twitching between 1 spinSpeed unit of rotation and 0 rotation
        else
        {
            rb.rotation = 0;
        }
    }
}
