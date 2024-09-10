using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PerlinNoise
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
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        //2d array which stored noise values:
        float[,] noiseMap = new float[mapWidth,mapHeight];

        //Creates a pseudo-random number generator (prng) number using a seed:
        System.Random prng = new System.Random (seed);
        
        //Array which stores offsets for each octave:
        Vector2[] octaveOffsets = new Vector2[octaves];
        
        //Creates random offsets for each octave to ensure seemingly random generation:
        for (int i = 0; i < octaves; i++)
        {
            //Creating large random offsets for x and y:
            float offsetX = prng.Next (-100000, 100000) + offset.x;
            float offsetY = prng.Next (-100000, 100000) + offset.y;
            octaveOffsets [i] = new Vector2 (offsetX, offsetY);
        }

        //Preventing divide by 0 error:
        if (scale <= 0)
        {
            scale = 0.0001f;
        } 
        
        //Variables for normalization uses:
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        //Centering the map:
        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        //For each position on the noise map...
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                //Setting amplitude and frequency for the 1st octave:
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                //Noise map generates using multiple octaves to increase the amount of detail in the map:
                for (int i = 0; i < octaves; i++)
                {
                    //Finding sample positions in the map which are affected by frequency and octaves:
                    float sampleX = (x-halfWidth) / scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y-halfHeight) / scale * frequency + octaveOffsets[i].y;

                    //Get perlin noise value for each point between -1 and 1 (this is the purpose of the *2-1):
                    float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;
                    
                    noiseHeight += perlinValue * amplitude;

                    //For each next octave, decrease amplitude and increase frequency:
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                //Normalization if-statements:
                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                } else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                noiseMap [x, y] = noiseHeight;
            }
        }

        //Normalizing the map so that the height values for each coordinate range between 0 and 1:
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                //MinNoiseHeight = 0, maxNoiseHeight = 1:
                noiseMap [x, y] = Mathf.InverseLerp (minNoiseHeight, maxNoiseHeight, noiseMap [x, y]);
            }
        }

        return noiseMap;
    }

}
