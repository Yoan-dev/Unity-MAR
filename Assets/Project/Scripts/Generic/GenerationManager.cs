﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerationManager : MonoBehaviour
{

    public Terrain terrain;
    private TerrainData terrainData;

    #region Prefabs;

    public GameObject start;
    public GameObject checkpoint;

    #endregion Prefabs;

    #region Metrics;

    public int[] minX = new int[] { 0 };
    public int[] maxX = new int[] { 500 };
    public int[] minZ = new int[] { 0 };
    public int[] maxZ = new int[] { 500 };
    public int[] minElevations = new int[] { 2 };
    public int[] maxElevations = new int[] { 100 };
    public float[] baseElevation = new float[] { 0.3f };
    public float[] minElevationHeight = new float[] { 0.2f };
    public float[] maxElevationHeight = new float[] { 0.7f };
    public int[] minElevationRadius = new int[] { 10 };
    public int[] maxElevationRadius = new int[] { 100 };
    public float[] elevationsMinGapFactor = new float[] { 0.5f };
    public int seed = 0;
    public bool useSeed = false;

    #endregion Metrics;
    
    void Start()
    {
        terrainData = terrain.terrainData;
    }

    public void Generate()
    {
        //for (int i = 0; i < GameObject.Find("CheckpointsManager").transform.childCount; i++)
            //Destroy(GameObject.Find("CheckpointsManager").transform.GetChild(i).gameObject);
        Map map = new Map();
		map.Initialize ();
		/*map.UpdateMap(
            minX[0],
            maxX[0],
            minZ[0],
            maxZ[0],
            minElevations[0],
            maxElevations[0],
            baseElevation[0],
            minElevationHeight[0],
            maxElevationHeight[0],
            minElevationRadius[0],
            maxElevationRadius[0],
            elevationsMinGapFactor[0]
        );*/
        if (useSeed)
            Random.seed = seed;
        Debug.Log("Seed: " + Random.seed);
        map.Generate();
        terrainData.SetHeights(0, 0, map.GetHeights());
        terrainData.SetAlphamaps(0, 0, map.GetTextures());

        //Debug.Log(map.GetStartingPosition()[0] + ", " + map.GetStartingPosition()[1]);
        /*Instantiate(start, 
            new Vector3(
            terrainData.size.x - map.GetStartingPosition()[1], 
            6,
            map.GetStartingPosition()[0]), 
            Quaternion.identity, GameObject.Find("CheckpointsManager").transform);
        foreach (int[] coords in map.GetCheckpoints())
        {
            Instantiate(checkpoint,
                new Vector3(
                terrainData.size.x - coords[1],
                6,
                coords[0]),
                Quaternion.identity, GameObject.Find("CheckpointsManager").transform);
        }*/

        #region;     
        return;//
		for (int i = 1; i < minX.GetLength(0); i++)
        {
            map.UpdateMap(
                minX[i],
                maxX[i],
                minZ[i],
                maxZ[i],
                minElevations[i],
                maxElevations[i],
                baseElevation[i],
                minElevationHeight[i],
                maxElevationHeight[i],
                minElevationRadius[i],
                maxElevationRadius[i],
                elevationsMinGapFactor[i]
            );
            map.GenerateUpdate();
            terrainData.SetHeights(0, 0, map.GetHeights());
            terrainData.SetAlphamaps(0, 0, map.GetTextures());
        }

        float[,] terrainHeights = terrain.terrainData.GetHeights(0, 0, terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight);
        terrain.terrainData.SetHeights(0, 0, terrainHeights);
        #endregion;
    }
}