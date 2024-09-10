using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Falloff
{
    //Generates monotone (black and white) map for the map to be made of islands:
    public static float[,] GenerateFalloffMap(int size, float branchLength, float fadeDistance)
    {
        float[,] map = new float[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float x = i / (float)size * 2 - 1;
                float y = j / (float)size * 2 - 1;

                //Calculating the distance from the center (0, 0):
                float distanceFromCenter = Mathf.Sqrt(x * x + y * y);

                //Calculating the falloff values based on the terrain (outside the multiply sign):
                float falloffValue = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
                falloffValue = Evaluate(falloffValue);

                //Determine if a point is near one of the branches of the multiply sign:
                float distanceToBranch;
                float distanceToEnd;
                float value;
                if (IsInMultiplySign(x, y, branchLength, fadeDistance, out distanceToBranch, out distanceToEnd))
                {
                    //Smoothly transition from white (1) to darker value based on distance:
                    float branchFade = Mathf.InverseLerp(0, fadeDistance, distanceToBranch);
                    float endFade = Mathf.InverseLerp(0, fadeDistance, distanceToEnd);
                    
                    //Smoothly interpolate based on both diagonal and branch end distances:
                    float transitionFactor = Mathf.Max(branchFade, endFade);
                    value = Mathf.Lerp(1f, falloffValue, transitionFactor);
                }
                else
                {
                    //Use normal falloff values outside the multiply sign for normal generation:
                    value = falloffValue;
                }

                map[i, j] = value;
            }
        }

        return map;
    }

    static bool IsInMultiplySign(float x, float y, float branchLength, float fadeDistance, out float distanceToBranch, out float distanceToEnd)
    {
        //Adding tolerance for the thickness of the branches:
        float tolerance = 0.05f;

        //Calculating the lengths of the diagonal lines (multiply sign branches)
        float distanceToDiagonal1 = Mathf.Abs(x - y); //Distance to top-left to bottom-right diagonal
        float distanceToDiagonal2 = Mathf.Abs(x + y); //Distance to top-right to bottom-left diagonal

        //Using the minimum distance to the closest diagonal:
        distanceToBranch = Mathf.Min(distanceToDiagonal1, distanceToDiagonal2);

        //Using the length of the branch:
        distanceToEnd = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y)) - branchLength;

        //Is the point is within the branch length and fade distance?
        if (Mathf.Abs(x) < branchLength + tolerance && distanceToBranch < fadeDistance)
        {
            return true;
        }

        return false;
    }

    static float Evaluate(float value)
    {
        float a = 3;
        float b = 2.2f;

        // S-shape graph equation for falloff map to use:
        return Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b * value, a));
    }
}
