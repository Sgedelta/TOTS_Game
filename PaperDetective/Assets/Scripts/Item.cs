using UnityEngine;

public class Item : MonoBehaviour
{
    private ItemTemplate template;
    ItemTemplate Template { get { return template; } } //getter because we don't want other things overwriting what item this is

    public float amount; //for certain items, it will make sense for the player to have more than one of the item, or even fractional items. 
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
    /// checks if this item can combine with another given item template. Does NOT check if there are the correct amounts of each item, just if a valid recipe exists
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool CheckCombinationWith(ItemTemplate other)
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
                    return true;
                }
            }
        }
        //none of the things work, so we must not be able to combine
        return false;
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

}
