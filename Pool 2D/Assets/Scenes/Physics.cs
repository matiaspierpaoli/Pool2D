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
    Vector3 direction;
    bool isStatic;
    float airFriction;
    float airDensity=1.225f;
    float airFrictionConstant=0.000000667f;
    Vector3 normal;
    float depth;


    // Start is called before the first frame update
    void Start()
    {
        float normalForce = (mass * gravity);
        acceleration = (force / mass);
        friction = (miu * normalForce);

       airFriction = airFrictionConstant * 0.5f * airDensity * (radius * radius) / 4;
       
    }

    // Update is called once per frame
    void Update()
    {
        acceleration -= friction;     
        acceleration -= airFriction;

        speed += acceleration * Time.deltaTime;

        if (speed <= 0)
        {
            speed = 0;
        }

        transform.position += direction * (speed * Time.deltaTime);
        
    }

    public void HitByCue(Vector3 force)
    {
        if (!isStatic)
        {
            direction.z = 0;

            this.direction = force.normalized;

            acceleration = Mathf.Abs(force.magnitude) / mass;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }


    public void OnCollisionBallToBall(Physics other)
    {
        if (gameObject.tag == "Ball" && other.tag == "Ball")
        {
            float distance = Vector3.Distance(gameObject.transform.position, other.gameObject.transform.position);

            normal = (other.transform.position - transform.position).normalized;
            depth = radius + other.radius - distance;

            Vector3 normalForceA = -normal * Mathf.Abs((mass * acceleration) - (other.mass * other.acceleration)) / 2;

            Vector3 directionForceA = direction * (mass * acceleration) / 2;

            Vector3 resultA = (normalForceA + directionForceA);

            if (!isStatic)
            {
                transform.position += (-normal * depth / 2);
            }

            HitByCue(resultA);
        }

    }

    public void OnCollisionBallToWall(Physics other, Vector3 normal, float depth)
    {
        if (gameObject.tag == "Ball" && other.tag == "Walls")
        {
            Vector3 directionForceA = direction * (mass * acceleration) / 2;

            Vector3 resultA = Vector3.Reflect(directionForceA, -normal);

            if (!isStatic)
            {
                transform.position += (-normal * depth);
            }

            HitByCue(resultA);
        }
    }
}
