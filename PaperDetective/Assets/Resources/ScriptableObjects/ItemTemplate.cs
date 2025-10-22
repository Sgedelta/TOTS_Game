using System;
using UnityEngine;

using System.Collections.Generic;

[CreateAssetMenu(fileName = "ItemTemplate", menuName = "Scriptable Objects/ItemTemplate")]
public class ItemTemplate : ScriptableObject
{
    public Sprite itemSprite;
    /// <summary>
    /// The Internal ID of the ItemTemplate, to be used for all internal checks
    /// </summary>
    public string id;
    /// <summary>
    /// The Display name of the Item, to be used for all external facing things
    /// </summary>
    public string displayName;

    /// <summary>
    /// The amount of this item that will be made when the (minimum) proper amount of the ingredients are combined. 
    ///     Debateable if we want actual crafting to combine items multiple times based on the amounts that they actually have.
    /// </summary>
    public float defaultAmount;

    /// <summary>
    /// An array (keep the size to 2, we do not have a system for combining more than 2 items at once) of the items that this is made of
    /// </summary>
    public ItemTemplate[] madeFromItems = new ItemTemplate[2];

    /// <summary>
    /// An array (keep the size to 2) of the amounts of items from madeFromItems that this is made from
    /// </summary>
    public float[] madeFromAmounts = new float[2];

    /// <summary>
    /// A list of items that this is used in. 
    /// </summary>
    public List<ItemTemplate> usedIn; 
}
