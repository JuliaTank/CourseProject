using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 offset;

    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.position + offset;
        //transform.rotation = player.rotation;
        //transform.rotation = new Quaternion(player.rotation.x,player.rotation.y+50f,player.rotation.z,player.rotation.w);
        
    }

   
}
