using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //We will utilize a Singleton for our GameManager, so only one instance will exist across the entire game.
    //This is why we label this instance "static", as this one variable is shared across all possible GameManager scripts.
    public static GameManager instance;


    /* -------------- TO DO ----------------- */

    ////Store all the individual LevelManagers in a dictionary so we can call upon them.
    //private Dictionary<Scene, LevelManager> levelManagers;

    ////We will keep an array of the player's inventory to track across scenes.
    //private Item[] inventory;

    /* -------------------------------------- */


    //Track the current scene.
    private Scene currentScene;

    //Keep track of the player's objective across scenes.
    private string currentObjective;




    /// <summary>
    /// This determines the singular instance of GameManager that will exist across all scenes.
    /// </summary>
    private void Awake()
    {
        //Check if there is no instance of this GameManager that exists.
        if(instance == null)
        {
            //Set the gameObject this script is attached to as the single instance of the GameManager.
            instance = this;

            //This method informs Unity to retain the object this script is attached to when changing scenes.
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            //If there is already an instance of the GameManager, then delete the object this is attached to.
            //This ensures that only one instance of the GameManager exists across all scenes.
            Destroy(this.gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Ensure we keep track of the current active scene.
        currentScene = SceneManager.GetActiveScene();

    }



    void AddToInventory()
    {

    }

    void RemoveFromInventory()
    {

    }

    /// <summary>
    /// Updates the current objective.
    /// </summary>
    /// <param name="newObjective">The message that will be displayed on-screen as the "new" current objective</param>
    void updateObjective(string newObjective)
    {
        //Assign the newObjective to be the currentObjective.
        currentObjective = newObjective;
    }
}
