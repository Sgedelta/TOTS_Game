using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "NoteInstance", menuName = "Scriptable Objects/NoteInstance")]
public class NoteInstance : ScriptableObject
{

    [SerializeField] private bool isVisible;
    /// <summary>
    /// If this note should be revealed in the Notes Menu
    /// </summary>
    public bool IsVisible { get { return isVisible; } set { isVisible = value; } }
    
    [SerializeField] private bool isUsed;
    /// <summary>
    /// If this note has been "used" - meaning it's information should be presented with strikethru
    /// </summary>
    public bool IsUsed
    {
        get { return isUsed; }
        set
        {
            //so we can set IsUsed to true and know that it will show up
            if(value)
            {
                isVisible = true;
            }
            isUsed = value;
        }
    }

    [SerializeField] private string noteID;
    /// <summary>
    /// A unique ID for this note, to be used in a dictionary for indexing these
    /// </summary>
    public string NoteID { get { return noteID; } }

    [SerializeField] private string noteContent;
    /// <summary>
    /// The Content of the Note, to be displayed to the user
    /// </summary>
    public string NoteContent { get { return noteContent;  } }

    [SerializeField] private NoteInstance parentInstance;
    /// <summary>
    /// The Parent Note, which this shows as a "sub note" of, like a bulleted list
    /// </summary>
    public NoteInstance ParentInstance {  get { return parentInstance; } }

    [SerializeField] private int noteOrder = 0;
    /// <summary>
    /// The "layer" of the note, in relation to its parent. Notes with the same noteOrder are ranked alphabetically by Note Content.
    /// Use CompareOrder to Compare Order instead of checking Note Order directly
    /// </summary>
    public int NoteOrder { get { return noteOrder; } set { noteOrder = value; } }

    /// <summary>
    /// Compares the sort order of two notes, using NoteOrder and then Content string's alphabetic ordering
    /// </summary>
    /// <param name="other">The note to compare to</param>
    /// <returns>-11 if this note preceeds the other note, 1 if the other note preceeds this note or the other note is null, and 0 if they are equal</returns>
    public int CompareOrder(NoteInstance other)
    {
        if (other == null) return 1;

        if(other.NoteOrder < noteOrder) return -1;
        if(other.NoteOrder > noteOrder) return 1;
        
        return noteContent.ToLower().CompareTo(other.NoteContent.ToLower());

    }
    
}
