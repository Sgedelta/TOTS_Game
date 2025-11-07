using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections.Generic;
using System;
using NUnit.Framework;
using System.Linq;
using Yarn;
using Yarn.Unity;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] Sprite square;
    [SerializeField] private int capacity;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private float slotPaddingMin = 25;
    SortedList<string, Item> inventory = new SortedList<string, Item>();
    [SerializeField] private List<Item> itemsInInv;
    private List<GameObject> invSlots = new List<GameObject>();
    Camera cam;

    [SerializeField] private GameObject newItemPrefab;
    public GameObject NewItemPrefab { get { return newItemPrefab; } }

    private DialogueRunner dialogue;

    public static Inventory instance;

    /// <summary>
    /// This determines the singular instance of GameManager that will exist across all scenes.
    /// </summary>
    private void Awake()
    {
        //Check if there is no instance of this Inventory that exists.
        if (instance == null)
        {
            //Set the gameObject this script is attached to as the single instance of the Inventory.
            instance = this;

            //This method informs Unity to retain the object this script is attached to when changing scenes.
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            //If there is already an instance of the Inventory, then delete the object this is attached to.
            //This ensures that only one instance of the Inventory exists across all scenes.
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        RectTransform trans = GetComponent<RectTransform>();

        //clamp capacity
        capacity = (int)Mathf.Clamp(capacity, 0, trans.rect.width / (slotPrefab.GetComponent<RectTransform>().rect.width + slotPaddingMin * 2));

        //calculate box width
        float boxWidth = trans.rect.width / capacity;


        //create slots
        for(int i = 0; i < capacity; i++)
        {
            GameObject slot = Instantiate(slotPrefab, trans);
            
            invSlots.Add(slot);

            slot.GetComponent<RectTransform>().localPosition = new Vector3(boxWidth * (i - (capacity / 2) + 0.5f), 0, 0); //note: might have to update z for order

        }

        itemsInInv = GameManager.instance.items;
        cam = Camera.main;

        dialogue = FindFirstObjectByType<DialogueRunner>();
    }

    void Update()
    {
        Debug.Log("Screen Width : " + Screen.width);
        Sort();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, new Vector2(0, 1), 0.01f, LayerMask.GetMask("Item"));

        // if player clicked on an item
        if (hit && hit.collider.gameObject.CompareTag("Item"))
        {
            Item hitItem = hit.collider.gameObject.GetComponent<Item>();
            // if the left mouse button is clicked
            if (context.performed)
            {
                // Item follows cursor
                hit.collider.gameObject.GetComponent<Item>().mouseBound = true;

                // Remove item from inventory and show on mouse
                if (inventory.ContainsKey(hitItem.Template.id))
                {
                    inventory.Remove(hitItem.Template.id);
                    hitItem.GetComponent<SpriteRenderer>().color = Color.white;
                }
            }

            // if left mouse button is released
            else if (context.canceled)
            {
                Debug.Log("unclick");
                // item stops following cursor and returns to the inventory box if the interaction failed
                hitItem.mouseBound = false;
                if (!hitItem.CheckInteraction())
                {
                    // Flash red to suggest failed interaction?

                    if (inventory.Count < 10)
                    {
                        inventory.Add(hitItem.Template.id, hitItem);
                    }
                    // if there are none, return it to where it was
                    else
                    {
                        hitItem.targetPos = hitItem.sourcePos;
                    }
                }
                else
                {
                    inventory.Add(hitItem.Template.id, hitItem);
                }
            }
            Sort();
        }

    }

    [YarnCommand("AddItem")]
    public void Add(Item item)
    {
        inventory.Add(item.Template.id, item);
        itemsInInv.Add(item);
        GameManager.instance.items = itemsInInv;
        Sort();
    }


    public void Remove(Item item)
    {
        if (itemsInInv.Contains(item)) { itemsInInv.Remove(item); }
        GameManager.instance.items = itemsInInv;

        if (inventory.ContainsKey(item.Template.id))
            inventory.Remove(item.Template.id);
        else
            Debug.Log("That item doesn't exist in inventory");
        Sort();
    }

    [YarnCommand("RemoveItem")]
    public void RemoveAndDestroy(Item item)
    {
        if (itemsInInv.Contains(item)) { itemsInInv.Remove(item); }
        GameManager.instance.items = itemsInInv;

        if (inventory.ContainsKey(item.Template.id))
            inventory.Remove(item.Template.id);
        else
            Debug.Log("That item doesn't exist in inventory");


        item.amount = 0;

        Sort();
    }

    public void Sort()
    {
        int x = 0;

        //note: a little quick and dirty, won't update things "outside" of inventory - should really handle this on inventory adding side\
        //fill slots with images and position items into clickable location
        for (int i = 0; i < Mathf.Min(capacity, inventory.Count); i++)
        {
            invSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = inventory.Values[i].Template.itemSprite;
            invSlots[i].transform.GetChild(0).GetComponent<Image>().color = Color.white;
            inventory.Values[i].GetComponent<SpriteRenderer>().color = Color.clear;

            inventory.Values[i].targetPos = invSlots[i].transform.position;

            x++;
        }
        //make all unfilled slots empty
        for(int i = x;  i < capacity; i++)
        {
            invSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
            invSlots[i].transform.GetChild(0).GetComponent<Image>().color = Color.clear;
        }
        
    }

    public void TransformItem(Item existingItem, ItemTemplate newItem)
    {
        inventory.Remove(existingItem.Template.id);
        existingItem.InitAs(newItem, 1);
        inventory.Add(existingItem.Template.id, existingItem);
        Sort();
    }

    [YarnCommand("TransformItem")]
    public void TransformItem(string ogItem, string newItem)
    {
        ItemTemplate it = new ItemTemplate();
        Item i = new Item();
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in items)
        {
            if (item.GetComponent<Item>().Template.id == newItem)
                it = item.GetComponent<Item>().Template;
            else if (item.GetComponent<Item>().Template.id == ogItem)
                i = item.GetComponent<Item>();
        }
        TransformItem(i, it);
    }


    [YarnCommand("Check")]
    public bool CheckItem(string itemName)
    {
        if (inventory.ContainsKey(itemName))
        {
            dialogue.VariableStorage.SetValue($"$has{itemName}", true);
        }

        return inventory.ContainsKey(itemName);      
    }
}