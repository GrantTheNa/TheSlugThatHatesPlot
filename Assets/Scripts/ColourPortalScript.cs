using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColourPortalScript : MonoBehaviour
{

	//public float variables 
	public float Red = 0.0f;
	public float Green = 0.0f;
	public float Blue = 0.0f;
	public GameObject slugChangeColour;

	void OnTriggerEnter(Collider other)
	{
		other.GetComponent<Renderer>().material.color = new Color(Red, Green, Blue);
		SoundManager.PlaySound(SoundManager.Sound.PlayerColourSwitch);
	}
}
