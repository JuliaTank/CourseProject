using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MoveTailLeft();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void MoveTailLeft()
    {
        transform.Rotate(new Vector3(0, 70, 0) * 5);
        Invoke("MoveTailRight",0.5f);
    }

    void MoveTailRight()
    {
        transform.Rotate(new Vector3(0, -70, 0) * 5);
        Invoke("MoveTailLeft",0.5f);
    }
}
