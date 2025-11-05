using System.Collections.Generic;
using UnityEngine;

public class PositionSetter : MonoBehaviour
{
    private string state;
    public string State
    {
        get { return state; }
        set 
        { 
            state = value; 
        }
    }

    [SerializeField] private List<string> states;
    [SerializeField] private List<Vector2> positions;

    public Dictionary<string, Vector2> StatePositions = new Dictionary<string, Vector2>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
