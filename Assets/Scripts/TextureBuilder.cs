using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureBuilder
{
    //Creating a texture from a colour array:
    public static Texture2D TextureFromColourMap(Color[] colourMap, int width, int height)
    {
        //Creating and specifying the texture to be created:
        Texture2D texture = new Texture2D (width, height);
        
        //Removing the "blurry-ness" of the texture:
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        
        //Apply the colour map to the texture:
        texture.SetPixels (colourMap);
        texture.Apply ();
        
        return texture;
    }
}
