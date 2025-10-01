using System;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity.Editor;

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

        //string path = "Assets/InternalLogs/playtest_logs_1.json";

        Debug.Log(jsonData);

        Debug.Log(Application.persistentDataPath);

        //System.IO.File.WriteAllText(path, jsonData);
    }
}


[System.Serializable]
public class UserData
{
    public string playTime;
}
