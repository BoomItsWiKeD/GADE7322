using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    //Renderer reference:
    public Renderer textureRender;
    
    //Game object components references:
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    //Draws the mesh:
    public void DrawMesh(MeshData meshData, Texture2D texture)
    {
        meshFilter.sharedMesh = meshData.CreateMesh ();
        meshRenderer.sharedMaterial.mainTexture = texture;
    }
}
