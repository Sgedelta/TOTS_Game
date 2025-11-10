using System;
using UnityEngine;
using UnityEngine.Events;

//Author: Sam Easton

public class PersistentDataChecker : MonoBehaviour
{
    /// <summary>
    /// A unique identifier for this object
    /// </summary>
    [SerializeField] private string objectID = "";
    public string ObjectID { get { return objectID; } }

    /// <summary>
    /// the data holder for this in game object
    /// </summary>
    [SerializeField] private PersistentData data;

    /// <summary>
    /// The PersistentData data holder for this in game GameObject
    /// </summary>
    public PersistentData Data { get { return data; } }

    /// <summary>
    /// A public event for loading from a given state
    /// </summary>
    public UnityEvent<string> LoadState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //this is not set up correctly
        if(objectID == null || objectID == "")
        {
            Debug.LogError($"Game Object \"{gameObject.name}\" has a PersistentDataChecker but Object ID is not set correctly. Destroying the Persistent Data.");
            Destroy(this);
            return;
        }

        //this is set up, but the persistent data does not already exist - we need to make sure we hook it up before going forward
        if(!GameManager.instance.AllPersistentData.ContainsKey(objectID))
        {
            GameManager.instance.AllPersistentData.Add(objectID, data);
        } 
        else
        {
            //otherwise, use persistent data in loader
            data = GameManager.instance.AllPersistentData[objectID];
        }

        //finally, load data
        LoadFromPersistentData(data);

        
    }

    /// <summary>
    /// A method that sets our current state to the proper 
    /// </summary>
    /// <param name="data"></param>
    public void LoadFromPersistentData(PersistentData data)
    {

        //check if we need to destroy this object
        if(!data.KeepOnLoad)
        {
            Destroy(gameObject);
            return;
        }

        //invoke the event with the current state - this is set up in editor and could go to many different things - if the state is updated
        if(data.CurrentObjectState != null && data.CurrentObjectState != "")
        {
            LoadState.Invoke(data.CurrentObjectState);
        }


    }

}

[Serializable]
/// <summary>
/// The object in memory that holds persistent data between scenes
/// </summary>
public class PersistentData
{
    /// <summary>
    /// If this game object should exist once the scene is "loaded" (our loading, not unity)
    /// if False, the Game Object is destroyed when it's PersistentDataChecker Start is called
    /// </summary>
    public bool KeepOnLoad = true;

    /// <summary>
    /// The persistent object state, other than if it's created/destroyed. Used for things like sprite states. 
    /// UnityEvent<string> invoked with this as an argument.
    /// </summary>
    public string CurrentObjectState = "";
    //  Note from Sam: this (ABOVE) could just be an easy state - using vending example "unused", "bookStuck", and "empty". 
    //      Now this is likely enough for many states, and probably enough for our use, so I am not writing something more complex atm
    //      but in theory we can also use this as a more complex, json-like object with multiple pieces of data within in. I don't honestly think
    //      we *need* much more data than this. I suggest using vertical bar ( | ) for data separation. This would be for something like player, where
    //      we would be storing data like things equipped, where that player is coming from, etc. The only issue is that this can get a little nasty in 
    //      only updating one thing (i.e. only updating equipment part). not hard, just some string manipulation, but could be annoying to do. A list of
    //      strings is probably easier if there's a lot of things that need that? but I don't think we need that for our scope and it's more work than
    //      it's worth, for now. thus: single string!

}
