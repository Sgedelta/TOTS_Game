using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

/// <summary>
/// Author: Carl Browning
/// Purpose: This class holds all the info related to a cutscene and the methods Yarn needs to advance it
/// </summary>

public class CutsceneInfo : MonoBehaviour
{
    /// <summary>
    /// This is a list of all the things the camera pans to in the cutscene, in the order they're panned to.
    /// If an object is panned to multiple times, it should appear multiple times in this list.
    /// </summary>
    public List<Transform> speakers;

    public List<float> cameraZooms;

    bool cutsceneStarted = false;

    [YarnCommand("changeZoom")]
    public void ChangeZoom()
    {
        if(cameraZooms.Count == 0)
        {
            Debug.LogError("Attempted to change camera zoom but no zooms were left in the list");
            return;
        }
            
        CutsceneManager.instance.ChangeZoom(cameraZooms[0]);
        speakers.RemoveAt(0);
    }

    [YarnCommand("changeSpeaker")]
    public void ChangeSpeaker()
    {
        
        if (speakers.Count == 0)
        {
            Debug.LogError("Attempted to change speaker but no speakers were left in the list");
            return;
        }
        if (cutsceneStarted)
        {
            CutsceneManager.instance.SwapTarget(speakers[0]);
        }
        else
        {
            CutsceneManager.instance.StartCutscene(speakers[0]);
            cutsceneStarted = true;
        }

            speakers.RemoveAt(0);
    }

}
