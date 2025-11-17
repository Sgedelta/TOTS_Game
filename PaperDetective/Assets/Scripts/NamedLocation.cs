using UnityEngine;

public class NamedLocation : MonoBehaviour
{
    //Author: Sam Easton
    // Essentially just a data holder that can be put on an object and found/referenced. 

    //this needs to be unique to the scene!
    [SerializeField] private string locationName;
    public string LocationName { get { return locationName; } }

    //you could just do NamedLocation.gameObject.transform.position but... NamedLocation.pos is nicer.
    public Vector3 pos { get { return transform.position; } }



}
