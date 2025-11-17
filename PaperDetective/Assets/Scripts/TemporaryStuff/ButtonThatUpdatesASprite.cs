using UnityEngine;

public class ButtonThatUpdatesASprite : MonoBehaviour
{
    public bool toggled = false;

    public string toggledState = "";
    public string unToggledState = "";

    public SpriteUpdater spUpd;

    public void SwapToggle()
    {
        toggled = !toggled;

        spUpd.State = toggled ? toggledState :  unToggledState;
        
    }
}
