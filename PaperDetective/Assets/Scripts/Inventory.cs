using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using System;
using NUnit.Framework;
using System.Linq;

public class Inventory : MonoBehaviour
{
    List<Item> inventory = new List<Item>();
    Vector2[] invPos = new Vector2[10];

    [SerializeField] private GameObject newItemPrefab;
    public GameObject NewItemPrefab { get { return newItemPrefab; } }

    private void Start()
    {
        float x = -6.25f;
        for (int i = 0; i < 10; i++)
        {
            invPos[i] = new Vector3(x, transform.position.y);
            x += 1.39f;
        }
    }

    void Update()
    {

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
                if (inventory.Contains(hitItem))
                {
                    inventory.Remove(hitItem);
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
                        inventory.Add(hitItem);
                    }
                    // if there are none, return it to where it was
                    else
                    {
                        hitItem.targetPos = hitItem.sourcePos;
                    }
                }
                else
                {
                    inventory.Add(hitItem);
                }
            }
        }

        Sort();
    }

    public void Add(Item item)
    {
        inventory.Add(item);
        Sort();
    }

    public void Remove(Item item)
    {
        if (inventory.Contains(item))
            inventory.Remove(item);
        else
            Debug.Log("That item doesn't exist in inventory");
        Sort();
    }

    public void Sort()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            inventory[i].targetPos = invPos[i];
            //Debug.Log("sorting " + i + " " + inventory[i].name + " " + invPos[i]);
        }
    }
}