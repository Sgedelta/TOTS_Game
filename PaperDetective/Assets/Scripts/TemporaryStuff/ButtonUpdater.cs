using UnityEngine;
using UnityEngine.UI;

public class ButtonUpdater : MonoBehaviour
{
    //this is temporary. don't put it on anything that's not a button.
    void Start()
    {
        Button b = GetComponent<Button>();
        GameManager.instance.FileManager.SetPlaytestButton(b);
    }

}
