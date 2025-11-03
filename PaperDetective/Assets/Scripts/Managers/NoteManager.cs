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

    /// <summary>
    /// Resets a given note's visibility and use state.
    /// </summary>
    /// <param name="note"></param>
    public void ResetNote(NoteInstance note)
    {
        note.IsUsed = false;
        note.IsVisible = false;
    }
    /// <summary>
    /// Uses a note, making its text striken through and drastically increases its NoteOrder and updating the display
    /// </summary>
    /// <param name="ID"></param>
    [YarnCommand("UseNote")]
    public void UseNote(string ID)
    {
        notes[ID].IsUsed = true;
        notes[ID].NoteOrder += 1000;
        UpdateDisplay();
    }

    /// <summary>
    /// Reveals a note, making it visible and updating the display
    /// </summary>
    /// <param name="ID"></param>
    [YarnCommand("RevealNote")]
    public void RevealNote(string ID)
    {
        notes[ID].IsVisible = true;
        UpdateDisplay();
    }
    
    /// <summary>
    /// Updates the display text for the note tab depending on the visible and used notes.
    /// </summary>
    private void UpdateDisplay()
    {
        noteDisplayText.text = string.Empty;
        foreach (NoteInstance note in sortedNotes.Values)
        {
            //If the note is visible, display it, and if the note is used, then make it strikethrough text 
            if (note.IsVisible)
            {
                noteDisplayText.text += (note.IsUsed) ? $"\u2022 <s>{note.NoteContent}</s>" : $"\u2022 {note.NoteContent}<br><br>";
            }
        }
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
