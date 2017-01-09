﻿using UnityEngine;
using System.Collections;

public class GenerationManager : MonoBehaviour {

	public Terrain terrain;
	private TerrainData terrainData;

	// Use this for initialization
	void Start () {
		terrainData = terrain.terrainData;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.A)) {
			Debug.Log ("x: " + terrainData.size.x);
			Debug.Log ("y: " + terrainData.size.y);
			Debug.Log ("z: " + terrainData.size.z);
		}
		if (Input.GetKeyUp (KeyCode.Space))
			Generate ();
	}

	private bool[,] road;
	private float[,] heights;

	private void Generate () {
		road = GenerateRoad ();
		heights = GenerateHeights ();
		terrainData.SetHeights (0, 0, GenerateTerrain (road, heights));
	}

	private bool[,] GenerateRoad () {
		bool[,] res = new bool[(int)terrainData.size.x, (int)terrainData.size.z];
		for (int i = 0; i < res.GetLength (0); i++) {
			for (int j = 0; j < res.GetLength (1); j++) {
				if (
					((i < 40 || i > terrainData.size.x - 40) || (j < 40 || j > terrainData.size.z - 40)) &&
					(i > 30 && i < terrainData.size.x - 30 && j > 30 && j < terrainData.size.z - 30))
					res [i, j] = true;
				else res [i, j] = false;
			}
		}
		return res;
	}

	private float[,] GenerateHeights () {
		float[,] res = new float[(int)terrainData.size.x, (int)terrainData.size.z];
		for (int i = 0; i < res.GetLength (0); i++) {
			for (int j = 0; j < res.GetLength (1); j++) {
				res [i, j] = 0.5f;
			}
		}
		return res;
	}

	private float[,] GenerateTerrain (bool[,] road, float[,] heights) {
		return null;
	}

	void OnDrawGizmos()
	{
		if (road != null)
		{
			for (int i = 0; i < road.GetLength(0); i++)
			{
				for (int j = 0; j < road.GetLength(1); j++)
				{
					if (road [i, j])
						Gizmos.color = Color.red;
					else
						Gizmos.color = Color.white;
					
					Vector2 pos = new Vector2(i, j);
					Gizmos.DrawCube(pos, Vector2.one);
				}
			}
		}
	}

	/*

	private IList<Vector2> outerRoads = new List<Vector2>();
	private IList<Vector2> innerRoads = new List<Vector2>();

	private bool[,] GenerateRoad () {
		bool[,] res = new bool[(int)terrainData.size.x, (int)terrainData.size.z];
		for (int i = 0; i < res.GetLength (0); i++) {
			for (int j = 0; j < res.GetLength (1); j++) {
				if (
					((i < 40 || i > terrainData.size.x - 40) || (j < 40 || j > terrainData.size.z - 40)) &&
					(i > 30 && i < terrainData.size.x - 30 && j > 30 && j < terrainData.size.z - 30)) {
					res [i, j] = true;
					if ((i == 39 || i == terrainData.size.x - 41) || (j == 39 || j == terrainData.size.z - 41) &&
					    (i > 30 && i < terrainData.size.x - 30 && j > 30 && j < terrainData.size.z - 30))
						outerRoads.Add (new Vector2 (i, j));
					else
						innerRoads.Add (new Vector2 (i, j));
				}
				else res [i, j] = false;
			}
		}
		return res;
	}

	private float[,] GenerateHeights (bool[,] road) {
		float[,] res = new float[(int)terrainData.size.x, (int)terrainData.size.z];
		for (int i = 0; i < res.GetLength (0); i++) {
			for (int j = 0; j < res.GetLength (1); j++) {
				res [i, j] = Mathf.Min(ClosestRoad (i, j) / 320.0f, 1.0f);
			}
		}
		return res;
	}

	private float ClosestRoad (int x, int y) {
		if (VectorIn(x, y, innerRoads)) return 0;
		return res;
	}

	private bool VectorIn (int x, int y, IList<Vector2> list) {
		foreach (Vector2 vector in list)
			if (vector.x == x && vector.y == y)
				return true;
			else
				return false;
	}
	*/
}