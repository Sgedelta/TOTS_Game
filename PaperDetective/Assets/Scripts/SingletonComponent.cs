using System.Collections.Generic;
using UnityEngine;

public class SingletonComponent : MonoBehaviour
{

    public static Dictionary<string, GameObject> Instances = new Dictionary<string, GameObject>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(Instances.TryGetValue(gameObject.name, out GameObject instance) && instance != null && instance != this.gameObject)
        {
            Destroy(this);
            return;
        }

        if(Instances.ContainsKey(gameObject.name))
        {
            Instances[gameObject.name] = this.gameObject;
            
        } else
        {
            Instances.Add(gameObject.name, this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
