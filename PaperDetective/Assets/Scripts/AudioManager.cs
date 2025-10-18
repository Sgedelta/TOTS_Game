using UnityEngine;
using Yarn;
using Yarn.Unity;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private DialogueRunner dialogueRunner;

    [SerializeField] private AudioSource speechPlayer;

    Vector2 tempPitchBounds = new Vector2 (4.0f, 5.0f);

    private void OnEnable()
    {
        BasicTypewriter.CharacterTyped += SpeechCharacterTyped;
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

    public void SpeechStart()
    {
        speechPlayer.time = 1;
        speechPlayer.Play();
    }

    public void SpeechStop()
    {  
        speechPlayer.Stop();
    }

    public void SpeechCharacterTyped()
    {
        speechPlayer.pitch = Random.Range(tempPitchBounds.x, tempPitchBounds.y);
        speechPlayer.Play();
    }
}
