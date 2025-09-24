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
    Vector3[] invPos = new Vector3[10];

    private void Start()
    {
        float x = -6.25f;
        for (int i = 0; i < 10; i++)
        {
            invPos[i] = new Vector3(x, transform.position.y, 0);
            x += 1.39f;
            print(invPos[i]);
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
            // if mouseDown
            if (context.performed)
            {
                // Item follows cursor
                hit.collider.gameObject.GetComponent<Item>().mouseBound = true;

                // Remove item from inventory
                if (inventory.Contains(hitItem))
                {
                    for (int i = 0;i < invPos.Length; i++)
                    {
                        if ((Vector2)hitItem.gameObject.transform.position == (Vector2)invPos[i])
                        {
                            invPos[i].z = 0;
                            break;
                        }
                    }
                    inventory.Remove(hitItem);
                }
            }
            // if mouseUp
            else if (context.canceled)
            {
                // item stops following cursor and returns to the inventory box if the interaction failed
                hitItem.mouseBound = false;
                if (!hitItem.CheckInteraction())
                {
                    // Flash red to suggest failed interaction?

                    // Find the first empty inventory slot and move the item there
                    if (inventory.Count < 10)
                    {
                        inventory.Add(hitItem);
                        for (int i = 0; i < invPos.Length; i++)
                        {
                            if (invPos[i].z == 0)
                            {
                                invPos[i].z = 1;
                                hitItem.targetPos = invPos[i];
                                break;
                            }
                        }
                    }
                    // if there are none, return it to where it was
                    else
                    {
                        hitItem.targetPos = hitItem.sourcePos;
                    }
                }
            }
        }

    }
}