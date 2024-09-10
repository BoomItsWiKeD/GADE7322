using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
    //Generates the mesh vertices to form triangles on the map:
    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve, int levelOfDetail)
    {
        //Finding width and height dimensions of the map to generate triangles:
        int width = heightMap.GetLength (0);
        int height = heightMap.GetLength (1);
        
        //Finding the center of the map and is always positive:
        float midX = (width - 1) / -2f;
        float midZ = (height - 1) / 2f;

        //Controls how much the mesh is simplified depending on preference:
        //Higher level of detail means less vertices and triangles.
        int meshSimplificationIncrement = 1;
        if (levelOfDetail == 0)
        {
            meshSimplificationIncrement = 1;
        }
        else
        {
            meshSimplificationIncrement = levelOfDetail * 2;
        }
        
        //How many vertices will be placed on a line of the mesh after mesh can be simplified:
        int verticesPerLine = (width - 1) / meshSimplificationIncrement + 1;

        //Generate the mesh data based on the height map and the level of detail:
        MeshData meshData = new MeshData (verticesPerLine, verticesPerLine);
        int vertexIndex = 0;

        //For each position on the height map...
        for (int y = 0; y < height; y+= meshSimplificationIncrement)
        {
            for (int x = 0; x < width; x+= meshSimplificationIncrement)
            {
                //Calculate the position of each vertex, using the height map and a height multiplier (based on a curve):
                //MidX and MidZ are offsets for centering the mesh.
                //Also setting the UV coordinates for texture mapping, which must be normalized.
                meshData.vertices [vertexIndex] = new Vector3 (midX + x, heightCurve.Evaluate(heightMap [x, y]) * heightMultiplier, midZ - y);
                meshData.uvs [vertexIndex] = new Vector2 (x / (float)width, y / (float)height);

                //Only generate the triangles if not using the most right and most bottom vertices (otherwise not enough vertices for triangles):
                if (x < width - 1 && y < height - 1)
                {
                    //Creating two triangles to make quads:
                    //Each triangle connects the current vertex with the two vertices from the next row and column.
                    meshData.AddTriangle (vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
                    meshData.AddTriangle (vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        return meshData;
    }
}

public class MeshData
{
    public Vector3[] vertices; //X,y and z positions of each vertex.
    public int[] triangles; //Index for each triangle in an array.
    public Vector2[] uvs; //Coordinates of each vertex.

    int triangleIndex;

    //Constructor:
    public MeshData(int meshWidth, int meshHeight)
    {
        vertices = new Vector3[meshWidth * meshHeight];
        uvs = new Vector2[meshWidth * meshHeight];
        
        //Each quad is 2 triangles (6 verts), and there are (width-1)*(height)-1 quads:
        triangles = new int[(meshWidth-1)*(meshHeight-1)*6];
    }

    //Creating triangles based on three vertices (a, b and c):
    public void AddTriangle(int a, int b, int c)
    {
        triangles [triangleIndex] = a;
        triangles [triangleIndex + 1] = b;
        triangles [triangleIndex + 2] = c;
        triangleIndex += 3;
    }

    //Creating the mesh in unity:
    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        
        mesh.RecalculateNormals();
        return mesh;
    }
}
