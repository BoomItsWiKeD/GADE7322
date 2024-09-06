using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

//Turns noise map into a texture, then apply the texture to a plane
public class ShowMap : MonoBehaviour
{
    //Creating an instance of the renderer to show the noise texture:
    public Renderer textureRenderer;

    public void DrawTexture(Texture2D texture)
    {
        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }
}
