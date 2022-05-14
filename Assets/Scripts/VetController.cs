using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VetController : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 whereTo;
    private int health;
    void Start()
    {
        health = 5;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (health == 0)
        {
            Destroy(gameObject);
        }
    }

   
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag.Equals("Bark"))
        {
            Debug.Log("VET: OUCH!! "+ health);
            health -= 1;
        }
    }
}
