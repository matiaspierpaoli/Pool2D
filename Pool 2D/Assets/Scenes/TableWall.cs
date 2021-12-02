using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableWall : Physics
{
    [SerializeField] float width = 1;
    [SerializeField] float height = 1;

    Vector3[] vertices = new Vector3[4];

    public Vector3[] TransformedVertices => vertices;

    private void Start()
    {
        CalculateVertices();
    }

    private void LateUpdate()
    {
        CalculateVertices();
    }

    private void CalculateVertices()
    {
        vertices[0] = transform.position + (-transform.right * width / 2) + (transform.up * height / 2);
        vertices[1] = transform.position + (transform.right * width / 2) + (transform.up * height / 2);
        vertices[2] = transform.position + (transform.right * width / 2) + (-transform.up * height / 2);
        vertices[3] = transform.position + (-transform.right * width / 2) + (-transform.up * height / 2);
    }

}
