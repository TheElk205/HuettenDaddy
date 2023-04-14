using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startpos;

    public Camera camera;

    public float parallaxEffect;
    public float yOffset = 0;
    private float currentOffset = 0;

    private Vector3 velocity;

    private float smoothFactor = 0.25f;
    
    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        currentOffset = transform.position.y;
    }

    private void FixedUpdate()
    {
        Vector3 temp = (camera.transform.position * (1 - parallaxEffect));
        Vector3 dist = (camera.transform.position * parallaxEffect);

        transform.position = new Vector3(startpos + dist.x, transform.position.y, transform.position.z);

        if (temp.x > startpos + length)
        {
            startpos += length;
        }
        else if (temp.x < startpos - length)
        {
            startpos -= length;
        }
    }
}
