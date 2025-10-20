using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.Audio;

public class Dia_Log : MonoBehaviour
{
    private HashSet<string> loggedLines = new HashSet<string>();
    public List<string> DialogueLog = new List<string>();
    private string lastLine = "";

    [SerializeField] LinePresenter linePresenter;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (linePresenter == null) linePresenter = this.GetComponent<LinePresenter>();
    }

    // Update is called once per frame
    void Update()
    {
        //get the current line
        string currentLine = linePresenter.lineText.text;

        //make sure currentLine is not null or nothing and it's not the previous line
        if (!string.IsNullOrEmpty(currentLine) && currentLine != lastLine)
        {
            //adding to a hashset is O(1), and add will return true if it successfully adds to the set
            if (loggedLines.Add(currentLine))
            {
                DialogueLog.Add(currentLine);
            }

            lastLine = currentLine;
        }
    }
}
