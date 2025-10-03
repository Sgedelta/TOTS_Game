using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using Yarn.Unity.Editor;

public class FileManager : MonoBehaviour
{
    //Get a file that we will write to
    [SerializeField]
    private TextAsset jsonFile;

    //Create an instance of the JSON data that we will record
    private UserData userData = new UserData();

    //Keep track of the time since the game started
    private float time;

    //Get the testing button
    [SerializeField]
    private Button button;

    //Get the text asset we will display information on.
    [SerializeField]
    private TMP_Text text;


    private void Start()
    {
        button.onClick.AddListener(SaveFile);
    }


    // Update is called once per frame
    void Update()
    {
        //Update the time played each frame
        time += Time.deltaTime;
    }

    private void SaveFile()
    {
        userData.playTime = $"{time}";

        //Convert our user data into JSON format.
        //Include "true" to make the format readable.
        string jsonData = JsonUtility.ToJson(userData, true);

        text.text = jsonData;
    }
}


[System.Serializable]
public class UserData
{
    public string playTime;
}
