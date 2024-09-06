using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise //No mono behaviour because not attached to GOs, can be static class
{
    /*
    Generating noise map method which makes a grid with values of 0 and 1 (B and W):
    Method has 2d float array return type, and 2 parameters for map size, due to the user having to be able-
    to specify map size when called then it returns an output in the form of a 2d array.
    Scale is a number to change the x and/or y values to non-integer values.
    
    Octave = Level of detail for the map (number of layers of noise). The doubling of frequency. I.e. if 40Hz, one octave above it is 80Hz. Always whole number (int)
    Persistence = How much each octave contributes to overall shape (adjusts amplitude). Is Between 0 and 1.
    Lacunarity = How much detail is added or removed per octave (adjusts frequency). Higher value means more finer noise. Is Greater than 1.
    Seed = Number used to predict what noise generation happens. Offset is used to move the noise pattern across 2d space.
    Offset = Vector coordinate number to be added to original offset coordinates for more randomness.
    */

    public static float[,] MakeNoiseMap(int mapWidth, int mapHeight, float scale, int octaves, float persistence, float lacunarity, int seed, Vector2 offset)
    {
        //Variable for float 2d array "map":
        float[,] noiseMap = new float[mapWidth, mapHeight];
        
        //Generating a random number for the seed:
        System.Random randNum = new System.Random(seed);
        
        //1d array of floats used for offsets:
        //It picks a point in 2d space and stores it.
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = randNum.Next(-100000, 100000) + offset.x;
            float offsetY = randNum.Next(-100000, 100000) + offset.y;

            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        //If statement to fix divide by 0 error for scale.
        if (scale <= 0)
        {
            //Minimum scale value if scale=0:
            scale = 0.000001f;
        }

        //Used for normalizing the noise height:
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;
        
        //When "zooming into" the noise map when changing scale, it focuses in top right corner of the noiseMap.
        //To change this, center of map is identified (optional code):
        float midMapWidth = mapWidth / 2f;
        float midMapHeight = mapHeight / 2f;
        
        //Looping through the "map" x and y values, because each coordinate is compromised of x and y "(x; y)":
        //To do this, a nested for loop is needed (similar to 1st year 2d console app project)
        //Start from minimum y value and loop until reaching max y value (mapHeight)
        for (int y = 0; y < mapHeight; y++)
        {
            //For each y value, an x value is needed, so start from minimum x value and loop until reaching max x value (mapWidth)
            for (int x = 0; x < mapWidth; x++)
            {
                float frequency = 1,
                    amplitude = 1,
                    noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    //samples for x and y values (x = x / scale) and scale is to change x and y to seem more random
                    //BUT if scale = 0, then error because cannot divide by 0, if statement is outside and above loops to fix this:
                    //Also higher frequency = further apart scale coordinates.
                    //Also adding the offset number to the values.
                    //The "- midMapWidth" is used to center the "zooming in" when changing scale (can be removed).
                    float sampleX = (x - midMapWidth) / scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - midMapHeight) / scale * frequency + octaveOffsets[i].y;

                    //Using sampleX and sampleY for built-in perlinNoise method derived from the built-in Mathf class:
                    //Generates perlin noise and scales it between -1 and 1 (the "* 2 - 1" part is used for scaling):
                    float perlinVal = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;

                    //Increasing noiseHeight by the perlinValue of each octave:
                    noiseHeight += perlinVal * amplitude;
                    
                    //At the end of an octave, amplitude is multiplied by persistence value:
                    //Since persistence is between 0 and 1 (is a fraction), it decreases the octaves.
                    amplitude *= persistence;

                    //Frequency increases each octave based on lacunarity, therefore affecting how fine the noise is:
                    frequency *= lacunarity;
                }
                
                //Then applying the noiseHeight to the noise map and tracking the max and min values for normalization:
                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                
                noiseMap[x, y] = noiseHeight;
            }
        }
        
        //Going through the loops again to check what range the noiseMap values are in:
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                //for each value in the noiseMap, set it to Mathf.InverseLerp.
                //If noiseMap value = minNoiseHeight, return 0. If noiseMap value = maxNoiseHeight, return 1. Etc.
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }
        
        return noiseMap;
    }
}
