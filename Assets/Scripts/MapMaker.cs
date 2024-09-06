using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class MapMaker : MonoBehaviour
{
    //Draw mode options to choose what is to be generated:
    public enum DrawMode
    {
        NoiseMap, ColourMap
    }
    public DrawMode drawMode;
    
    //Variables which are attached to a GO which use the "MakeNoiseMap" method from "PerlinNoise" script. Also shows example values:
    public int mapWidth; //E.g: 100
    public int mapHeight; //E.g: 100
    public float scale; //E.g: 0.3
    public int octaves; //E.g: 4
    public float persistence; //E.g: 0.5
    public float lacunarity; //E.g: 2
    public int seed; //E.g: 23422
    public Vector2 offset; //E.g: x=10.5, y=26.8

    public TypeOfTerrain[] regions;
    
    //calling the "MakeNoiseMap" method from other script so it can be used in this script:
    public void callNoiseMap()
    {
        float[,] noiseMap = PerlinNoise.MakeNoiseMap(mapWidth, mapHeight, scale, octaves, persistence, lacunarity, seed, offset);

        //Creating a colour map for each color specified by the region colours (Same as in ShowMap.cs):
        //Saving colours into this array.
        Color[] colourMap = new Color[mapWidth * mapHeight];
        
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                //Checking current height at each x,y coordinate:
                float currentHeight = noiseMap[x, y];
                
                //Looping through regions to determine which region the height is classified as:
                for (int i = 0; i < regions.Length; i++)
                {
                    //If region is determined, set colour (Same as in ShowMap.cs) then break:
                    if (currentHeight <= regions[i].height)
                    {
                        colourMap[y * mapWidth + x] = regions[i].color;
                        break;
                    }
                }
            }
        }

        ShowMap display = FindObjectOfType<ShowMap>();

        //Drawing the noise map:
        if (drawMode == DrawMode.NoiseMap)
        {
            //Calling the "DrawTexture" method from the general "TextureManager" script:
            display.DrawTexture(TextureManager.FromHeightMap(noiseMap));
        }
        else if (drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureManager.FromColourMap(colourMap, mapWidth, mapHeight));
        }
    }

    //Runs while in editor:
    private void OnValidate()
    {
        //Map width cannot be negative, but must be greater than 0:
        if (mapWidth < 0)
        {
            mapWidth = 1;
        }
        //Map height cannot be negative, but must be greater than 0:
        if (mapHeight < 0)
        {
            mapHeight = 1;
        }
        //Number of octaves must be at least 0:
        if (octaves < 0)
        {
            octaves = 0;
        }
        //Lacunarity must be at least 1:
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        //Persistence must be between 0 and 1:
        if (persistence < 0)
        {
            persistence = 0;
        }
        else if (persistence > 1)
        {
            persistence = 1;
        }
    }
    
    //Making different terrain types for various heights on the noiseMap (Can be changed in inspector):
    [Serializable]
    public struct TypeOfTerrain
    {
        public string name;
        public float height;
        public Color color;
    }
}
