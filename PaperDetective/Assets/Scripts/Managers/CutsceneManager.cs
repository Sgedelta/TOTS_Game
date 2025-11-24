using Unity.Cinemachine;
using UnityEngine;
using Yarn.Unity;
//Author: Carl Browning
public class CutsceneManager : MonoBehaviour
{
    private PLayerController playerController;

    private CinemachineCamera cinemachineCamera;

    private float tiltPercentage;

    private float tiltTime;

    private float tiltAmount;

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

    private void FixedUpdate()
    {
        if(Mathf.Abs(cinemachineCamera.Lens.Dutch - tiltAmount) > .5)
        {
            tiltPercentage += Time.deltaTime / tiltTime;
            cinemachineCamera.Lens.Dutch = Mathf.Lerp(cinemachineCamera.Lens.Dutch, tiltAmount, tiltPercentage);
        }
    }

    [YarnCommand("startCutscene")]
    /// <summary>
    /// Begins a cutscene by setting the camera to follow the given target and disabling player movement
    /// </summary>
    /// <param name="newTarget">The new transform the camera is meant to follow</param>
    public void StartCutscene(GameObject newTarget)
    {
        if(cinemachineCamera == null)
        {
            cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();
        }
        cinemachineCamera.Follow = newTarget.transform;
        playerController.CanMove = false;
        playerController.CanInteract = false;
        Inventory.instance.canInteract = false;
    }

    [YarnCommand("swapTarget")]
    /// <summary>
    /// Changes the target the camera is following during a cutscene
    /// </summary>
    /// <param name="newTarget">The new transform the camera is meant to follow</param>
    public void SwapTarget(GameObject newTarget)
    {
        cinemachineCamera.Follow = newTarget.transform;
    }

    [YarnCommand("changeZoom")]
    public void ChangeZoom(float newZoom)
    {
         cinemachineCamera.Lens.OrthographicSize = newZoom;
    }

    [YarnCommand("changeTilt")]
    public void ChangeTilt(float newTilt, float time = 30)
    {
        tiltPercentage = 0;
        tiltAmount = newTilt;
        tiltTime = time;
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
