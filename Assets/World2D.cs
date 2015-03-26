using UnityEngine;
using System.Collections;

public class World2D : MonoBehaviour {

    public GameObject chunkObject;
    public int chunkSizeX = 10;
    public int chunkSizeY = 10;
    public int chunksX = 1;
    public int chunksY = 1;

    private int[,] blockType;

	// Use this for initialization
	void Start () {
	    blockType = new int[chunksX * chunkSizeX + 1,chunksY * chunkSizeY + 1];

        //for
        for(int x = 1; x < blockType.GetLength(0) - 1; x++)
        {
            int Surface = PerlinNoise(x, 200, 200, 40, 20, 1.2f) + 200;
            for (int y = 1; y < blockType.GetLength(1) - 1; y++)
            {
                int TrueSurf = PerlinNoise(y, 200, x, 15, 50, 1.5f);
                if (y <= Surface)
                    blockType[x, y] = 1;
                if (y <= TrueSurf)
                    blockType[x, y] = 0;
            }
        }
        //endfor

        //for
        for (int x = 0; x < chunksX; x++)
        {
            for (int y = 0; y < chunksY; y++)
            {
                    GameObject go = (GameObject)Instantiate(chunkObject, Vector3.zero, Quaternion.identity);
                    go.GetComponent<Chunk2D>().chunkPosX = x;
                    go.GetComponent<Chunk2D>().chunkPosY = y;
            }
        }
        //endfor
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private int PerlinNoise(int x, int y, int z, float scale, float height, float power)
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

    public int BlockType(int x, int y)
    {
        return blockType[x, y];
    }
}
