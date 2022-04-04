using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegMovement2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MoveLegBackward();
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
