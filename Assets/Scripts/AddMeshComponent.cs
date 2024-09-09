using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMeshComponent : MonoBehaviour
{
    public static GameObject meshGameObject;
    public static void AddMeshComp()
    {
        MeshCollider meshComp = meshGameObject.AddComponent<MeshCollider>();

        meshComp.sharedMesh = meshGameObject.GetComponent<MeshFilter>().mesh;
    }
}
