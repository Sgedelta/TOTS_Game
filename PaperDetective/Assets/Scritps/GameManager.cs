using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Runtime.CompilerServices;

/// <summary>
/// The state the game is in at any given time
/// </summary>
public enum GameStates
{
    MainMenu,
    Gameplay,
    Pause,
    Options
}
public class GameManager : MonoBehaviour
{
    //We will utilize a Singleton for our GameManager, so only one instance will exist across the entire game.
    //This is why we label this instance "static", as this one variable is shared across all possible GameManager scripts.
    public static GameManager instance;


    /* -------------- TO DO ----------------- */

    ////Store all the individual LevelManagers in a dictionary so we can call upon them.
    //private Dictionary<Scene, LevelManager> levelManagers;

    ////We will keep track of the inventory, which is kept in a separate InventoryManager
    //private InventoryManager inventory;

    /* -------------------------------------- */


    //Track the current scene.
    private Scene currentScene;

    //Keep track of the player's objective across scenes.
    private string currentObjective;

    //Determine what state the player is in at all times.
    private GameStates currentState;




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

            //Start the game in the gameplay mode
            // -- THIS CAN BE CHANGED IF NEEDED --
            currentState = GameStates.Gameplay;
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
        //Get the scene that we start off with.
        currentScene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
        //If the current scene does not match the currentActiveScene, update all data.
        if(currentScene != SceneManager.GetActiveScene())
        {
            //Call the method to change the level.
            ChangeLevel();
        }

        checkGameState();
    }

    /// <summary>
    /// Check the state of the game and activate the associated behaviors.
    /// </summary>
    private void checkGameState()
    {
        switch (currentState)
        {
            //If we are in the Main Menu state, [ do the Main Menu behaviors ]
            case GameStates.MainMenu:

                break;
            //If the game is in the main gameplay, [ allow player control and interaction ]
            case GameStates.Gameplay:

                break;
            //If the pause button is clicked, freeze the world and open the pause menu.
            case GameStates.Pause:

                break;
            //If the options are opened up, allow the players to change the game options.
            case GameStates.Options:

                break;
        }
    }

    /// <summary>
    /// Update the current level and carry over key information across scenes.
    /// </summary>
    void ChangeLevel()
    {
        //Update the current active scene.
        currentScene = SceneManager.GetActiveScene();

        ////Get the player object in this scene and update them with the current inventory.
        //playerObject = Find the current player gameobject
        //playerObject.inventory = inventory;

        ////Update the LevelManager to show the current objective.
        //levelManagers<currentScene>.objective = currentObjective;
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
