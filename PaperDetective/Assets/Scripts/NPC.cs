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

    [SerializeField] private string startNode;

    /// <summary>
    /// The range of the NPC's specific audio playback
    /// </summary>
    [SerializeField] public Vector2 NPCAudioBounds = new Vector2(1.0f, 1.5f);

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (moveTo.Count > 0 && moving)
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

    public void QueueMove(Vector2 move)
    {
        moveTo.Add(move);
    }

    public void Talk()
    {
        AudioManager.instance.PitchBounds = NPCAudioBounds;
        if (!SingletonComponent.Instances["Dialogue System Variant"].GetComponent<DialogueRunner>().IsDialogueRunning)
            SingletonComponent.Instances["Dialogue System Variant"].GetComponent<DialogueRunner>().StartDialogue(startNode);
    }

    public void Silence()
    {
        dialogueRunner.Stop();
    }


}
