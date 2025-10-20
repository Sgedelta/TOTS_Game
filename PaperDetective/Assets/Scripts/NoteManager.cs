using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Yarn.Unity;

public class NoteManager : MonoBehaviour
{
    public static NoteManager Instance { get; private set; }
    public bool saved = false;
    /// <summary>
    /// A dictionary of notes given a note ID and content
    /// </summary>
    Dictionary<string, NoteInstance> notes = new Dictionary<string, NoteInstance>();

    SortedList<(int, string), NoteInstance> sortedNotes = new SortedList<(int, string), NoteInstance>();

    [SerializeField] public TMP_Text noteDisplayText;
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
        NoteInstance[] all = Resources.LoadAll<NoteInstance>("ScriptableObjects/Notes");
        for (int i = 0; i < all.Length; i++)
        {
            notes.Add(all[i].NoteID, all[i]);

            var key = (-all[i].NoteOrder, all[i].NoteID.ToLower());
            sortedNotes.Add(key, all[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetNote(NoteInstance note)
    {
        note.IsUsed = false;
        note.IsVisible = false;
    }

    public void UseNote(string ID)
    {
        notes[ID].IsUsed = true;
    }


    [YarnCommand("RevealNote")]
    public void RevealNote(string ID)
    {
        Debug.Log("hi)");
        notes[ID].IsVisible = true;        
        noteDisplayText.text += notes[ID].NoteContent;
        //Optional
        noteDisplayText.text += "<br>";
        Debug.Log(notes[ID].NoteContent);
    }
}
