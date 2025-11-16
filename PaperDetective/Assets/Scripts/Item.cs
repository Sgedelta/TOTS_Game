using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Yarn.Unity;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemTemplate template;
    public ItemTemplate Template { get { return template; } } //getter because we don't want other things overwriting what item this is

    public float amount = 1; //for certain items, it will make sense for the player to have more than one of the item, or even fractional items. 
    [SerializeField] private float worldAmount = -1;
    public bool mouseBound;
    public Vector3 sourcePos;
    public Vector3 targetPos;
    LayerMask itemMask;
    LayerMask triggerMask;
    float time;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sourcePos = transform.position;
        targetPos = transform.position;
        itemMask = LayerMask.GetMask("Item");
        triggerMask = LayerMask.GetMask("Item Trigger");
        InitAs(template, 1);
        if (worldAmount > 0)
            amount = worldAmount;
    }

    // Update is called once per frame
    void Update()
    {
        if (mouseBound)
        {
            /*targetPos*/ transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Inventory.instance.CheckItem(template.id))
        {
            transform.position = targetPos;
        }

        Debug.Log($"{template.id}: {Inventory.instance.CheckItem(template.id)}");

        /*
        if (targetPos != transform.position)
        {
            time += Time.deltaTime;
            transform.position = Vector2.Lerp(transform.position, targetPos, 2f / time);
        }
        else
            time = 0;
        */

        if (amount <= 0)
        {
            Inventory.instance.Remove(this);
            Inventory.instance.Sort();
            Debug.Log("GET REKT SCRUB: " + this.name);
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Sets up this Item as the given itemTemplate. Overwrites this item's template field. 
    /// amountToMake is treated as the amount of times this item is being crafted, so if the template's 
    /// default amount is 2 and the amountToMake is 3 the final amount will be 6. 
    /// </summary>
    public void InitAs(ItemTemplate itemTemplate, int amountToMake)
    {
        if (itemTemplate == null) return; //we cannot init when we don't have a template

        template = itemTemplate;
        amount = template.defaultAmount * amountToMake;
        if (template.itemSprite)
            GetComponent<SpriteRenderer>().sprite = GetComponent<Item>().template.itemSprite;
        gameObject.name = template.id;
    }

    public bool CheckInteraction()
    {
        // Check if the item is touching another item
        if (this.gameObject.GetComponent<BoxCollider2D>().IsTouchingLayers(itemMask))
        {
            // Find the object that is closest to the item and return whether a combination was successful

            // Get an array of all objects the item is overlapping
            Collider2D[] hitColliders = Physics2D.OverlapBoxAll(gameObject.transform.position, transform.localScale, itemMask);
            float distance = 100000;
            Collider2D hitItem = null;

            // Find which object the Item is closest to
            if (hitColliders.Length > 2)
            {
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    if ((GetComponent<Collider2D>().transform.position - gameObject.transform.position).magnitude < distance)
                    {
                        distance = (GetComponent<Collider2D>().transform.position - gameObject.transform.position).magnitude;
                        hitItem = GetComponent<Collider2D>();
                    }
                }
            }
            // if there's only two hitcolliders then take the second one (the first one is a self-report)
            else if (hitColliders.Length == 2)
            {
                hitItem = hitColliders[1];
            }
            // check the item combinability with that
            if (hitColliders.Length > 1 && hitItem)
            {
                Debug.Log("Trying to combine");
                int i = CombineWith(hitItem.GetComponent<Item>());
                if (i <= 0)
                    return false;
                else
                    return true;
            }
        }
        // Trigger an event because item got dropped onto a trigger box
        else if (this.gameObject.GetComponent<BoxCollider2D>().IsTouchingLayers(triggerMask))
        {
            // Get the trigger template
            Collider2D trigger = Physics2D.OverlapBox(gameObject.transform.position, transform.localScale, triggerMask);
            TriggerTemplate trigTemp = trigger.GetComponent<Trigger>().template;
            Debug.Log(trigTemp.triggerItem.id + " " + template.id);
            if (trigTemp.triggerItem == template)
            {
                if (trigTemp.madeItem && trigTemp.deleteHeld)
                {
                    Debug.Log("switching");
                    InitAs(trigTemp.madeItem, 1);
                }
                else if (trigTemp.madeItem && !trigTemp.deleteHeld)
                {
                    Debug.Log("making");
                    CreateItem(trigTemp.madeItem, 1);
                }
                else if (trigTemp.deleteHeld)
                {
                    Debug.Log("deleting");
                    amount = 0;
                }
                // Prompt Dialogue
                if (!SingletonComponent.Instances["Dialogue System Variant"].GetComponent<DialogueRunner>().IsDialogueRunning)
                    SingletonComponent.Instances["Dialogue System Variant"].GetComponent<DialogueRunner>().StartDialogue(trigTemp.dialogueNode);
                
                Destroy(trigger.gameObject);
            }
            else
            {
                Debug.Log("Nothing Happens");
            }
        }
        return false;
    }

    /// <summary>
    /// A method that attempts combination with another item. Will CheckCombinationWith and calculate how many we can make. 
    /// </summary>
    /// <param name="other"></param>
    /// <returns>-1 if the combination failed due to items not being allowed to combine. 
    /// 0 if the combination failed due to lack of proper amounts of each item. 
    /// 1 if the combination was successful. </returns>
    public int CombineWith(Item other)
    {
        ItemTemplate result;

        //if we cannot combine, return -1
        if (!CheckCombinationWith(other, out result))
        {
            return -1;
        }

        //default to making the most possible - this gets limited down in the loop
        int amountToMake = int.MaxValue;


        //now, check amounts (result should always be non-null. if there is a null result here something is wrong with the items)
        for (int i = 0; i < result.madeFromItems.Length; i++)
        {
            float itemAmount;

            //only two cases so switch is too much - either the products items are us or them.
            if (result.madeFromItems[i] == template)
            {
                itemAmount = amount;
            }
            else
            {
                itemAmount = other.amount;
            }

            //update amount to make, only ever allowing decreasing amounts
            amountToMake = Mathf.Min(amountToMake, (int)(itemAmount / result.madeFromAmounts[i]));
        }

        //bail if amountToMake is zero
        if (amountToMake < 1)
        {
            return 0;
        }
        // otherwise, update the amount of each item we have
        else
        {
            for (int i = 0; i < result.madeFromItems.Length; i++)
            {
                if (result.madeFromItems[i] == template)
                {
                    amount -= amountToMake * result.madeFromAmounts[i];
                    Debug.Log(name + " amount is " + amount);
                }
                else
                {
                    other.amount -= amountToMake * result.madeFromAmounts[i];
                    Debug.Log(other.name + " amount is " + other.amount);
                }
            }
        }

        // Instantiate new Item (in the proper amount) and place into inventory.
        CreateItem(result, amountToMake);

        return 1;
    }

    public void CreateItem(ItemTemplate temp, int amount)
    {
        // Instantiate new Item (in the proper amount) and place into inventory.
        GameObject madeItem = Instantiate(Inventory.instance.NewItemPrefab);
        Debug.Log("New Item is: " + madeItem.name);
        madeItem.GetComponent<Item>().InitAs(temp, amount);
        Inventory.instance.Add(madeItem.GetComponent<Item>());
        Inventory.instance.Sort();
    }

    /// <summary>
    /// checks if this item can combine with another given item template. Does NOT check if there are the correct amounts of each item, just if a valid recipe exists
    /// </summary>
    /// <param name="other"></param>
    /// <param name="resultItem">the resulting item from the combination</param>
    /// <returns></returns>
    public bool CheckCombinationWith(ItemTemplate other, out ItemTemplate resultItem)
    {
        Debug.Log(this.gameObject.name);
        //check all that this is used in
            foreach (ItemTemplate product in template.usedIn)
            {
                //then all of each of those ingredients (this should only be 2 items)
                foreach (ItemTemplate ingredient in product.madeFromItems)
                {
                    //if the ingredient's template is the correct one
                    if (ingredient == other)
                    {
                        resultItem = product;
                        return true;
                    }
                }
            }
        
        //none of the things work, so we must not be able to combine
        resultItem = null;
        return false;
    }

    /// <summary>
    /// checks if this item can combine with another given item template. Does NOT check if there are the correct amounts of each item, just if a valid recipe exists
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool CheckCombinationWith(ItemTemplate other)
    {
        ItemTemplate result; //we don't use this, but we declare it so that we can keep the check logic to one place
        return CheckCombinationWith(other, out result);
    }

    /// <summary>
    /// Checks if this item can combine with another. does NOT check if there are the correct amounts of each item, just if a valid recipe exists
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool CheckCombinationWith(Item other)
    {
        //bounce to the default function
        return CheckCombinationWith(other.Template);
    }

    /// <summary>
    /// Checks if this item can combine with another. does NOT check if there are the correct amounts of each item, just if a valid recipe exists
    /// </summary>
    /// <param name="other"></param>
    /// <param name="result">the item that is made</param>
    /// <returns></returns>
    public bool CheckCombinationWith(Item other, out ItemTemplate result)
    {
        //bounce to the default function
        return CheckCombinationWith(other.Template, out result);
    }

}
