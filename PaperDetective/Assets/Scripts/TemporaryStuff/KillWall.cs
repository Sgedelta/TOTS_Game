using UnityEngine;
using Yarn.Unity;

public class KillWall : MonoBehaviour
{
    [SerializeField]
    private GameObject wall;

    private DialogueRunner runner;

    [SerializeField]
    private string node;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        runner = FindFirstObjectByType<DialogueRunner>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 6)
        {
            runner.StartDialogue(node);
        }
    }

    [YarnCommand("KillWall")]
    public void Kill()
    {
        if(wall != null)
        {
            Destroy(wall);
        }
    }
}
