using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointSetter : MonoBehaviour
{
    public Transform currentCheckpoint;
    public Transform spawnSetter;

    void OnTriggerEnter(Collider other)
    {
        currentCheckpoint.transform.position = spawnSetter.transform.position;
        SoundManager.PlaySound(SoundManager.Sound.CheckpointSetter_Interaction);
    }
}
