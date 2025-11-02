using System.Collections.Generic;
using UnityEngine;

//Author: Sam Easton

/// <summary>
/// A class that is put on an object with a SpriteRenderer. Has a internal state, as well as a dictionary of states and sprites. 
/// Can update state and sprite associated. When state is updated, if state is not valid, will push warning and go to sprite the GameObject has in editor
/// </summary>
public class SpriteUpdater : MonoBehaviour
{
    private const string DEFAULT_SPRITE_KEY = "DEFAULT_SPRITE";


    private string state;
    public string State
    {
        get { return state; }
        set
        {
            state = value;
            UpdateSprite();
            if(PersistentDataID != null && PersistentDataID != "" && GameManager.instance.AllPersistentData.ContainsKey(PersistentDataID))
            {
                GameManager.instance.AllPersistentData[PersistentDataID].CurrentObjectState = value;
            }
        }
    }

    [SerializeField] private List<string> states;
    [SerializeField] private List<Sprite> sprites;


    public Dictionary<string, Sprite> StateSprites = new Dictionary<string, Sprite>();

    /// <summary>
    /// The sprite renderer for this to update. only needed to set if this is not on an object with a Sprite Renderer
    /// </summary>
    [SerializeField] private SpriteRenderer sr;

    [SerializeField] public string PersistentDataID;

    void Start()
    {
        //set sr if it is not already set.
        if (sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
        }
        //check if we found it/it was set
        if (sr == null)
        {
            Debug.LogError($"Sprite Updater on {gameObject.name} cannot find SpriteRenderer component. Is it on an object with a Sprite Renderer or have sr set? Destroying SpriteManager.");
            Destroy(this);
            return;
        }

        //load all things into dictionary
        for (int i = 0; i < states.Count; i++)
        {
            if (sprites[i] == null)
            {
                Debug.LogWarning($"Sprite Updater on {gameObject.name} has state \"{states[i]}\", but associated sprite is null. skipping!");
                continue;
            }
            StateSprites.Add(states[i], sprites[i]);
        }
        for (int i = states.Count; i <  sprites.Count; i++)
        {
            Debug.LogWarning($"Sprite Updater on {gameObject.name} has sprite named {sprites[i].name}, but does not have associated state. adding state with sprite name instead.");
            StateSprites.Add(sprites[i].name, sprites[i]);
        }

        //add default as well
        StateSprites.Add(DEFAULT_SPRITE_KEY, sr.sprite);
    }

    /// <summary>
    /// Update the sprite to be in line with the current state. use public State Property to update both simulatenously.
    /// </summary>
    public void UpdateSprite()
    {
        if (StateSprites.ContainsKey(state))
        {
            sr.sprite = StateSprites[state];
        }
        else
        {
            sr.sprite = StateSprites[DEFAULT_SPRITE_KEY];
        }
    }

    /// <summary>
    /// Updates State Property with var. used for unity events. i.e. in PersistentDataCheckers
    /// </summary>
    /// <param name="newState"></param>
    public void UpdateState(string newState)
    {
        State = newState;
    }
}
