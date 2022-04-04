using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MoveLegForward();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MoveLegForward()
    {
        transform.Rotate(new Vector3(0, 0, 1) * 5);
        Invoke("MoveLegBackward",0.5f);
    }

    void MoveLegBackward()
    {
        transform.Rotate(new Vector3(0, 0, -1) * 5);
        Invoke("MoveLegForward",0.5f);
    }
}
