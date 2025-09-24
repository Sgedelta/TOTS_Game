using UnityEngine;

public class Walk : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private SpriteRenderer sprite;
    /// <summary>
    /// How fast the walk animation is
    /// </summary>
    [SerializeField] private float spinSpeed;

    /// <summary>
    /// How far the player will rotate suring one "step" before rotating the other way
    /// </summary>
    [SerializeField] private float maxWalkAngle;

    private float spinDirection = 1;

    private float currentAngle;
    // Update is called once per frame
    void Update()
    {
        //If player is moving, do the rotation
        if (rb.linearVelocity.x != 0 || rb.linearVelocityY != 0)
        {
            if (Mathf.Abs(currentAngle) > maxWalkAngle)
            {
                spinDirection *= -1;
            }
            currentAngle += spinDirection * spinSpeed;
            sprite.transform.Rotate(new Vector3(0, 0, spinDirection * spinSpeed));
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
