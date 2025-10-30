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
    
    /// <summary>
    /// Singleton time
    /// </summary>
    public static sceneManager instance;
    public void Start()
    {
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
    public void ChangeScene(string sceneName)
    {
        if(SceneManager.GetActiveScene().name != sceneName)
        {
            fadeOut = true;
            nextScene = sceneName;
        }
            
    }

    private void FadeOut()
    {
        fadePercentage += Time.deltaTime / fadeTime;
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = Color.Lerp(new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0), new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1), fadePercentage);
    }

    private void FadeIn(Scene scene, LoadSceneMode mode)
    {
        fadeIn = true;
        fadePercentage = 1;
    }
}
