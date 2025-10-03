using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextUpdater : MonoBehaviour
{
    //this is temporary. don't put it on anything that's not a button.
    void Start()
    {
        TMP_Text t = GetComponent<TMP_Text>();
        GameManager.instance.FileManager.UpdatePlaytestText(t);
    }
}
