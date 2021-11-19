using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ColissionManager : MonoBehaviour
{
    public List<Physics> Balls;
    public bool collision;
    public float distance = 0.0f; 


    // Start is called before the first frame update
    void Start()
    {
        //Balls = new List<Physics>();
        collision = false;
    }

    void CheckBallToBallCollission()
    {
        for (int i = 0; i <= Balls.Count - 1; i++)
        {
            for (int j = 0; j <= Balls.Count - 1; j++)
            {
                if (i != j)
                {
                    distance = Vector3.Distance(Balls[i].gameObject.transform.position, Balls[j].gameObject.transform.position);
                    if (distance < Balls[i].radius + Balls[j].radius)
                    {
                        Balls[i].OnCollision(); // Colisión detectada
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckBallToBallCollission();
        
    }
}
