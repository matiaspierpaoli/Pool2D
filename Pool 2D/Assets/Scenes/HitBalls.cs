using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBalls : MonoBehaviour
{
    [SerializeField] float maxDistanceForce = 1.2f;
    [SerializeField] float forceMultiplier = 30.5f;

    private Physics whiteBall;
    private Camera cam;
    private Vector2 pixelCoordinatesMousePos;
    private Vector3 worldCoordinatesMousePos;
    private Vector3 direction = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 positionWhereMousePressed = new Vector3(0.0f, 0.0f, 0.0f);
    private float distanceMousePressedMouseReleased = 0.0f;
    private float force = 0.0f;

    void Start()
    {
        cam = Camera.main;
        whiteBall = GetComponent<Physics>();
    }

    // Update is called once per frame
    void Update()
    {
        pixelCoordinatesMousePos = Input.mousePosition;

        worldCoordinatesMousePos = cam.ScreenToWorldPoint(new Vector3(pixelCoordinatesMousePos.x, pixelCoordinatesMousePos.y, cam.nearClipPlane)); //Sacas la posicion del mouse en coordenadas de mundo         

        distanceMousePressedMouseReleased = Vector2.Distance(worldCoordinatesMousePos, positionWhereMousePressed); //Sacamos distancia entre donde se preciono y donde se libero el click del mouse
        distanceMousePressedMouseReleased = Mathf.Clamp(distanceMousePressedMouseReleased, 0.0f, maxDistanceForce); //Te aseguras que la distancia no sea mayor a un maximo

        direction = (positionWhereMousePressed - worldCoordinatesMousePos).normalized; //Sacas la direccion normalizada del vector

   

        if (Input.GetMouseButtonDown(0))
        {
            positionWhereMousePressed = worldCoordinatesMousePos;
        }

        if (Input.GetMouseButtonUp(0))
        {
            
            force = distanceMousePressedMouseReleased * forceMultiplier; //Calculamos la fuerza que va a resivir el rigid body

            whiteBall.HitByCue(direction * force); //Le mandamos la direccion y la fuerza al rigid body
        }
    
    }
}
