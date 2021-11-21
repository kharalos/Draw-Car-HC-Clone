using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCar : MonoBehaviour
{
    private MeshFilter meshFilter;
    private LineRenderer lineRenderer;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    private Mesh newMesh;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
    }

    void Update()
    {

    }
    public void GetAnchors(Vector3[] newAnchors)
    {


        for (int i = 0; i < newAnchors.Length; i++)
        {
            Debug.Log("Total Anchor Number is "+newAnchors.Length);
        }

        newMesh = new Mesh();
        lineRenderer.positionCount = newAnchors.Length - 1;
        lineRenderer.SetPositions(newAnchors);
        lineRenderer.BakeMesh(newMesh, Camera.main, false);
        BakeMesh();
        //DrawMesh(newAnchors);
    }

    private void BakeMesh()
    {
        meshFilter.mesh = newMesh;
        meshCollider.sharedMesh = newMesh;
    }

    private List<Vector3> points = new List<Vector3>();
    private void DrawMesh()
    {
        Vector3[] verticies = new Vector3[points.Count];

        for (int i = 0; i < verticies.Length; i++)
        {
            verticies[i] = points[i];
        }

        int[] triangles = new int[((points.Count / 2) - 1) * 6];

        //Works on linear patterns tn = bn+c
        int position = 6;
        for (int i = 0; i < (triangles.Length / 6); i++)
        {
            triangles[i * position] = 2 * i;
            triangles[i * position + 3] = 2 * i;

            triangles[i * position + 1] = 2 * i + 3;
            triangles[i * position + 4] = (2 * i + 3) - 1;

            triangles[i * position + 2] = 2 * i + 1;
            triangles[i * position + 5] = (2 * i + 1) + 2;
        }
 
 
        Mesh mesh = GetComponent <MeshFilter > ().mesh;
        mesh.Clear();
        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
