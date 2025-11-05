using System.Collections.Generic;
using UnityEngine;

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
            UpdatePosition();
            if(PersistentDataID != null && PersistentDataID != "" && GameManager.instance.AllPersistentData.ContainsKey(PersistentDataID))
            {
                GameManager.instance.AllPersistentData[PersistentDataID].CurrentObjectState = value;
            }
        }
    }

    [SerializeField] private List<string> states;
    [SerializeField] private List<Vector2> positions;

    private Dictionary<string, Vector2> statePositions = new Dictionary<string, Vector2>();

    [SerializeField] public string PersistentDataID;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //load all states into dictionary
        for (int i = 0; i < states.Count; i++)
        {
            statePositions.Add(states[i], positions[i]);
        }
        //and default
        statePositions.Add(DEFAULT_POS_KEY, gameObject.transform.position);

    }

    public void UpdatePosition()
    {
        if (statePositions.ContainsKey(state))
        {
            gameObject.transform.position = statePositions[state];
        }
        else
        {
            gameObject.transform.position = statePositions[DEFAULT_POS_KEY];
        }
    }

    public void UpdateState(string newState)
    {
        State = newState;
    }

}
