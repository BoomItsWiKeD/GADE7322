using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class MapBuilder : MonoBehaviour
{
    //Variables which are to be used in the editor and within scripts:
    public static readonly int mapChunkSize = 241;
    [Range(0,6)] //This is to limit the values of the variable by having a slider.
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
    public NavMeshSurface navSurface;

    private void Start()
    {
        //Makes seed between -2,147,483,648 and 2,147,483,647:
        Random randNum = new Random();
        seed = randNum.Next();
        
        //Generates map and mesh:
        GenerateMap();
        
        //Create navmesh:
        navSurface.BuildNavMesh();
    }

    private void Awake()
    {
        falloffMap = Falloff.GenerateFalloffMap(mapChunkSize, branchLength, fadeDistance);
    }

    //Method which generates the map:
    public void GenerateMap()
    {
        noiseMap = PerlinNoise.GenerateNoiseMap (mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);

        //ColourMap array declared to colour parts of the map of specific heights.
        colourMap = new Color[mapChunkSize * mapChunkSize];
        
        //For each coordinate (x,y):
        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                if (useFalloff)
                {
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);
                }
                
                //Checking the height at position:
                float currentHeight = noiseMap [x, y];
                
                //For each position, change colour based on the height at that position:
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

        //Variable used to display the map:
        MapDisplay display = FindObjectOfType<MapDisplay> ();
        
        //Variables used for generating the mesh:
        MeshData meshData = MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail);
        Mesh terrainMesh = meshData.CreateMesh();
        
        //Variables for the game object components:
        MeshFilter meshFilter = meshGameObject.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = meshGameObject.GetComponent<MeshRenderer>();
        MeshCollider meshCollider = meshGameObject.GetComponent<MeshCollider>();
            
        //Update the MeshFilter and MeshRenderer of the "Mesh" game object, and add them if not already applied:
        if (meshFilter == null)
        {
            meshFilter = meshGameObject.AddComponent<MeshFilter>();
        }
        meshFilter.mesh = terrainMesh;
        
        if (meshRenderer == null)
        {
            meshRenderer = meshGameObject.AddComponent<MeshRenderer>();
        }
        meshRenderer.sharedMaterial.mainTexture = TextureBuilder.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize);
        
        //Add meshCollider game component:
        if (meshCollider == null)
        {
            meshCollider = meshGameObject.AddComponent<MeshCollider>();
        }
        meshCollider.sharedMesh = terrainMesh;
        
        //Generate the mesh:
        display.DrawMesh(MeshGenerator.GenerateTerrainMesh (noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureBuilder.TextureFromColourMap (colourMap, mapChunkSize, mapChunkSize));
    }

    //This method checks if the values set in the editor are the default values, and makes sure that they are set correctly:
    //This method also runs in the editor.
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

        falloffMap = Falloff.GenerateFalloffMap(mapChunkSize, branchLength, fadeDistance);
    }
}

//Specifications for the types of terrain (E.g. name="deepWater", height=0.1f, colour="darkBlue")
//Height is a value between 0 and 1 and these values are determined by how dark or light the noise map's colours are.
[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color colour;
}
