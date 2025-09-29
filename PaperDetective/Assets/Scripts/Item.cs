using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemTemplate template;
    ItemTemplate Template { get { return template; } } //getter because we don't want other things overwriting what item this is

    public float amount; //for certain items, it will make sense for the player to have more than one of the item, or even fractional items. 
    [SerializeField] private float startAmount = -1;
    public bool mouseBound;
    public Vector3 sourcePos;
    public Vector3 targetPos;
    LayerMask mask;
    float time;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sourcePos = transform.position;
        mask = LayerMask.GetMask("Interactable");
        InitAs(template, 1);
        if (startAmount > 0)
            amount = startAmount;
    }

    // Update is called once per frame
    void Update()
    {
        if (mouseBound)
        {
            targetPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (targetPos != transform.position)
        {
            time += Time.deltaTime;
            transform.position = Vector2.Lerp(transform.position, targetPos, 2f/time);
        }
        else
            time = 0;
    }

    /// <summary>
    /// Sets up this Item as the given itemTemplate. Overwrites this item's template field. 
    /// amountToMake is treated as the amount of times this item is being crafted, so if the template's 
    /// default amount is 2 and the amountToMake is 3 the final amount will be 6. 
    /// </summary>
    public void InitAs(ItemTemplate itemTemplate, int amountToMake)
    {
        template = itemTemplate;
        amount = template.defaultAmount * amountToMake;
    }

    public bool CheckInteraction()
    {
        if (this.GetComponent<BoxCollider2D>().IsTouchingLayers(mask))
        {
            // Check for interaction
            return true;
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
        if(!CheckCombinationWith(other, out result))
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
            if(result.madeFromItems[i] == template)
            {
                itemAmount = amount;
            } else
            {
                itemAmount = other.amount;
            }

            //update amount to make, only ever allowing decreasing amounts
            amountToMake = Mathf.Min(amountToMake, (int)(amount / result.madeFromAmounts[i]));
        }

        //bail if amountToMake is zero
        if(amountToMake < 1)
        {
            return 0;
        }

        //TODO: Instantiate new Item (in the proper amount) and place into inventory.

        //TODO: Update the amounts of the items we have, and then potentially (likely) delete this and/or other if their amounts are 0


        return 1;
        
    }


    /// <summary>
    /// checks if this item can combine with another given item template. Does NOT check if there are the correct amounts of each item, just if a valid recipe exists
    /// </summary>
    /// <param name="other"></param>
    /// <param name="resultItem">the resulting item from the combination</param>
    /// <returns></returns>
    public bool CheckCombinationWith(ItemTemplate other, out ItemTemplate resultItem)
    {
        //check all that this is used in
        foreach(ItemTemplate product in template.usedIn)
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
