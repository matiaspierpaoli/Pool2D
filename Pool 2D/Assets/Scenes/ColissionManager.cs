using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ColissionManager : MonoBehaviour
{
    public List<Physics> Balls;
    public List<Physics> auxiliarBalls;
    private List<KeyValuePair<Physics, Physics>> collisionRegister;
    public bool collision;
    public float distance = 0.0f; 


    // Start is called before the first frame update
    void Start()
    {
        //Balls = new List<Physics>();
        collision = false;

        collisionRegister = new List<KeyValuePair<Physics, Physics>>();
    }

    void CheckBallToBallCollission()
    {
        for (int i = 0; i < Balls.Count; i++)
        {
            // Last i elements are already in place
            for (int j = 0; j < Balls.Count - 1; j++)
            {
                if (i != j)
                {
                    distance = Vector3.Distance(Balls[i].gameObject.transform.position, Balls[j].gameObject.transform.position);
                    if (distance < Balls[i].radius + Balls[j].radius)
                    {                      
                         collisionRegister.Add(new KeyValuePair<Physics, Physics>(Balls[i], Balls[j]));
                    }
                }               
            }                                  
        }     
    }

    void resolveCollision()
    {
        for (int i = 0; i < collisionRegister.Count; i++)
        {
            collisionRegister[i].Key.OnCollision(collisionRegister[i].Value);
            collisionRegister[i].Value.OnCollision(collisionRegister[i].Key);
        }

       


        collisionRegister.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        CheckBallToBallCollission();
        resolveCollision();


    }
}
