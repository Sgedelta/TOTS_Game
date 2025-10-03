using UnityEngine;
using Yarn.Unity;

public class AudioManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private DialogueRunner dialogueRunner;

    [SerializeField] private AudioSource speechPlayer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(dialogueRunner.IsDialogueRunning)
        {
            if(!speechPlayer.isPlaying)
                SpeechStart();
        }
        else
        {
            SpeechStop();
        }
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
}
