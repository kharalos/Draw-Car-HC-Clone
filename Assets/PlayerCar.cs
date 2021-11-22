using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(ExtrudeMesh))]
public class PlayerCar : MonoBehaviour
{
    private MeshFilter meshFilter;
    private LineRenderer lineRenderer;
    private MeshCollider meshCollider;
    private Mesh newMesh;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
    }

    void Update()
    {

    }
    public void GetAnchors(Vector3[] newAnchors)
    {
        Debug.Log("Total Anchor Number is "+newAnchors.Length);

        newMesh = new Mesh();
        lineRenderer.positionCount = newAnchors.Length - 1;
        lineRenderer.SetPositions(newAnchors);
        lineRenderer.BakeMesh(newMesh, true);

        BakeMesh();
    }

    private void BakeMesh()
    {
        meshFilter.mesh = new Mesh();
        meshFilter.mesh = newMesh;
        meshCollider.sharedMesh = newMesh;
        //GetComponent<ExtrudeMesh>().GenerateMesh();
    }
}
