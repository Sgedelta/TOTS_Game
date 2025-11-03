using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlaySounds : MonoBehaviour
{
    private AudioManager audioManager;

    [SerializeField] private List<AudioSource> audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioManager = AudioManager.instance;

    }

    public void PlaySound(string name)
    {
        audioManager.PlaySound(name);
    }
}