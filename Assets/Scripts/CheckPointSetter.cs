﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointSetter : MonoBehaviour
{
    public Transform currentCheckpoint;
    public Transform spawnSetter;


    void Start()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        currentCheckpoint.transform.position = spawnSetter.transform.position;
    }
}