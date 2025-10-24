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
        notes[ID].IsVisible = true;  
        //Optional, adds unicode for bullet point
        noteDisplayText.text += "\u2022 ";
        noteDisplayText.text += notes[ID].NoteContent;
        //Optional, spaces the entries by one empty line
        noteDisplayText.text += "<br><br>";
    }
    //When the game is closed, reset all clues
    private void OnApplicationQuit()
    {
        foreach (NoteInstance note in notes.Values)
        {
            ResetNote(note);
        }
    }
}
