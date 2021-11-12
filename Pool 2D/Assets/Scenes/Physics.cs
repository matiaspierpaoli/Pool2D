using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Physics : MonoBehaviour
{
    float Speed = 1;
    float Force = 5;
    float Mass = 0.16f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float Aceleration = (Mass / Force);

        transform.position +=Vector3.forward * (Speed + Aceleration * Time.deltaTime);
        
    }
}
