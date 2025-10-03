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



    //Get the GameManager to check endings
    public bool ending;

    public string[] npcs;


    private void Start()
    {


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
        userData.ending = ending;
        userData.npcList = npcs;

        //Convert our user data into JSON format.
        //Include "true" to make the format readable.
        string jsonData = JsonUtility.ToJson(userData, true);

        text.text = jsonData;
#if UNITY_EDITOR
        UnityEditor.EditorGUIUtility.systemCopyBuffer = jsonData;
#endif
    }

    public void SetPlaytestButton(Button newButton)
    {
        button = newButton;
        button.onClick.AddListener(SaveFile);
    }

    public void UpdatePlaytestText(TMP_Text text)
    {
        this.text = text;
    }
}



[System.Serializable]
public class UserData
{
    public string playTime;
    public bool ending;
    public string[] npcList;
}
