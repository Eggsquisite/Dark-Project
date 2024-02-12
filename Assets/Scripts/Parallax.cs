using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startPos;
    public float parallaxFactor;
    public GameObject cam;


    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float temp = cam.transform.position.x * (1 - parallaxFactor);
        float distance = cam.transform.position.x * parallaxFactor; 

        Vector3 newPosition = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        transform.position = newPosition;

        if (temp > startPos + (length / 2)) startPos += length;
        else if (temp < startPos - (length / 2)) startPos -= length;
    }
}
