﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SphereControl : MonoBehaviour, Icontrollable
{
    private Vector3 drag_position;
    public Renderer rend;
    Color original;


    // Start is called before the first frame update
    void Start()
    {
        drag_position = transform.position;
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        original = rend.material.GetColor("_Color");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, drag_position, 0.05f);
    }

    public void youve_been_touched()
    {
        transform.position += Vector3.right;
        rend.material.color = Color.red;

    }

    public void scaleObject(Vector3 initialScale, float factor)
    {
        transform.localScale = initialScale * factor;
    }

    public void MoveTo(Vector3 destination)
    {
        drag_position = destination;

    }

    public void deselect_object()
    {
        rend.material.color = original;
    }


    
}
