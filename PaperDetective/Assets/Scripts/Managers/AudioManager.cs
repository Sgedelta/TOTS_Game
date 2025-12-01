using UnityEngine;
using Yarn;
using Yarn.Unity;
using UnityEngine.Events;
using UnityEngine.Audio;
using NUnit.Framework;
using System.Collections.Generic;
using Yarn.Unity.Legacy;
using TMPro;

public class AudioManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private DialoguePresenter dialoguePresenter;

    [SerializeField] private List<AudioSource> audioSource;
    [SerializeField] private AudioSource speechPlayer;

    [SerializeField] private Vector2 narratorPitch = new Vector2(1.0f, 1.5f);
    [SerializeField] private Vector2 npcPitch = new Vector2(1.0f, 1.5f);
    private TMP_Text nameText;
    public TMP_Text NameText
    {
        get { return nameText; }
        set { nameText = value; }
    }

    private Vector2 pitchBounds;
    public Vector2 PitchBounds
    {
        get { return pitchBounds; }
        set { pitchBounds = value; }
    }
    public Vector2 NpcPitch
    {
        get { return npcPitch; }
        set { npcPitch = value; }
    }
    public static AudioManager instance;

    private void OnEnable()
    {
        CustomTypewriter.CharacterTyped += SpeechCharacterTyped;
    }
    void Awake()
    {
        if (instance == null && instance != this)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        npcPitch = new Vector2(1.0f, 1.5f);
    }
    void Start()
    {
        
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
        if(speechPlayer == null)
        {
            Debug.LogError("speechPlayer is Null!");
            return;
        }


        speechPlayer.pitch = Random.Range(pitchBounds.x, pitchBounds.y);
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

    public void SetPitchNarrator()
    {
        pitchBounds = narratorPitch;
    }

    public void SetPitchNPC(Vector2 input)
    {
        npcPitch = input;
        pitchBounds = npcPitch;
    }
}
