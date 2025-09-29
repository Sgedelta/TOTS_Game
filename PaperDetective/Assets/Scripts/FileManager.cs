using System;
using UnityEngine;
using Yarn.Unity.Editor;

public class FileManager : MonoBehaviour
{
    //Get a file that we will write to
    [SerializeField]
    private TextAsset jsonFile;

    private UserData userData = new UserData();

    //Keep track of the time since the game started
    private float time;


    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
    }

    private void OnApplicationQuit()
    {
        userData.playTime = $"{time}";

        string jsonData = JsonUtility.ToJson(userData, true);

        //string path = "Assets/InternalLogs/playtest_logs_1.json";

        Debug.Log(jsonData);

        //System.IO.File.WriteAllText(path, jsonData);
    }
}


[System.Serializable]
public class UserData
{
    public string playTime;
}
