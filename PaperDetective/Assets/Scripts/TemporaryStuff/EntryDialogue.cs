using UnityEngine;
using Yarn;
using Yarn.Unity;

public class EntryDialogue : MonoBehaviour
{
    private DialogueRunner runner;

    private bool firstRun = true;

    [SerializeField]
    private string node;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        runner = FindFirstObjectByType<DialogueRunner>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (firstRun)
        {
            runner.StartDialogue(node);
            firstRun = false;
            Destroy(this.gameObject);
        }
    }
}
