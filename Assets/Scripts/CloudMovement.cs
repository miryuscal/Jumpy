using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour
{

    public float speed;
    public bool movement = false; 


    void Start()
    {
       
    }

    
    void Update()
    {
        if(movement == false)
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);

        }

    }
}
