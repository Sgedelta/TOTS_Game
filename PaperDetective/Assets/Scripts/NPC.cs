using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class NPC : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float speed;
    /// <summary>
    /// The (x,y) coords that the NPC will move to starting at the 0th index
    /// and ending at the nth index.
    /// </summary>
    [SerializeField] private List<Vector2> moveTo;

    /// <summary>
    /// Whether or not the NPC should go to its next move location.
    /// </summary>
    [SerializeField] private bool moving;

    /// <summary>
    /// How close the NPC has to move to the given moveTo location before it goes to the next one
    /// </summary>
    [SerializeField] private float closeEnoughRadius;

    private DialogueRunner dialogueRunner;

    /// <summary>
    /// Where the NPC's dialogue will start when interacted with
    /// </summary>
    [SerializeField] private string startNode;

    [SerializeField] private Vector2 bitRange;

    // Update is called once per frame
    void Update()
    {
        if(moveTo.Count > 0 && moving)
        {
            rb.linearVelocity = (moveTo[0] - rb.position).normalized * speed;

            if ((moveTo[0] - rb.position).magnitude < closeEnoughRadius)
            {
                moveTo.RemoveAt(0);
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="move"></param>
    public void QueueMove(Vector2 move)
    {
        moveTo.Add(move);
    }

    public void Talk()
    {
        if (!SingletonComponent.Instances["Dialogue System Variant"].GetComponent<DialogueRunner>().IsDialogueRunning)
            SingletonComponent.Instances["Dialogue System Variant"].GetComponent<DialogueRunner>().StartDialogue(startNode);
    }

    public void Silence()
    {
        dialogueRunner.Stop();
    }


}
