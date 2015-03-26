using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {

    public int ChunksX = 1;
    public int ChunksY = 1;
    public int ChunksZ = 1;
    public int ChunkSizeX = 27;
    public int ChunkSizeY = 27;
    public int ChunkSizeZ = 27;

    public GameObject chunkObject;

    public Chunk[, ,] chunksData;

    //private int[, ,] blockType;

	// Use this for initialization
	void Start () {
        chunksData = new Chunk[ChunksX, ChunksY, ChunksZ];
	//blockType = new int[ChunksX * ChunkSizeX + 1,ChunksY * ChunkSizeY + 1,ChunksZ * ChunkSizeZ + 1];
        /*//for
        for(int x = 1; x < blockType.GetLength(0) - 1; x++)
        {
            for (int z = 1; z < blockType.GetLength(2) - 1; z++)
            {
                int Surface = PerlinNoise(x, 200, z, 15, 2, 1.2f) + 10;
                for (int y = 1; y < blockType.GetLength(1) - 1; y++)
                {
                    int TrueSurf = Surface + PerlinNoise(y, x, z, 200, 24, 1.5f);
                    if (y <= TrueSurf)
                        blockType[x,y,z] = 1;
                }
            }
        }
        //endfor*/

        StartCoroutine("CreateChunks");
	}

    IEnumerator CreateChunks()
    {
        //for
        for (int x = 0; x < ChunksX; x++)
        {
            for (int y = 0; y < ChunksY; y++)
            {
                for (int z = 0; z < ChunksZ; z++)
                {
                    GameObject go = (GameObject)Instantiate(chunkObject, Vector3.zero, Quaternion.identity);
                    chunksData[x, y, z] = go.GetComponent<Chunk>();
                    go.GetComponent<Chunk>().chunkPosX = x;
                    go.GetComponent<Chunk>().chunkPosY = y;
                    go.GetComponent<Chunk>().chunkPosZ = z;
                    yield return new WaitForSeconds(0.0001f);
                }
            }
        }
        //endfor
    }

	
	// Update is called once per frame
	void Update () {
	
	}

    public int PerlinNoise(int x, int y, int z, float scale, float height, float power)
    {
        float rValue;
        rValue = Noise.GetNoise(((double)x) / scale, ((double)y) / scale, ((double)z) / scale);
        rValue *= height;

        if (power != 0)
        {
            rValue = Mathf.Pow(rValue, power);
        }

        return (int)rValue;
    }

   /* public int BlockType(int x, int y, int z)
    {
        return blockType[x, y, z];
    }*/
}
