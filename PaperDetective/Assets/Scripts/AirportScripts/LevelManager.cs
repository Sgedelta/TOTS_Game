using UnityEngine;
using Yarn.Unity;

public class LevelManager : MonoBehaviour
{
    //Get the reference to the Dictionary object the Player must hold to understand people.
    //[SerializeField]
    //private Item dictionary;

    //Determine if the Player has the dictionary.
    public bool hasDict;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //At the start, the Player does not have the dictionary, so they cannot understand people.
        hasDict = false;
    }

    // Update is called once per frame
    void Update()
    {
        //If the Player picks up the dictionary, they can then understand people.
        //if(Player.inventory.contains(dictionary))
        //{
        //hasDict = true;
        //}

        // -- Testing Purposes Only
        //if the player hits the space bar, then they have the dict
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DictionaryGot();
        }
    }

    [YarnCommand("has_Dict")]
    private void DictionaryGot()
    {
        DialogueRunner runner = FindObjectOfType<Yarn.Unity.DialogueRunner>();

        if(runner != null)
        {
            runner.VariableStorage.SetValue("$hasDict", true);
        }
    }
}
