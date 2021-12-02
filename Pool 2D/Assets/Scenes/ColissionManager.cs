using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ColissionManager : MonoBehaviour
{
    public List<Physics> Balls;
    public List<Physics> Walls;
    public List<Physics> auxiliarBalls;
    private List<KeyValuePair<Physics, Physics>> collisionRegister;
    public bool collision;
    public float distance = 0.0f;
    public List<Vector3> normals;
    public List<float> depths;

    

    // Start is called before the first frame update
    void Start()
    {
        //Balls = new List<Physics>();
        collision = false;

        collisionRegister = new List<KeyValuePair<Physics, Physics>>();
    }

    void CheckBallToWallCollision()
    {
        for (int i = 0; i < Balls.Count; i++)
        {
            for (int j = 0; j < Walls.Count; j++)
            {
                TableWall wall = (TableWall)Walls[j];

                if (IntersectCirclePolygon(Balls[i], wall.TransformedVertices, out Vector3 normal, out float depth))
                {
                    collisionRegister.Add(new KeyValuePair<Physics, Physics>(Balls[i], Walls[j]));
                    normals.Add(normal);
                    depths.Add(depth);
                }                
            }                        
        }

        resolveCollisionBallToWall();
    }



    public bool IntersectCirclePolygon(Physics circle, Vector3[] vertices, out Vector3 normal, out float depth)
    {
        normal = Vector3.zero;
        depth = float.MaxValue;

        Vector3 axis = Vector3.zero;

        float axisDepth = 0;
        float minA, maxA, minB, maxB;

        // Busco entre todos los vertices de A para generar una proyeccion y ver si hay una separacion (si existe una separacion entonces no existe colision).
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 va = vertices[i];
            Vector3 vb = vertices[(i + 1) % vertices.Length];

            Vector3 edge = vb - va;
            axis = new Vector3(-edge.y, edge.x); // calculo el ejede referencia, creando un nuevo vector con las cordenada intercambiadas y negando la coordenada X para mantener un orde en sentido horario.

            ProjectVertices(vertices, axis, out minA, out maxA);
            ProjectCircle(circle, axis, out minB, out maxB);

            if (minA >= maxB || minB >= maxA)
            {
                return false;
            }

            axisDepth = Mathf.Min(maxB - minA, maxA - minB);

            if (axisDepth < depth)
            {
                depth = axisDepth;
                normal = axis;
            }
        }

        int closestPointIndex = FindClosestPointOnPolygon(circle.transform.position, vertices);
        Vector3 closestPoint = vertices[closestPointIndex];

        axis = closestPoint - circle.transform.position;

        ProjectVertices(vertices, axis, out minA, out maxA);
        ProjectCircle(circle, axis, out minB, out maxB);

        if (minA >= maxB || minB >= maxA)
        {
            return false;
        }

        axisDepth = Mathf.Min(maxB - minA, maxA - minB);

        if (axisDepth < depth)
        {
            depth = axisDepth;
            normal = axis;
        }

        depth /= normal.magnitude;

        normal.Normalize();

        Vector3 polygonCenter = FindArithmeticMean(vertices);

        Vector3 direction = polygonCenter - circle.transform.position;

        if (Vector3.Dot(direction, normal) < 0)
        {
            normal = -normal;
        }

        return true;
    }


    private static void ProjectVertices(Vector3[] vertices, Vector3 axis, out float min, out float max)
    {
        min = float.MaxValue;
        max = float.MinValue;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 v = vertices[i];
            float projection = Vector3.Dot(v, axis);

            if (projection < min) { min = projection; }
            if (projection > max) { max = projection; }
        }
    }

    private static void ProjectCircle(Physics circle, Vector3 axis, out float min, out float max)
    {
        Vector3 direction = axis.normalized;
        Vector3 directionAndRadius = direction * circle.radius;

        Vector3 p1 = circle.transform.position + directionAndRadius;
        Vector3 p2 = circle.transform.position - directionAndRadius;

        min = Vector3.Dot(p1, axis);
        max = Vector3.Dot(p2, axis);

        if (min > max)
        {
            float aux = min;
            min = max;
            max = aux;
        }
    }

    private static int FindClosestPointOnPolygon(Vector3 circleCenter, Vector3[] vertices)
    {
        int result = -1;
        float minDistance = float.MaxValue;

        for (int i = 0; i < vertices.Length; i++)
        {
            float distance = Vector3.Distance(vertices[i], circleCenter);

            if (distance < minDistance)
            {
                minDistance = distance;
                result = i;
            }
        }

        return result;
    }

    private static Vector3 FindArithmeticMean(Vector3[] vertices)
    {
        float sumX = 0;
        float sumY = 0;

        for (int i = 0; i < vertices.Length; i++)
        {
            sumX += vertices[i].x;
            sumY += vertices[i].y;
        }

        return new Vector3(sumX / (float)vertices.Length, sumY / (float)vertices.Length);
    }

    void resolveCollisionBallToWall()
    {

        for (int i = 0; i < collisionRegister.Count; i++)
        {
            collisionRegister[i].Key.OnCollisionBallToWall(collisionRegister[i].Value, normals[i], depths[i]);
            collisionRegister[i].Value.OnCollisionBallToWall(collisionRegister[i].Key, normals[i], depths[i]);
        }

        normals.Clear();
        depths.Clear();
        collisionRegister.Clear();
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

        resolveCollisionBallToBall();
    }

    void resolveCollisionBallToBall()
    {
        for (int i = 0; i < collisionRegister.Count; i++)
        {
            collisionRegister[i].Key.OnCollisionBallToBall(collisionRegister[i].Value);
            collisionRegister[i].Value.OnCollisionBallToBall(collisionRegister[i].Key);
        }

        collisionRegister.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        CheckBallToWallCollision();

        CheckBallToBallCollission();
       


    }
}
