using UnityEngine;
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

	private bool[,] map;
	private float[,] heights;

	private void Generate () {
		map = GenerateRoad ();
		terrainData.SetHeights (0, 0, GenerateHeights (GenerateRoad ()));
	}

	void OnDrawGizmos()
	{
		if (map != null)
		{
			for (int i = 0; i < map.GetLength(0); i++)
			{
				for (int j = 0; j < map.GetLength(1); j++)
				{
					if (map [i, j])
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
		float[,] heights = new float[(int)terrainData.size.x, (int)terrainData.size.z];
		for (int i = 0; i < heights.GetLength (0); i++) {
			for (int j = 0; j < heights.GetLength (1); j++) {
				heights [i, j] = (float)(i + j) / (float)(heights.GetLength (0) + heights.GetLength (1)) * 0.15f;
			}
		}
		terrainData.SetHeights (0, 0, heights);
		*/

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

	private float[,] GenerateHeights (bool[,] road) {
		float[,] res = new float[(int)terrainData.size.x, (int)terrainData.size.z];
		for (int i = 0; i < res.GetLength (0); i++) {
			for (int j = 0; j < res.GetLength (1); j++) {
				res [i, j] = ClosestRoad (road, i, j) / 320.0f;
			}
		}
		return res;
	}

	private float ClosestRoad (bool[,] road, int x, int y) {
		float res = 9999;
		for (int i = 0; i < road.GetLength (0); i++) {
			for (int j = 0; j < road.GetLength (1); j++) {
				if (road [i, j]) {
					float dist = Mathf.Sqrt (Mathf.Pow (i - x, 2) + Mathf.Pow (j - y, 2));
					if (dist < res)
						res = dist;
				}
			}
		}
		return res;
	}

}
