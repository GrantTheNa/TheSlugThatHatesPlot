using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlugColourUpdate : MonoBehaviour
{

	//public float variables 
	public float Red = 0.0f;
	public float Green = 0.0f;
	public float Blue = 0.0f;
	public Transform colourChange;
	public Material slugColor;

	void Update()
	{
		//other.GetComponent<Renderer>().material.color = new Color(Red, Green, Blue);
		GetComponent<Renderer>().material.color = colourChange.GetComponent<Renderer>().material.color;
	}
}
