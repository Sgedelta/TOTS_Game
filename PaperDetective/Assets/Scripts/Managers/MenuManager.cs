using UnityEngine;
using UnityEngine.UI;

public class script : MonoBehaviour
{
    [SerializeField] GameObject currentTab;
    [SerializeField] GameObject menu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchPage(GameObject tab)
    {
        tab.SetActive(true);
        if (tab != currentTab) currentTab.SetActive(false);
        currentTab = tab;
    }
    /// <summary>
    /// Toggles whether the menu is open. We can also add any pause game logic we need
    /// </summary>
    public void ToggleMenu()
    {
        //Sets player movement and interaction to the opposite of the menu state. If the menu is active, player cannot move or interact and vice versa
        GameObject.FindGameObjectWithTag("Player").GetComponent<PLayerController>().CanMove = menu.activeSelf;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PLayerController>().CanInteract = menu.activeSelf;
        menu.SetActive(!menu.activeSelf);
        SwitchPage(currentTab);
    }
}
