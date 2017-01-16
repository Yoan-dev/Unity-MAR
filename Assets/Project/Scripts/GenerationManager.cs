using UnityEngine;
using System.Collections;

public class GenerationManager : MonoBehaviour {

	public Terrain terrain;
	private TerrainData terrainData;

	#region Metrics;

	public int minElevations = 2;
	public int maxElevations = 100;
	public float baseElevation = 0.3f;
	public float minElevationHeight = 0.2f;
	public float maxElevationHeight = 0.7f;
	public int minElevationRadius = 10;
	public int maxElevationRadius = 100;
	public float elevationsMinGapFactor = 0.5f;
	public int seed = 0;
	public bool useSeed = false;

	#endregion Metrics;

	// Use this for initialization
	void Start () {
		terrainData = terrain.terrainData;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.Space))
			Generate ();
	}

	private void Generate () {
		Map map = new Map ();
		map.Initialize (
			(int)terrainData.size.x, 
			(int)terrainData.size.z,
			minElevations,
			maxElevations,
			baseElevation,
			minElevationHeight,
			maxElevationHeight,
			minElevationRadius,
			maxElevationRadius,
			elevationsMinGapFactor
		);
		if (useSeed)
			Random.seed = seed;
		Debug.Log ("Seed: " + Random.seed);
		map.Generate ();
		terrainData.SetHeights (0, 0, map.GetHeights());
        terrainData.SetAlphamaps(0, 0, map.GetTextures());
	}
}
