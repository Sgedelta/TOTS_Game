using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Splines.Interpolators;
using UnityEngine.UI;

public class sceneManager : MonoBehaviour
{
    /// <summary>
    /// How long it takes for the screen to be fully blocked
    /// </summary>
    [SerializeField] private float fadeTime;

    /// <summary>
    /// The image attached to the canvas, currently a massive black rectangle
    /// </summary>
    [SerializeField] private Image fadeImage;

    /// <summary>
    /// How far into the fade we currently are
    /// </summary>
    private float fadePercentage = 0;

    /// <summary>
    /// Whether we're currently fading out
    /// </summary>
    private bool fadeOut;

    /// <summary>
    /// Whether we're currently fading in
    /// </summary>
    private bool fadeIn;

    private string nextScene;

    private bool debugMode = true;

    /// <summary>
    /// Singleton time
    /// </summary>
    public static sceneManager instance;
    public void Start()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        SceneManager.sceneLoaded += FadeIn;
        DontDestroyOnLoad(this.gameObject);
    }
    public void FixedUpdate()
    {
        if (fadeOut)
        {
            FadeOut();
            //If we've fully faded out, load the next scene.
            if(fadeImage.color.a >= 1)
            {
                SceneManager.LoadScene(nextScene);
                fadeOut = false;
                fadePercentage = 0;
            }
                
        }
        else if (fadeIn)
        {
            fadePercentage -= Time.deltaTime / fadeTime;
            fadeImage.gameObject.SetActive(true);
            fadeImage.color = Color.Lerp(new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0), new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1), fadePercentage);
            if(fadeImage.color.a <= 0)
            {
                fadeIn = false;
                fadeImage.gameObject.SetActive(false);
            }
        }

    }

    /// <summary>
    /// Begins the fade-out process and remembers the scene you want to transition to.
    /// </summary>
    /// <param name="sceneName">The name of the scene you want to transition into, as seen in the build profile</param>
    public void ChangeScene(string sceneName)
    {
        if(SceneManager.GetActiveScene().name != sceneName)
        {
            fadeOut = true;
            nextScene = sceneName;
        }
            
    }

    public void DebugSceneChange(string sceneName)
    {
        if(debugMode)
        {
            ChangeScene(sceneName);
        }
    }

    /// <summary>
    /// Makes the screen slowly fade to black
    /// </summary>
    private void FadeOut()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PLayerController>().CanMove = false;
        fadePercentage += Time.deltaTime / fadeTime;
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = Color.Lerp(new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0), new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1), fadePercentage);
    }

    /// <summary>
    /// Begins the process of fading in from black
    /// </summary>
    /// <param name="scene">Doesn't do anything, exists so method can be added to SceneManager.sceneLoaded</param>
    /// <param name="mode">Doesn't do anything, exists so method can be added to SceneManager.sceneLoaded</param>
    private void FadeIn(Scene scene, LoadSceneMode mode)
    {
        fadeIn = true;
        fadePercentage = 1;
    }
}
