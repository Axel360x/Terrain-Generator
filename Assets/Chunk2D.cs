using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chunk2D : MonoBehaviour {

    private Mesh mesh;
    private World2D world;

    private List<int> newTriangles = new List<int>();
    private List<Vector3> newVertices = new List<Vector3>();

    private int faceCount;

    public int chunkPosX;
    public int chunkPosY;

	// Use this for initialization
	void Start () {
        mesh = GetComponent<MeshFilter>().mesh;
        world = GameObject.Find("World 2D").GetComponent<World2D>();

        GenerateMesh();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void Block(int x, int y)
    {
        newVertices.Add(new Vector3(x, y, 0));
        newVertices.Add(new Vector3(x + 1, y, 0));
        newVertices.Add(new Vector3(x + 1, y - 1, 0));
        newVertices.Add(new Vector3(x, y - 1, 0));

        newTriangles.Add(faceCount * 4);
        newTriangles.Add(faceCount * 4 + 1);
        newTriangles.Add(faceCount * 4 + 2);
        newTriangles.Add(faceCount * 4);
        newTriangles.Add(faceCount * 4 + 2);
        newTriangles.Add(faceCount * 4 + 3);

        faceCount++;

    }

    private void GenerateMesh()
    {
        //for
        for (int x = world.chunkSizeX * chunkPosX; x < world.chunkSizeX * (chunkPosX + 1); x++)
        {
            for (int y = world.chunkSizeY * chunkPosY; y < world.chunkSizeY * (chunkPosY + 1); y++)
            {
                if (BlockType(x, y) != 0)
                    Block(x, y);
            }
        }
        //endfor

        updateMesh();
    }

    private void updateMesh()
    {
        mesh.Clear();
        mesh.vertices = newVertices.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();

        newVertices.Clear();
        newTriangles.Clear();
        faceCount = 0;
    }

    private int BlockType(int x, int y)
    {
        return world.BlockType(x, y);
    }
}
