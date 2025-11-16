using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PositionSetter : MonoBehaviour
{
    private const string DEFAULT_POS_KEY = "DEFAULT_POSITION";

    private string state;

    public string State
    {
        get { return state; }
        set 
        { 
            state = value;
            if(PersistentDataID != null && PersistentDataID != "" && GameManager.instance.AllPersistentData.ContainsKey(PersistentDataID))
            {
                GameManager.instance.AllPersistentData[PersistentDataID].CurrentObjectState = value;
            }
            UpdatePosition();
        }
    }

    [SerializeField] public string PersistentDataID;

    [SerializeField] private bool setPosOnStart = true;

    private NamedLocation[] namedLocations;


    private void Awake()
    {
        if(PersistentDataID == null || PersistentDataID == "")
        {
            Debug.LogError($"PersistentDataID on ${gameObject.name} is not set properly! Destroying Position Setter to prevent further errors!");
            Destroy(this);
            return;
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (setPosOnStart && GameManager.instance.AllPersistentData.ContainsKey(PersistentDataID))
        {
            UpdatePosition();
        }
    }


    private void UpdatePosition()
    {
        //get the newest state
        state = GameManager.instance.AllPersistentData[PersistentDataID].CurrentObjectState;

        //if this is true, this method gets significantly slower...
        if (namedLocations == null || namedLocations.Length == 0)
        {
            UpdateNamedLocations();
        }


        //if we don't find anything, default to the location the player is placed in the scene - this will happen if the entrance isn't hooked up correctly or we run the game from this scene
        Vector3 pos = transform.position;

        //if we don't get a state, stop!
        if (state == "")
        {
            return;
        }

        //most other solutions using persistent data can and should use dicts, but they normally store all data internally - we don't, so looping over arrays it is...
        for (int i = 0; i < namedLocations.Length; i++)
        {
            //check if our state is the name
            if (namedLocations[i].LocationName == state)
            {
                pos = namedLocations[i].pos;
            }
        }

        transform.position = pos;


    }

    public void UpdateNamedLocations()
    {
        //warning: this is rather intensive in scenes with a lot of objects, so don't call this every frame!!
        namedLocations = FindObjectsByType<NamedLocation>(FindObjectsSortMode.InstanceID);
    }


    public void ResetNamedLocations()
    {
        //causes an UpdateNamedLocations to be run next time Position is Set, so be careful with this.
        namedLocations = null;
    }

    /// <summary>
    /// Updates State Property with var. used for unity events. i.e. in PersistentDataCheckers
    /// </summary>
    /// <param name="newState"></param>
    [YarnCommand("UpdatePosition")]
    public void UpdateState(string newState)
    {
        State = newState;
    }


}
