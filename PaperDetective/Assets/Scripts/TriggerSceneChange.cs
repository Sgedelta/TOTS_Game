using UnityEngine;
using Yarn.Unity;

public class TriggerSceneChange : MonoBehaviour
{
    [SerializeField] private string sceneName;
    
    //these don't *have* to be unique, but they need to be unique to a scene (the one you are entering into)
    [SerializeField] private string entranceName;

    [SerializeField] private bool clickEnabled = false;
    [SerializeField] private bool enterEnabled = true;

    private sceneManager sceneManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sceneManager = GameObject.FindFirstObjectByType<sceneManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PLayerController pc = collision.gameObject.GetComponent<PLayerController>();
        if (enterEnabled && pc)
        {
            GoToScene(pc, false);
        }
    }

    
    public void GoToScene(PLayerController pc, bool fromClick = true)
    {
        if(fromClick && !clickEnabled)
        {
            return;
        }


        //get the player's persistent data - will need an idField and more logic to do any item, for now only setting player locations
        //  if this isn't working, check the Persistent Data Checker ID on the player is set correctly
        if(pc != null)
        {
            GameManager.instance.AllPersistentData[pc.gameObject.GetComponent<PersistentDataChecker>().ObjectID].CurrentObjectState =
                entranceName; // note: Update if player object state ever gets more complex...
        }


        //load the next scene
        sceneManager.ChangeScene(sceneName);
    }

    [YarnCommand("TriggerSceneChange")]
    public void YarnSceneChange(PLayerController pc = null)
    {
        GoToScene(pc);
    }
}
