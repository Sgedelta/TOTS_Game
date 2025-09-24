using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float speed;
    /// <summary>
    /// The (x,y) coords that the NPC will move to starting at the 0th index
    /// and ending at the nth index.
    /// </summary>
    [SerializeField] private List<Vector2> moveTo;
    

    // Update is called once per frame
    void Update()
    {
        if(moveTo.Count > 0)
        {
            rb.linearVelocity = (moveTo[0] - rb.position).normalized * speed;

            if ((moveTo[0] - rb.position).magnitude < 1)
            {
                moveTo.RemoveAt(0);
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

        
    }

    
}
