using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Physics : MonoBehaviour
{
    [SerializeField]float speed = 1;
    [SerializeField]float force = 3;
    [SerializeField]float mass = 1f;
    [SerializeField]float gravity = 9.8f;
    [SerializeField]float miu = 0.02f;
    [SerializeField]float acceleration =0;
    [SerializeField]float friction =0;

    // Start is called before the first frame update
    void Start()
    {
        float normalForce = (mass * gravity);
        acceleration = (force / mass);
        friction = (miu * normalForce);

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            acceleration = (force / mass);
            speed = 1;
        }

        acceleration -= friction;     

        speed += acceleration;

        if (speed <= 0)
        {
            speed = 0;
        }


        transform.position +=Vector3.right * (speed * Time.deltaTime);
        
    }
}
