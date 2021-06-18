using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision_Colour_Detection : MonoBehaviour
{

    public float Red = 0.0f;
    public float Green = 0.0f;
    public float Blue = 0.0f;
    public GameObject SlugComponent;
    public int CurrentLayer;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshCollider>().enabled = false;
        gameObject.layer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (SlugComponent.GetComponent<Renderer>().material.color == GetComponent<Renderer>().material.color)
        {
            GetComponent<MeshCollider>().enabled = true;
            gameObject.layer = CurrentLayer;
        }
        else
        {
            GetComponent<MeshCollider>().enabled = false;
            StartCoroutine(FallCollision());
            gameObject.layer = 0;

        }

        }

        IEnumerator FallCollision()
        {
            {
                GetComponent<BoxCollider>().enabled = false;
                yield return new WaitForSeconds(1f);
                GetComponent<BoxCollider>().enabled = true;
            }
        }
    }
