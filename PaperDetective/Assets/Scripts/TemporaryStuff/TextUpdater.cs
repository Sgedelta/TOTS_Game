using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextUpdater : MonoBehaviour
{
    //this is temporary. don't put it on anything that's not a button.
    void Start()
    {
        TMP_InputField t = GetComponent<TMP_InputField>();
        GameManager.instance.FileManager.UpdatePlaytestText(t);
    }
}
