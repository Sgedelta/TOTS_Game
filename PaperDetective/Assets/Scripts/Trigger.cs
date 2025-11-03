using UnityEngine;
using UnityEngine.InputSystem;

public class Trigger : MonoBehaviour
{
    public TriggerTemplate template;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        Debug.Log("trigger box jsdfhk");
    }
}
