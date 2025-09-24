using System;
using UnityEngine;

public class temp_move : MonoBehaviour
{

    //this file is quick and dirty to get an object moving.

    [SerializeField] Vector2[] positions;
    [SerializeField] float[] times;
    private float nextTime = 0;
    private float timeElapsed = 0;
    private int index = 1;
    private int lastIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //needs 2 position-times
        if (positions.Length <= 1 && times.Length <= 1)
        {
            this.enabled = false;
        }

        nextTime = times[0];
    }

    // Update is called once per frame
    void Update()
    {


        Vector2 newPos = Vector2.Lerp(positions[lastIndex], positions[index], timeElapsed / nextTime);
        gameObject.transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
        

        timeElapsed += Time.deltaTime;
        if(timeElapsed > nextTime)
        {
            timeElapsed -= nextTime;
            lastIndex = index;
            index = (index + 1) % positions.Length;
            nextTime = times[index];
        }
    }
}
