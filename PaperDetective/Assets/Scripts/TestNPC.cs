using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using Yarn.Unity;

public class TestNPC : MonoBehaviour
{
    [SerializeField] private string startNode = "";
    [SerializeField] DialogueRunner dialogueRunner;

    private void Awake()
    {
        if (dialogueRunner == null)
        {
            dialogueRunner = FindAnyObjectByType<DialogueRunner>();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //TODO: if the player is close enough, allow interact key bind to also work 
        //Quick and dirty if clicked
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit && hit.collider.tag == "NPC")
            {
                if (!dialogueRunner.IsDialogueRunning) dialogueRunner.StartDialogue(startNode);
            }
        }
    }
}
