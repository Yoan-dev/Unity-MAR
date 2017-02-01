using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerationManager : MonoBehaviour
{

    public Terrain terrain;
    private TerrainData terrainData;

    #region Prefabs;

    public GameObject start;
    public GameObject checkpoint;
    public GameObject zigzagCamera;
    public GameObject genericCamera;
    public GameObject[] trees;

    #endregion Prefabs;

    #region Metrics;
    
    public int seed = 0;
    public bool useSeed = false;

    #endregion Metrics;
    
    void Start()
    {
        terrainData = terrain.terrainData;
    }

    public void Generate()
    {
        Map map = new Map();
		map.Initialize ();
        if (useSeed)
            Random.seed = seed;
        Debug.Log("Seed: " + Random.seed);
        map.Generate();
        terrainData.SetHeights(0, 0, map.GetHeights());
        terrainData.SetAlphamaps(0, 0, map.GetTextures());

        InstantiateTrees(map);
        InstantiateCheckpoints(map);
        InstantiateCameras(map);
    }

    private void InstantiateTrees(Map map)
    {
        for (int i = 0; i < GameObject.Find("Decorations").transform.childCount; i++)
            Destroy(GameObject.Find("Decorations").transform.GetChild(i).gameObject);
        bool[,] trees = map.GetTrees();
        for (int i = 0; i < trees.GetLength(0); i++)
        {
            for (int j = 0; j < trees.GetLength(1); j++)
            {
                if (trees[i, j])
                {
                    GameObject toInstantiate = this.trees[Random.Range(0, this.trees.Length)];
                    Instantiate(toInstantiate, new Vector3(j, terrainData.GetHeight(j, i), i), Quaternion.identity, GameObject.Find("Decorations").transform);
                }
            }
        }
    }

    private void InstantiateCheckpoints(Map map)
    {
        for (int i = 0; i < GameObject.Find("CheckpointsManager").transform.childCount; i++)
            Destroy(GameObject.Find("CheckpointsManager").transform.GetChild(i).gameObject);
        Instantiate(start,
            new Vector3(
            map.GetStartingPosition()[1],
            10,
            map.GetStartingPosition()[0]),
            Quaternion.identity, GameObject.Find("CheckpointsManager").transform);
        foreach (int[] coords in map.GetCheckpoints())
        {
            Instantiate(checkpoint,
                new Vector3(
                coords[1],
                10,
                coords[0]),
                Quaternion.identity, GameObject.Find("CheckpointsManager").transform);
        }
    }

    private void InstantiateCameras(Map map)
    {
        for (int i = 0; i < GameObject.Find("ReplayCamerasManager").transform.childCount; i++)
            Destroy(GameObject.Find("ReplayCamerasManager").transform.GetChild(i).gameObject);
        foreach (int[] coords in map.GetCameras().Keys)
        {
            if (map.GetCameras()[coords] == "zigzag")
                Instantiate(zigzagCamera, new Vector3(coords[1], 75, coords[0]), Quaternion.identity, GameObject.Find("ReplayCamerasManager").transform);
            else if (map.GetCameras()[coords] == "generic")
                Instantiate(genericCamera, new Vector3(coords[1], 12, coords[0]), Quaternion.identity, GameObject.Find("ReplayCamerasManager").transform);

        }
    }
}