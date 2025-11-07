using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections.Generic;
using System;
using NUnit.Framework;
using System.Linq;
using Yarn;
using Yarn.Unity;

public class Inventory : MonoBehaviour
{
    [SerializeField] Sprite square;
    SortedList<string, Item> inventory = new SortedList<string, Item>();
    [SerializeField] private List<Item> itemsInInv;
    Vector2[] invPos = new Vector2[10];
    [SerializeField] SpriteRenderer[] invSquares = new SpriteRenderer[10];
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
        itemsInInv = GameManager.instance.items;
        cam = Camera.main;
        float x = -10.5f;
        for (int i = 0; i < 10; i++)
        {
            invPos[i] = new Vector3(x, -5.3f + cam.transform.position.y);
            x += 2.4f;
        }
        if (itemsInInv != null)
        {
            foreach (Item item in itemsInInv)
            {
                inventory.Add(item.Template.id, item);
            }
        }

        dialogue = FindFirstObjectByType<DialogueRunner>();
    }

    void Update()
    {
        Debug.Log("Screen Width : " + Screen.width);
        Sort();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!canInteract)
        {
            return;
        }
        //Debug.Log("click1");
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
        for (int i = 0; i < inventory.Count; i++)
        {
            invSquares[i].sprite = inventory.Values[i].Template.itemSprite;
            invSquares[i].GetComponent<SpriteRenderer>().color = Color.white;
            inventory.Values[i].GetComponent<SpriteRenderer>().color = Color.clear;
            inventory.Values[i].targetPos = new Vector2(invPos[i].x + cam.transform.position.x, -5.3f + cam.transform.position.y);
            //Debug.Log("sorting " + i + " " + inventory[i].name + " " + invPos[i]);
            x++;
        }
        for (int i = x; i < 10; i++)
        {
            Debug.Log(x);
            invSquares[i].color = Color.clear;
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