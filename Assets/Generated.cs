using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(MeshCollider))]
public class Generated : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;

    public void GetMesh(Mesh combinedMesh)
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();

        meshFilter.mesh = new Mesh();

        meshFilter.mesh = combinedMesh;
        meshCollider.sharedMesh = combinedMesh;
    }
}
