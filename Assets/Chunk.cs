using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chunk : MonoBehaviour {

    private Mesh mesh;
    private MeshCollider collider;

    private World world;

    private List<int> newTriangles = new List<int>();
    private List<Vector3> newVertices = new List<Vector3>();
    private List<Vector2> newUV = new List<Vector2>();

    private int faceCount;


    public int chunkPosX;
    public int chunkPosY;
    public int chunkPosZ;

    private byte[, ,] blockType;

	// Use this for initialization
	void Start () 
    {
        mesh = GetComponent<MeshFilter>().mesh;
        collider = GetComponent<MeshCollider>();
        world = GameObject.Find("World").GetComponent<World>();
        //GetComponent<MeshRenderer>().material.color = new Color32((byte)(chunkPosX * 15), (byte)(chunkPosY * 15), (byte)(chunkPosZ * 15), 0);
        blockType = new byte[world.ChunkSizeX, world.ChunkSizeY, world.ChunkSizeZ];

        //for
        for (int x = world.ChunkSizeX * chunkPosX; x < world.ChunkSizeX * (chunkPosX + 1); x++)
        {
            for (int z = world.ChunkSizeZ * chunkPosZ; z < world.ChunkSizeZ * (chunkPosZ + 1); z++)
            {
                int Surface = world.PerlinNoise(x, 200, z, 15, 2, 1.2f) + 10;
                for (int y = world.ChunkSizeY * chunkPosY; y < world.ChunkSizeY * (chunkPosY + 1); y++)
                {
                    int TrueSurf = Surface + world.PerlinNoise(y, x, z, 200, 24, 1.5f);
                    if (y <= TrueSurf)
                        blockType[x - world.ChunkSizeX * chunkPosX, y - world.ChunkSizeY * chunkPosY, z - world.ChunkSizeZ * chunkPosZ] = 1;
                }
            }
        }
        //endfor


        GenerateMesh();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void GenerateMesh()
    {
        //for
        for (int x = world.ChunkSizeX * chunkPosX; x < world.ChunkSizeX * (chunkPosX + 1); x++)
        {
            for (int y = world.ChunkSizeY * chunkPosY; y < world.ChunkSizeY * (chunkPosY + 1); y++)
            {
                for (int z = world.ChunkSizeZ * chunkPosZ; z < world.ChunkSizeZ * (chunkPosZ + 1); z++)
                {
                    if (BlockType(x, y, z) != 0)
                    {
                        if (BlockType(x, y + 1, z) == 0)
                        {
                            CubeTop(x, y, z);
                            CreateCubeSurface();
                        }
                        if (BlockType(x, y - 1, z) == 0)
                        {
                            CubeBot(x, y, z);
                            CreateCubeSurface();
                        }
                        if (BlockType(x, y, z + 1) == 0)
                        {
                            CubeNorth(x, y, z);
                            CreateCubeSurface();
                        }
                        if (BlockType(x, y, z - 1) == 0)
                        {
                            CubeSouth(x, y, z);
                            CreateCubeSurface();
                        }
                        if (BlockType(x + 1, y, z) == 0)
                        {
                            CubeEast(x, y, z);
                            CreateCubeSurface();
                        }
                        if (BlockType(x - 1, y, z) == 0)
                        {
                            CubeWest(x, y, z);
                            CreateCubeSurface();
                        }
                    }
                }
            }
        }
        //endfor
        UpdateMesh();
    }

    private void CubeTop(int x, int y, int z)
    {
        newVertices.Add(new Vector3(x, y, z + 1));
        newVertices.Add(new Vector3(x + 1, y, z + 1));
        newVertices.Add(new Vector3(x + 1, y, z));
        newVertices.Add(new Vector3(x, y, z));

    }

    private void CubeBot(int x, int y, int z)
    {
        y--;
        newVertices.Add(new Vector3(x, y, z));
        newVertices.Add(new Vector3(x + 1, y, z));
        newVertices.Add(new Vector3(x + 1, y, z + 1));
        newVertices.Add(new Vector3(x, y, z + 1));

    }

    private void CubeNorth(int x, int y, int z)
    {
        z++;
        newVertices.Add(new Vector3(x + 1, y - 1, z));
        newVertices.Add(new Vector3(x + 1, y, z));
        newVertices.Add(new Vector3(x, y, z));
        newVertices.Add(new Vector3(x, y - 1, z));

    }

    private void CubeSouth(int x, int y, int z)
    {
        newVertices.Add(new Vector3(x, y - 1, z));
        newVertices.Add(new Vector3(x, y, z));
        newVertices.Add(new Vector3(x + 1, y, z));
        newVertices.Add(new Vector3(x + 1, y - 1, z));

    }

    private void CubeEast(int x, int y, int z)
    {
        x++;
        newVertices.Add(new Vector3(x, y -1, z));
        newVertices.Add(new Vector3(x, y, z));
        newVertices.Add(new Vector3(x, y, z + 1));
        newVertices.Add(new Vector3(x, y - 1, z + 1));

    }

    private void CubeWest(int x, int y, int z)
    {
        newVertices.Add(new Vector3(x, y - 1, z + 1));
        newVertices.Add(new Vector3(x, y, z + 1));
        newVertices.Add(new Vector3(x, y, z));
        newVertices.Add(new Vector3(x, y - 1, z));

    }

    private void CreateCubeSurface()
    {
        newTriangles.Add(faceCount * 4);
        newTriangles.Add(faceCount * 4 + 1);
        newTriangles.Add(faceCount * 4 + 2);
        newTriangles.Add(faceCount * 4);
        newTriangles.Add(faceCount * 4 + 2);
        newTriangles.Add(faceCount * 4 + 3);

        faceCount++;
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = newVertices.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();

        newVertices.Clear();
        newUV.Clear();
        newTriangles.Clear();
        faceCount = 0;
    }

    private byte BlockType(int x, int y, int z)
    {
        int Surface = world.PerlinNoise(x, 100, z, 8f, 2f, 2f);
        int TrueSurf = world.PerlinNoise(x, 200, z, 15, 2, 1.2f) + 10 + world.PerlinNoise(y, x, z, 200, 12, 1.5f) + Surface;
        if(x > blockType.GetLength(0))
        {
            if (y <= TrueSurf)
                return 1;
            else
                return 0;
        }
        if(x < blockType.GetLength(0))
        {
            if (y <= TrueSurf)
                return 1;
            else
                return 0;
        }
        if (y > blockType.GetLength(1))
        {
            if (y <= TrueSurf)
                return 1;
            else
                return 0;
        }
        if (y < blockType.GetLength(1))
        {
            if (y <= TrueSurf)
                return 1;
            else
                return 0;
        }
        if (z > blockType.GetLength(2))
        {
            if (y <= TrueSurf)
                return 1;
            else
                return 0;
        }
        if (z < blockType.GetLength(2))
        {
            if (y <= TrueSurf)
                return 1;
            else
                return 0;
        }

        return blockType[x, y, z];
    }

   /* private byte BlockType(int x, int y, int z)
    {
        int TrueSurf = world.PerlinNoise(x, 200, z, 15, 2, 1.2f) + 10 + world.PerlinNoise(y, x, z, 200, 24, 1.5f);
        if (y <= TrueSurf)
            return 1;
        else
            return 0;
    }*/
}
