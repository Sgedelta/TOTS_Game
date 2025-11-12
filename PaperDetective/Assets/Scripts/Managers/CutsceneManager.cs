using Unity.Cinemachine;
using UnityEngine;
using Yarn.Unity;
//Author: Carl Browning
public class CutsceneManager : MonoBehaviour
{
    PLayerController playerController;

    CinemachineCamera cinemachineCamera;

    public static CutsceneManager instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(instance == null && instance != this)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PLayerController>();
        cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();
    }

    /// <summary>
    /// Begins a cutscene by setting the camera to follow the given target and disabling player movement
    /// </summary>
    /// <param name="newTarget">The new transform the camera is meant to follow</param>
    public void StartCutscene(Transform newTarget)
    {
        cinemachineCamera.Follow = newTarget;
        playerController.CanMove = false;
        playerController.CanInteract = false;
        Inventory.instance.canInteract = false;
    }

    /// <summary>
    /// Changes the target the camera is following during a cutscene
    /// </summary>
    /// <param name="newTarget">The new transform the camera is meant to follow</param>
    public void SwapTarget(Transform newTarget)
    {
        cinemachineCamera.Follow = newTarget;
    }
    /// <summary>
    /// Ends the cutscene by setting the camera to follow the player and enabling player movement
    /// </summary>
    [YarnCommand("StopCutscene")]
    public void StopCutscene()
    {
        cinemachineCamera.Follow = playerController.gameObject.transform;
        playerController.CanMove = true;
        playerController.CanInteract = true;
        Inventory.instance.canInteract = true;
    }
}
