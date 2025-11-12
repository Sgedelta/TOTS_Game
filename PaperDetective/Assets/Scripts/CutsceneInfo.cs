using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
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

    public void ChangeSpeaker()
    {
        CutsceneManager.instance.SwapTarget(speakers[0]);
        speakers.RemoveAt(0);
    }

}
