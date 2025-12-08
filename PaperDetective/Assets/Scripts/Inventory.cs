using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections.Generic;
using System;
using NUnit.Framework;
using System.Linq;
using Yarn;
using Yarn.Unity;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class Inventory : MonoBehaviour
{
    [SerializeField] bool inventoryHide;
    [SerializeField] Sprite square;
    [SerializeField] private int capacity;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private float slotPaddingMin = 25;
    private SortedList<string, Item> inventory;
    [SerializeField] private List<Item> itemsInInv;
    private List<GameObject> invSlots = new List<GameObject>();
    Camera cam;

    [SerializeField] private GameObject newItemPrefab;
    public GameObject NewItemPrefab { get { return newItemPrefab; } }

    private DialogueRunner dialogue;
    List<TextMeshProUGUI> itemNames;

    public static Inventory instance;
    public bool canInteract = true;
    
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
            DontDestroyOnLoad(this.transform.parent.gameObject);
            Debug.Log("Keeping the inventory " + this.transform.parent.gameObject.name);

            if (inventoryHide)
            {
                this.transform.parent.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log("Removing the inventory " + this.transform.parent.gameObject.name);
            //If there is already an instance of the Inventory, then delete the object this is attached to.
            //This ensures that only one instance of the Inventory exists across all scenes.

            if (inventoryHide)
            {
                this.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                Inventory.instance.transform.parent.gameObject.SetActive(true);
            }
            Destroy(this.transform.parent.gameObject);
        }
    }

    private void Start()
    {
        itemNames = new List<TextMeshProUGUI>();
        inventory = new SortedList<string, Item>();

        //create slots
        ResetAllInventorySlots();  

        itemsInInv = GameManager.instance.items;
        cam = Camera.main;

        dialogue = FindFirstObjectByType<DialogueRunner>();
    }

    void Update()
    {
        if(transform.parent.GetComponent<Canvas>().worldCamera != Camera.main)
        {
            transform.parent.GetComponent<Canvas> ().worldCamera = Camera.main;
        }
        //Debug.Log("Screen Width : " + Screen.width);
        Sort();
    }

    private void ResetAllInventorySlots()
    {
        while (invSlots.Count > 0)
        {
            GameObject slot = invSlots[0];
            invSlots.RemoveAt(0);
            Destroy(slot);
        }

        RectTransform trans = GetComponent<RectTransform>();

        //clamp capacity
        capacity = (int)Mathf.Clamp(capacity, 0, trans.rect.width / (slotPrefab.GetComponent<RectTransform>().rect.width + slotPaddingMin * 2));

        //calculate box width
        float boxWidth = trans.rect.width / capacity;

        for (int i = 0; i < capacity; i++)
        {
            GameObject slot = Instantiate(slotPrefab, trans);

            invSlots.Add(slot);
            itemNames.Add(slot.GetComponentInChildren<TextMeshProUGUI>());
            itemNames[i].color = Color.clear;

            slot.GetComponent<RectTransform>().localPosition = new Vector3(boxWidth * (i - (capacity / 2) + 0.5f), 0, 0); //note: might have to update z for 
        }
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!canInteract)
        {
            return;
        }
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, new Vector2(0, 1), 0.01f, LayerMask.GetMask("Item"));

        // if player clicked on an item
        if (hit && hit.collider.gameObject.CompareTag("Item"))
        {
            hit.collider.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 12;

            Item hitItem = hit.collider.gameObject.GetComponent<Item>();
            // if the left mouse button is clicked
            if (context.performed)
            {
                // Item follows cursor
                hit.collider.gameObject.GetComponent<Item>().mouseBound = true;

                // Remove item from inventory and show on mouse
                if (Inventory.instance.inventory.ContainsKey(hitItem.Template.id))
                {
                    Inventory.instance.inventory.Remove(hitItem.Template.id);
                    Debug.Log($"Removing {hitItem.Template.id} and putting it in mouse");
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

                    if (Inventory.instance.inventory.Count < 10)
                    {
                        Inventory.instance.inventory.Add(hitItem.Template.id, hitItem);
                        DontDestroyOnLoad(hitItem.gameObject);
                        if(GameManager.instance.AllPersistentData.ContainsKey(hitItem.Template.id))
                        {
                            GameManager.instance.AllPersistentData[hitItem.Template.id].KeepOnLoad = false;
                        }
                    }
                    // if there are none, return it to where it was
                    else
                    {
                        hitItem.targetPos = hitItem.sourcePos;
                    }
                }
                else
                {
                    Inventory.instance.inventory.Add(hitItem.Template.id, hitItem);
                    DontDestroyOnLoad(hitItem.gameObject);
                    if (GameManager.instance.AllPersistentData.ContainsKey(hitItem.Template.id))
                    {
                        GameManager.instance.AllPersistentData[hitItem.Template.id].KeepOnLoad = false;
                    }
                }
            }
            //Sort();
        }

    }

    [YarnCommand("AddItem")]
    public void Add(Item item)
    {
        if(item == null)
        {
            return;
        }


        if(Inventory.instance.inventory.ContainsKey(item.Template.id))
        {
            Inventory.instance.inventory[item.Template.id].amount += item.amount;
            return;
        }

        Inventory.instance.inventory.Add(item.Template.id, item);
        DontDestroyOnLoad(item.gameObject);
        itemsInInv.Add(item);
        GameManager.instance.items = itemsInInv;
        //Sort();
    }


    public void Remove(Item item)
    {
        if (itemsInInv.Contains(item)) 
        { 
            itemsInInv.Remove(item); 
            GameManager.instance.items = itemsInInv;
        }

        if (Inventory.instance.inventory.ContainsKey(item.Template.id))
        {
            Inventory.instance.inventory.Remove(item.Template.id);

        }
        else
        {
            Debug.Log("That item doesn't exist in inventory");
        }
        //Sort();
    }

    [YarnCommand("RemoveItem")]
    public void RemoveAndDestroy(Item item)
    {
        Remove(item);

        item.amount = 0;

        //Sort();
    }

    public void Sort()
    {
        int x = 0;


        //note: a little quick and dirty, won't update things "outside" of inventory - should really handle this on inventory adding side\
        //fill slots with images and position items into clickable location
        for (int i = 0; i < Mathf.Min(capacity, instance.inventory.Count); i++)
        {
            invSlots[i].GetComponentsInChildren<Image>()[1].sprite = instance.inventory.Values[i].Template.itemSprite;
            invSlots[i].GetComponentsInChildren<Image>()[1].color = Color.white;

            instance.inventory.Values[i].GetComponent<SpriteRenderer>().color = Color.clear;
            instance.inventory.Values[i].targetPos = invSlots[i].transform.position;

            if (i >= itemNames.Count)
            {
                itemNames.Add(instance.invSlots[i].GetComponentInChildren<TextMeshProUGUI>());
                itemNames[i].color = Color.clear;
                Debug.Log(instance.invSlots[i].GetComponentInChildren<TextMeshProUGUI>());
            }
            itemNames[i].text = instance.inventory.Values[i].Template.displayName;

            x++;
        }
        //make all unfilled slots empty
        for(int i = x;  i < capacity; i++)
        {
            //Debug.Log($"{i}: Capacity is {capacity}");
            //Debug.Log($"{i}: {invSlots[i].transform}");
            invSlots[i].GetComponentsInChildren<Image>()[1].sprite = null;
            invSlots[i].GetComponentsInChildren<Image>()[1].color = Color.clear;
            itemNames[i].color = Color.clear;
            itemNames[i].text = "";
        }
    }

    public void TransformItem(Item existingItem, ItemTemplate newItem)
    {
        Inventory.instance.inventory.Remove(existingItem.Template.id);
        existingItem.InitAs(newItem, 1);
        Inventory.instance.inventory.Add(existingItem.Template.id, existingItem);

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

    public void DisplayName(bool display, GameObject slot)
    {
        if (invSlots.Contains(slot))
        {
            int slotI = invSlots.IndexOf(slot);
            if (itemNames[slotI])
            {
                if (display)
                {
                    itemNames[slotI].color = Color.white;
                    itemNames[slotI].outlineColor = Color.black;
                    itemNames[slotI].faceColor = Color.white;
                }
                else
                {
                    itemNames[slotI].color = Color.clear;
                }
            }
        }
    }


        [YarnCommand("Check")]
    public bool CheckItem(string itemName)
    {
        /*
        string db = $"{itemName} in the inventory of size {Inventory.instance.inventory.Count}: ";
        foreach(Item i in Inventory.instance.inventory.Values)
        {
            db += i.Template.id + ", ";
        }
        Debug.Log(db);
        */


        if (Inventory.instance.inventory.ContainsKey(itemName))
        {
            dialogue.VariableStorage.SetValue($"$has{itemName}", true);
        }

        return inventory.ContainsKey(itemName);      
    }
}
