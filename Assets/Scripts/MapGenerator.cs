using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode
    {
        NoiseMap, ColourMap, Mesh, FalloffMap
    };
    public DrawMode drawMode;

    public static readonly int mapChunkSize = 241;
    [Range(0,6)]
    public int levelOfDetail;
    public float noiseScale;

    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public bool useFalloff;
    public float branchLength;
    public float fadeDistance;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public bool autoUpdate;

    public TerrainType[] regions;

    public float[,] noiseMap;
    private Color[] colourMap;
    private float[,] falloffMap;

    public GameObject meshGameObject;

    private void Start()
    {
        //Makes seed between -2,147,483,648 and 2,147,483,647:
        Random randNum = new Random();
        seed = randNum.Next();
        
        //Generates map and mesh:
        GenerateMap();
    }

    private void Awake()
    {
        falloffMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize, branchLength, fadeDistance);
    }

    public void GenerateMap()
    {
        noiseMap = Noise.GenerateNoiseMap (mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);

        colourMap = new Color[mapChunkSize * mapChunkSize];
        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                if (useFalloff)
                {
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);
                }
                float currentHeight = noiseMap [x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions [i].height)
                    {
                        colourMap [y * mapChunkSize + x] = regions [i].colour;
                        break;
                    }
                }
            }
        }

        MapDisplay display = FindObjectOfType<MapDisplay> ();

        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture (TextureGenerator.TextureFromHeightMap (noiseMap));
        }
        else if (drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture (TextureGenerator.TextureFromColourMap (colourMap, mapChunkSize, mapChunkSize));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            // Generate the terrain mesh
            MeshData meshData = MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail);
            Mesh terrainMesh = meshData.CreateMesh();

            // Update the MeshFilter and MeshRenderer of the "Mesh" game object
            MeshFilter meshFilter = meshGameObject.GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                meshFilter = meshGameObject.AddComponent<MeshFilter>();
            }
            meshFilter.mesh = terrainMesh;

            MeshRenderer meshRenderer = meshGameObject.GetComponent<MeshRenderer>();
            if (meshRenderer == null)
            {
                meshRenderer = meshGameObject.AddComponent<MeshRenderer>();
            }
            meshRenderer.sharedMaterial.mainTexture = TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize);

            // Add or update the MeshCollider
            MeshCollider meshCollider = meshGameObject.GetComponent<MeshCollider>();
            if (meshCollider == null)
            {
                meshCollider = meshGameObject.AddComponent<MeshCollider>();
            }
            meshCollider.sharedMesh = terrainMesh;
            
            display.DrawMesh (MeshGenerator.GenerateTerrainMesh (noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColourMap (colourMap, mapChunkSize, mapChunkSize));
            
        }
        else if (drawMode == DrawMode.FalloffMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(mapChunkSize, branchLength, fadeDistance)));
        }
        
        float minHeight = Mathf.Infinity;
        float maxHeight = -Mathf.Infinity;
        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float height = noiseMap[x, y];
                minHeight = Mathf.Min(minHeight, height);
                maxHeight = Mathf.Max(maxHeight, height);
            }
        }
        Debug.Log($"Height map range: Min: {minHeight}, Max: {maxHeight}");
    }

    void OnValidate()
    {
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }

        falloffMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize, branchLength, fadeDistance);
    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color colour;
}
