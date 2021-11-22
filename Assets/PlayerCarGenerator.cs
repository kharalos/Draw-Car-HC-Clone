using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarGenerator : MonoBehaviour
{

    // Get start and end nodes poses to place the wheels
    private Vector3 startNode, endNode;

    private Mesh[] meshesToCombine;

    private Mesh combinedMesh;

    [SerializeField]
    private Generated generated;
    public void GetTheMeshes()
    {
        /*Array.Clear(meshesToCombine, 0, meshesToCombine.Length);

        int i = 0;

        foreach (MeshFilter segMeshFilter in GetComponentsInChildren<MeshFilter>())
        {
            meshesToCombine[i] = segMeshFilter.mesh;

            i++;
        }*/
        CombineTheMeshes();
    }


    void CombineTheMeshes()
    {
        var objectMesh = transform.GetComponent<MeshFilter>().mesh;
        objectMesh = new Mesh();
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 2;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].transform.GetComponent<MeshRenderer>().enabled = false;

            i++;
        }
        objectMesh = new Mesh();
        objectMesh.CombineMeshes(combine);

        generated.GetMesh(objectMesh);

        objectMesh = new Mesh();

    }

}
