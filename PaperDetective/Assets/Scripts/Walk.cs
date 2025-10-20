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

    [SerializeField] private bool playSound;

    [SerializeField] private AudioSource stepPlayer;
    // Update is called once per frame
    void Update()
    {
        //If player is moving, do the rotation
        if (rb.linearVelocity.x != 0 || rb.linearVelocityY != 0)
        {
            if (Mathf.Abs(currentAngle) > maxWalkAngle)
            {
                spinDirection *= -1;
                if (playSound)
                {
                    stepPlayer.Play();
                }
                
            }
            currentAngle += spinDirection * spinSpeed * Time.deltaTime * 100;
            sprite.transform.Rotate(new Vector3(0, 0, spinDirection * spinSpeed * Time.deltaTime * 100));
        }
        //Otherwise slowly return the player to the upright position
        else if (Mathf.Abs(sprite.transform.rotation.z) > spinSpeed)
        {
            if (sprite.transform.rotation.z < 0)
            {
                currentAngle += spinSpeed * Time.deltaTime * 100;
                sprite.transform.Rotate(new Vector3(0, 0, spinSpeed * Time.deltaTime * 100));
            }
            else
            {
                currentAngle -= spinSpeed * Time.deltaTime * 100;
                sprite.transform.Rotate(new Vector3(0, 0, -spinSpeed * Time.deltaTime * 100));
            }
        }

        if(transform.rotation.z != 0)
        {
            transform.rotation = Quaternion.identity;
        }
    }
}
