using UnityEngine;
using System;

public class Walk : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private bool playSound;

    [SerializeField] private AudioSource stepPlayer;

    [SerializeField] private Animator anim;

    // Update is called once per frame
    void FixedUpdate()
    {
        //If player is moving, do the rotation
        Console.WriteLine("Velocity: " + rb.linearVelocity.magnitude);
        anim.SetFloat("Velocity", rb.linearVelocity.magnitude);
        anim.SetFloat("HorizontalVelocity", rb.linearVelocity.x);
    }
}
