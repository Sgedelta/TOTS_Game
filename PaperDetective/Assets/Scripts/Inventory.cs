using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using System;
using NUnit.Framework;
using System.Linq;
using Yarn;
using Yarn.Unity;

public class Inventory : MonoBehaviour
{
    SortedList<string, Item> inventory = new SortedList<string, Item>();
    [SerializeField] private List<Item> itemsInInv;
    Vector2[] invPos = new Vector2[10];
    Camera cam;

    [SerializeField] private GameObject newItemPrefab;
    public GameObject NewItemPrefab { get { return newItemPrefab; } }

    private DialogueRunner dialogue;

    private void Start()
    {
        cam = Camera.main;
        float x = -8f;
        for (int i = 0; i < 10; i++)
        {
            invPos[i] = new Vector3(x, -4.1f + cam.transform.position.y);
            x += 1.76f;
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
        Sort();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, new Vector2(0, 1), 0.01f);

        // if player clicked on an item
        if (hit && hit.collider.gameObject.CompareTag("Item"))
        {
            Item hitItem = hit.collider.gameObject.GetComponent<Item>();
            // if the left mouse button is clicked
            if (context.performed)
            {
                // Item follows cursor
                hit.collider.gameObject.GetComponent<Item>().mouseBound = true;

                // Remove item from inventory
                if (inventory.ContainsKey(hitItem.Template.id))
                {
                    inventory.Remove(hitItem.Template.id);
                }
            }

            // if left mouse button is released
            else if (context.canceled)
            {
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
        Sort();
    }

    [YarnCommand("RemoveItem")]
    public void Remove(Item item)
    {
        if (inventory.ContainsKey(item.Template.id))
            inventory.Remove(item.Template.id);
        else
            Debug.Log("That item doesn't exist in inventory");
        Sort();
    }

    public void Sort()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            invPos[i] = new Vector2(invPos[i].x, -4.1f + cam.transform.position.y);
            inventory.Values[i].targetPos = invPos[i];
            //Debug.Log("sorting " + i + " " + inventory[i].name + " " + invPos[i]);
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