using UnityEngine;

public class Item : MonoBehaviour
{
    string name;
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
}
