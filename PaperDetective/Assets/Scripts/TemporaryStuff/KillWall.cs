using UnityEngine;
using Yarn.Unity;

public class KillWall : MonoBehaviour
{
    [SerializeField]
    private GameObject wall;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
