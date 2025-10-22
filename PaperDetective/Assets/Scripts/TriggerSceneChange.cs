using UnityEngine;

public class TriggerSceneChange : MonoBehaviour
{
    [SerializeField] private string sceneName;

    private sceneManager sceneManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sceneManager = GameObject.FindFirstObjectByType<sceneManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PLayerController>())
        {
            sceneManager.ChangeScene(sceneName);
        }
    }
}
