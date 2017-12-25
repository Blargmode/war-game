using System;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour {

    public int depth = 20;
    public int width = 256;
    public int height = 256;
    public float scale = 20f;
    public float offsetX = 100f;
    public float offsety = 100f;

    private void Update()
    {
        //randomize offset x and y to make random terrain
        //offsetX = Random.Range(0f, 9999f);
        //offsetY = Random.Range(0f, 9999f);

        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    private TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;

        terrainData.size = new Vector3(width, depth, height);

        terrainData.SetHeights(0, 0, GenerateHeights());

        return terrainData;
    }

    private float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                heights[x, y] = CalcHeight(x, y);
            }
        }

        return heights;
    }

    private float CalcHeight(int x, int y)
    {
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)y / height * scale + offsety;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}
