using UnityEngine;
using System.Collections;

public class GenerationManager : MonoBehaviour {

	public Terrain terrain;

	//[Range(1, 100)]
	public int maxMoutain;
	public int maxRadiusMoutain;

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
		terrainData.SetHeights (0, 0, GenerateTerrain ());
	}

	private bool[,] GenerateRoad () {
		bool[,] res = new bool[(int)terrainData.size.x, (int)terrainData.size.z];
		for (int i = 0; i < res.GetLength (0); i++) {
			for (int j = 0; j < res.GetLength (1); j++) {
				if (
					((i < 50 || i > terrainData.size.x - 50) || (j < 50 || j > terrainData.size.z - 50)) &&
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
				res [i, j] = 0.3f;
			}
		}

		// Nombre de montagnes
		int nbMount = Random.Range (2, maxMoutain);

		// Hauteur de chaque montagne
		float[,] mounts = new float[nbMount, 3];
		for (int i = 0; i < mounts.GetLength (0); i++) {
			mounts [i,0] = Random.Range (0.5f, 1.0f);
		}

		// Position sur le terrain de chaque montagne
		for (int i = 0; i < mounts.GetLength (0); i++) {
			bool ok = true;
			int x;
			int z;
			int radius;

			do {
				x = (int) Random.Range (0, terrainData.size.x - 1);
				z = (int) Random.Range (0, terrainData.size.z - 1);
				radius = (int) Random.Range (10, maxRadiusMoutain);

				if (i == 0)
					ok = false;

				// Verification proximite avec montagnes precedentes
				for (int j = 0; j < i; j++) {
					float dist = Mathf.Sqrt (Mathf.Pow ((mounts [j, 1] - mounts [i, 1]), 2) + Mathf.Pow ((mounts [j, 2] - mounts [i, 2]), 2));
					float distMin = radius*0.5f;
					if(dist >= distMin)
						ok = false;
				}
			} while (ok);
				
			// Mise a jour dans la matrice res du pic central des montagnes
			mounts [i, 1] = x;
			mounts [i, 2] = z;
			res [x, z] = mounts [i, 0];

			// Mise a jour dans la matrice res de la base de la montagne sur le sol
			for (int baseX = Mathf.Max (0, (x - radius)); baseX <= Mathf.Min (terrainData.size.x - 1, (x + radius)); baseX++) {
				for (int baseZ = Mathf.Max (0, (z - radius)); baseZ <= Mathf.Min (terrainData.size.z - 1, (z + radius)); baseZ++) {
					if((Mathf.Pow ((baseX - x), 2) + Mathf.Pow ((baseZ - z), 2)) <= Mathf.Pow(radius, 2))
						res [baseX, baseZ] = Mathf.Max(res [baseX, baseZ], mounts [i, 0]);
				}
			}
		}

		return res;
	}

	private float[,] GenerateTerrain () {
		float[,] res = new float[(int)terrainData.size.x, (int)terrainData.size.z];

		for (int i = 0; i < res.GetLength (0); i++) {
			for (int j = 0; j < res.GetLength (1); j++) {
				res [i, j] = heights [i, j];
			}
		}

		for (int i = 0; i < res.GetLength (0); i++) {
			for (int j = 0; j < res.GetLength (1); j++) {
				if (road [i, j])
					res = PlaceRoad (i, j, 5, 0.075f, res);
			}
		}

		return res;
	}

	private float[,] PlaceRoad (int x, int y, int range, float depth, float[,] res) {
		float maxRange = Distance (x, y, x + range, y + range);
		for (int i = Mathf.Max(0, x - range); i < Mathf.Min(res.GetLength(0) - 1, x + range + 1); i++) {
			for (int j = Mathf.Max(0, y - range); j < Mathf.Min(res.GetLength(1) - 1, y + range + 1); j++) {
				if (road [i, j])
					res [i, j] = heights [i, j] - depth;
				else {
					float dist = Distance (x, y, i, j);
					res [i, j] = Mathf.Min (
						res [i, j],
						heights [i, j] - depth * (maxRange - dist) / maxRange
					);
				}
			}
		}
		return res;
	}

	private float Distance (int x1, int y1, int x2, int y2) {
		return Mathf.Sqrt (Mathf.Pow (x1 - x2, 2) + Mathf.Pow (y1 - y2, 2));
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
}
