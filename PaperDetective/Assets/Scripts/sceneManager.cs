using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
    [SerializeField] private bool change;
    public void ChangeScene(string sceneName)
    {
        if(SceneManager.GetActiveScene().name != sceneName)
            SceneManager.LoadScene(sceneName);
    }
}
