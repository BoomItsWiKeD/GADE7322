using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureManager
{
    public static Texture2D FromColourMap(Color[] colourMap, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        
        texture.SetPixels(colourMap);
        texture.Apply();

        return texture;
    }

    public static Texture2D FromHeightMap(float[,] heightMap)
    {
        //Setting width and height using format of [0;1] where 0 is x and 1 is y via GetLength():
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        //Setting the colours of each of the pixels in the texture:
        //Color is 1d array, so for x and y to both be in use, they need to be multiplied:
        Color[] colourMap = new Color[width * height];
        
        //Looping through the noise map to change each pixel colour:
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                //colourMap is 1d array, however it is treated as a 2d grid.
                //"y * width + x" converts 2d coordinates (x, y) into a 1d index, where y is for rows and x is for columns.
                //width is total number of columns in each row (total width of the 2d grid).
                //by multiplying y by width and adding x, a 1d array is made which corresponds to the (x,y) positions.
                
                //Color.Lerp interpolates between 2 colours - i.e. black and white. heightMap[x,y] returns value between 0.0 and 0.1
                //which is the factor used for interpolation, meaning that any colour will be in range between white and black.
                //Closer to 0 = more black. Closer to 1 = more white, between 0 and 1 makes grey colours.
                
                colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x,y]);
            }
        }

        return FromColourMap(colourMap, width, height);
    }
}
