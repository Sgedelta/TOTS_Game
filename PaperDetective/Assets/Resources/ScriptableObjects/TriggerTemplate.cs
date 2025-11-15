using System;
using UnityEngine;

using System.Collections.Generic;
using Yarn;

[CreateAssetMenu(fileName = "TriggerTemplate", menuName = "Scriptable Objects/TriggerTemplate")]
public class TriggerTemplate : ScriptableObject
{
    // internal ID
    public string id;

    // what item triggers the reaction
    public ItemTemplate triggerItem;

    // what item to give to the player
    public ItemTemplate madeItem;

    // what dialogue to prompt
    public string dialogueNode;

    // do you remove the object that triggered this event
    public bool deleteHeld;
}
