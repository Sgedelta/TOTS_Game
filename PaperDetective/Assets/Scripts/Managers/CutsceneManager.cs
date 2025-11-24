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
    
    private float zoomPercentage;

    private float zoomTime;

    private float zoomAmount = 5;

    private float movePercentage;

    private float moveTime;

    private Vector2 moveAmount;

    private bool cutsceneActive = false;

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
        if(!cutsceneActive)
        {
            return;
        }

        if (Mathf.Abs(cinemachineCamera.Lens.Dutch - tiltAmount) > .5)
        {
            tiltPercentage += Time.deltaTime / tiltTime;
            cinemachineCamera.Lens.Dutch = Mathf.Lerp(cinemachineCamera.Lens.Dutch, tiltAmount, tiltPercentage);
        }

        if (Mathf.Abs(cinemachineCamera.Lens.OrthographicSize - zoomAmount) > .5)
        {
            zoomPercentage += Time.deltaTime / zoomTime;
            cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(cinemachineCamera.Lens.OrthographicSize, zoomAmount, zoomPercentage);
        }

        if(Mathf.Abs(cinemachineCamera.gameObject.transform.position.x - moveAmount.x) > .1 || Mathf.Abs(cinemachineCamera.gameObject.transform.position.y - moveAmount.y) > .1)
        {
            movePercentage += Time.deltaTime / moveTime;
            cinemachineCamera.Follow.position = new Vector3(Mathf.Lerp(cinemachineCamera.Follow.position.x, moveAmount.x, movePercentage),
                                                           Mathf.Lerp(cinemachineCamera.Follow.position.y, moveAmount.y, movePercentage),
                                                           cinemachineCamera.Follow.position.z);
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
        SwapTarget(newTarget);
        playerController.CanMove = false;
        playerController.CanInteract = false;
        Inventory.instance.canInteract = false;
        cutsceneActive = true;
    }

    [YarnCommand("swapTarget")]
    /// <summary>
    /// Changes the target the camera is following during a cutscene
    /// </summary>
    /// <param name="newTarget">The new transform the camera is meant to follow</param>
    public void SwapTarget(GameObject newTarget, float time = 30)
    {
        moveAmount = new Vector2(newTarget.transform.position.x, newTarget.transform.position.y);
        movePercentage = 0;
        moveTime = time;
    }

    [YarnCommand("changeZoom")]
    public void ChangeZoom(float newZoom, float time = 30)
    {
        zoomPercentage = 0;
        zoomAmount = newZoom;
        zoomTime = time;
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
