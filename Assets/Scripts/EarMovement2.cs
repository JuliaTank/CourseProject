using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarMovement2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MoveEarUp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   private void MoveEarUp()
    {
        transform.Rotate(new Vector3(0, 0, 10) * 3);
        Invoke("MoveEarDown",0.5f);
    }

    private void MoveEarDown()
    {
        transform.Rotate(new Vector3(0, 0, -10) * 3);
        Invoke("MoveEarUp",0.5f);
    }
}
