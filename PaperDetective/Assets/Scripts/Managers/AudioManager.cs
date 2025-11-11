using UnityEngine;
using Yarn;
using Yarn.Unity;
using UnityEngine.Events;
using UnityEngine.Audio;
using NUnit.Framework;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private DialogueRunner dialogueRunner;

    [SerializeField] private List<AudioSource> audioSource;
    [SerializeField] private AudioSource speechPlayer;



    Vector2 tempPitchBounds = new Vector2(1.0f, 1.5f);

    private Vector2 pitchBounds;
    public Vector2 PitchBounds
    {
        get { return pitchBounds; }
        set { pitchBounds = value; }
    }

    public static AudioManager instance;

    private void OnEnable()
    {
        CustomTypewriter.CharacterTyped += SpeechCharacterTyped;
    }
    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }
    void Start()
    {
        dialogueRunner = GameManager.instance.DialogueSystem.GetComponent<DialogueRunner>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(dialogueRunner.IsDialogueRunning)
        //{
        //    if(!speechPlayer.isPlaying)
        //        SpeechStart();
        //}
        //else
        //{
        //    SpeechStop();
        //}
    }

    //public void SpeechStart()
    //{
    //    speechPlayer.time = 1;
    //    speechPlayer.Play();
    //}

    //public void SpeechStop()
    //{  
    //    speechPlayer.Stop();
    //}

    public void SpeechCharacterTyped()
    {
        speechPlayer.pitch = Random.Range(tempPitchBounds.x, tempPitchBounds.y);
        speechPlayer.Play();
    }

    public void PlaySound(string name)
    {
        foreach (AudioSource source in audioSource)
        {
            if (source.clip.name == name)
            {
                source.Play();
            }
        }
    }
}
