using UnityEngine;
using UnityEngine.UI;

public class ChangeSpriteOnHover : MonoBehaviour
{
    [SerializeField] private Sprite hoverSprite; // The sprite to change to on hover
    [SerializeField] private Sprite defaultSprite; // The default sprite
    
    private void OnMouseEnter()
    {
        gameObject.GetComponent<Image>().sprite = hoverSprite;
        gameObject.GetComponent<Image>().SetNativeSize();
    }

    private void OnMouseExit()
    {
        gameObject.GetComponent<Image>().sprite = defaultSprite;
        gameObject.GetComponent<Image>().SetNativeSize();

    }
}
