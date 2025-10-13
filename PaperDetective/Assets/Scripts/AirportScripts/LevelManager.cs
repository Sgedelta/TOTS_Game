using UnityEngine;
using Yarn.Unity;

public class LevelManager : MonoBehaviour
{
    //Get the reference to the Dictionary object the Player must hold to understand people.
    //[SerializeField]
    //private Item dictionary;

    //Determine if the Player has the dictionary.
    public bool hasDict;

    //Track the amount of currency the player has.
    private float money;

    //Get the dialogue runner object in the scene.
    DialogueRunner runner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //At the start, the Player does not have the dictionary, so they cannot understand people.
        hasDict = false;

        money = 0.00f;

        runner = GameManager.instance.DialogueSystem.GetComponent<DialogueRunner>();
    }

    // Update is called once per frame
    void Update()
    {
        //If the Player picks up the dictionary, they can then understand people.
        //if(Player.inventory.contains(dictionary) && hasDict == false)
        //{
        //DictionaryGot();
        //hasDict = true;
        //}

        // -- Testing Purposes Only
        //if the player hits the space bar, then they have the dict
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DictionaryGot();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            AddMoney(3.00f);
        }

        
        //If the player has enough money to get the dictionary, make sure the Yarn files know.
        //if(Player.currency >= 4.99)
        //{
        //
    }

    /// <summary>
    /// Send a message to the Yarn scripts that the player has the dictionary.
    /// </summary>
    [YarnCommand("has_Dict")]
    public void DictionaryGot()
    {

        //If we have the dialogue runner(if we don't something is wrong), update the global "hasDict" variable to true.
        if (runner != null)
        {
            runner.VariableStorage.SetValue("$hasDict", true);
        }
    }

    [YarnCommand("AddMoney")]
    public void AddMoney(float newMoney)
    {

        //If we have the dialogue runner, increase the currency that yarn has.
        if (runner != null)
        {
            runner.VariableStorage.SetValue("$currency", money + newMoney);
            money = newMoney;
        }
    }
}
