using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public static NoteManager Instance { get; private set; }
    public bool saved = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Instance.saved = true;
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
