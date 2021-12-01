using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Physics : MonoBehaviour 
{
    [SerializeField]float speed = 0;
    [SerializeField]float force = 0.2f;
    [SerializeField]float mass = 1f;
    [SerializeField]float gravity = 9.8f;
    [SerializeField]float miu = 0.02f;
    [SerializeField]float acceleration =0;
    [SerializeField]float friction =0;
    [SerializeField]public float radius = 2;



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
            speed = 0;
        }

        acceleration -= friction;     

        speed += acceleration * Time.deltaTime;

        if (speed <= 0)
        {
            speed = 0;
        }

        transform.position += new Vector3(0,0,2) * (speed * Time.deltaTime);
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }


    public void OnCollisionBallToBall(Physics other)
    {
        //if (other.tag == )
        //{

        //}

        acceleration -= friction;

        speed += acceleration * Time.deltaTime;

        if (speed <= 0)
        {
            speed = 0;
        }

        other.transform.position -= new Vector3(1, 0, 0) * (speed * Time.deltaTime);


    }

    public void OnCollisionBallToWall(Physics other)
    {
        other.transform.position -= new Vector3(0, 0, 2) * (speed * Time.deltaTime);

    }
}
