using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map {

	private Cell[,] cells;
	private IList<Elevation> elevations;

    #region Metrics;

    private int minX = 0;
    private int maxX = 500;
    private int minZ = 0;
    private int maxZ = 500;
    private int minElevations = 1;
	private int maxElevations = 1;
	private float baseElevation = 0.3f;
	private float minElevationHeight = 0.2f;
	private float maxElevationHeight = 0.7f;
	private int minElevationRadius = 10;
	private int maxElevationRadius = 100;
	private float elevationsMinGapFactor = 0.5f;

    #endregion Metrics;

    #region Generic;

    public void Initialize()
    {
        cells = CreateCells(maxX, maxZ);
    }

    public void UpdateMap(
        int minX,
        int maxX,
        int minZ,
        int maxZ,
        int minElevations,
        int maxElevations,
        float baseElevation,
        float minElevationHeight,
        float maxElevationHeight,
        int minElevationRadius,
        int maxElevationRadius,
        float elevationsMinGapFactor
    )
    {
        elevations = new List<Elevation>();

        this.minX = minX;
        this.maxX = maxX;
        this.minZ = minZ;
        this.maxZ = maxZ;
        this.minElevations = minElevations;
        this.maxElevations = maxElevations;
        this.baseElevation = baseElevation;
        this.minElevationHeight = minElevationHeight;
        this.maxElevationHeight = maxElevationHeight;
        this.minElevationRadius = minElevationRadius;
        this.maxElevationRadius = maxElevationRadius;
        this.elevationsMinGapFactor = elevationsMinGapFactor;
    }

    public Cell[,] CreateCells (int width, int height) {
		Cell[,] res = new Cell[width, height];
		for (int i = 0; i < res.GetLength (0); i++) {
			for (int j = 0; j < res.GetLength (1); j++) {
				res [i, j] = new Cell ();
                res [i, j].Height = baseElevation;
            }
		}
		return res;
	}

	public float[,] GetHeights() {
		float[,] res = new float[cells.GetLength (0), cells.GetLength (1)];
		for (int i = 0; i < res.GetLength (0); i++) {
			for (int j = 0; j < res.GetLength (1); j++) {
				res [i, j] = cells [i, j].Height;
			}
		}
		return res;
	}

    public float[,,] GetTextures()
    {
        float[,,] res = new float[cells.GetLength(0), cells.GetLength(1), Texture.NUMBER];
        for (int i = 0; i < res.GetLength(0); i++)
        {
            for (int j = 0; j < res.GetLength(1); j++)
            {
                for (int k = 0; k < res.GetLength(2); k++)
                {
                    if (cells[i, j].Texture == k) res[i, j, k] = 1.0f;
                    else res[i, j, k] = 0;
                }
            }
        }
        return res;
    }

	private float Distance (int x1, int y1, int x2, int y2) {
		return Mathf.Sqrt (Mathf.Pow (x1 - x2, 2) + Mathf.Pow (y1 - y2, 2));
	}

    #endregion Generic;

    #region Generation;

    public void Generate()
    {
        GenerateElevations();
        GenerateRoad();
        GenerateMap();
    }

    public void GenerateUpdate()
    {
        GenerateElevations();
        //GenerateRoad();
        //GenerateMap();
    }

    #region Elevations;

    private void GenerateElevations() {
		GenerateElevationsObjects ();
		GenerateElevationsHeights ();
	}

	private void GenerateElevationsObjects() {

		for (int i = 0; i < Resources.RandInt (minElevations, maxElevations); i++) {
			Elevation elevation = new Elevation ();
			elevation.Height = Resources.RandFloat (minElevationHeight, maxElevationHeight);

			bool ok = (i == 0) ? true : false;
			int x, z;
			int failLimit = 10;
			int fail = 0;
			do {
				x = Resources.RandInt (minX, maxX - 1);
				z = Resources.RandInt (minZ, maxZ - 1);
				elevation.Radius = Resources.RandInt (minElevationRadius, maxElevationRadius);

				// Verification proximite avec montagnes precedentes
				foreach (Elevation current in elevations) {
					float dist = Distance (elevation.X, elevation.Y, current.X, current.Y);
					float minDist = elevation.Radius * elevationsMinGapFactor;
					ok = dist >= minDist;
				}
				fail++;
			} while (!ok && fail < failLimit);

			if (ok) {
				elevation.X = x;
				elevation.Y = z;
				elevations.Add (elevation);
			}
		}
	}

	private void GenerateElevationsHeights() {

		float[,] heights = GetHeights ();
		foreach (Elevation elevation in elevations) {
			float[] coeffX;
			//int[] minBaseXArray = {0, minX, elevation.X - elevation.Radius};
			//int[] maxBaseXArray = { cells.GetLength (0) - 1, maxX - 1, elevation.X + elevation.Radius };
			int minBaseX = Mathf.Max (0, Mathf.Min (minX, elevation.X - elevation.Radius));
			int maxBaseX = Mathf.Min (cells.GetLength (0) - 1, Mathf.Max (maxX - 1, elevation.X + elevation.Radius));
			coeffX = CalculationPolynom (elevation.X - elevation.Radius, elevation.X + elevation.Radius, elevation.X, heights [elevation.X, elevation.Y] + elevation.Height);
			// Mise a jour dans la matrice res de la base de la montagne sur le sol
			for (int baseX = minBaseX /*Mathf.Max (minX, (elevation.X - elevation.Radius))*/; baseX <= maxBaseX /*Mathf.Min (maxX - 1, (elevation.X + elevation.Radius))*/; baseX++) {
				int[] solutionZ = {-1,-1};
				bool firstTime = true;
				for (int baseZ = elevation.Y - elevation.Radius; baseZ <= elevation.Y + elevation.Radius; baseZ++) {
					if (Distance (baseX, baseZ, elevation.X, elevation.Y) <= elevation.Radius) {
						if (firstTime) {
							firstTime = false;
							solutionZ [0] = baseZ;
						} else {
							solutionZ[1] = baseZ;
						}
					}
				}
				//int[] minBaseZArray = { 0, minZ, elevation.Y - elevation.Radius };
				//int[] maxBaseZArray = { cells.GetLength (1) - 1, maxZ - 1, elevation.Y + elevation.Radius };
				int minBaseZ = Mathf.Max (0, Mathf.Min(minZ, elevation.Y - elevation.Radius));
				int maxBaseZ = Mathf.Min (cells.GetLength (1) - 1, Mathf.Max (maxZ - 1, elevation.Y + elevation.Radius));
				for (int baseZ = minBaseZ /*Mathf.Max (minZ, (elevation.Y - elevation.Radius))*/; baseZ <= maxBaseZ /*Mathf.Min (maxZ - 1, (elevation.Y + elevation.Radius))*/; baseZ++) {
					if (Distance (baseX, baseZ, elevation.X, elevation.Y) <= elevation.Radius) {
						float[] coeffZ;
						coeffZ = CalculationPolynom (solutionZ [0], solutionZ [1], elevation.Y, DrawPolynom (coeffX [0], coeffX [1], coeffX [2], baseX));
						cells [baseX, baseZ].Height = Mathf.Max (cells [baseX, baseZ].Height, DrawPolynom (coeffZ[0], coeffZ[1], coeffZ[2], baseZ));				
					}
				}
			}
		}
	}

	private float[] CalculationPolynom (float r1, float r2, float z, float y) {
		float a = 0f;
		float b = 0f;
		float c = 0f;
		if (r2 != -1) {
			a = y / ((z - r1) * (z - r2));
			b = -2 * a * z;
			c = y + a * Mathf.Pow (z, 2);
		}
		float[] res = { a, b, c };

		return res;
	}

	private float DrawPolynom(float a, float b, float c, int x) {
		return a * Mathf.Pow (x, 2) + b * x + c;
	}
    
    #endregion Elevations;

    #region Road;

    private void GenerateRoad() {
		IList<int[]> coords = new List<int[]> ();

		coords.Add (new int[] { 30, 30 });

		foreach (int[] current in coords)
			PlaceRoad (current [0], current [1], 5);
	}

	private void PlaceRoad (int x, int y, int range) {
		for (int i = Mathf.Max (0, x - range); i < Mathf.Min (cells.GetLength (0) - 1, x + range + 1); i++) {
			for (int j = Mathf.Max (0, y - range); j < Mathf.Min (cells.GetLength (1) - 1, y + range + 1); j++) {
				cells [i, j].Type = CellType.ROAD;
			}
		}
	}

    #endregion Road;

    #region Mapping;

    private void GenerateMap () {
		float[,] heights = GetHeights ();
		for (int i = 0; i < cells.GetLength (0); i++) {
			for (int j = 0; j < cells.GetLength (1); j++) {
				if (cells [i, j].Type == CellType.ROAD)
					DigRoad (i, j, 5, 0.075f, heights);
			}
		}
	}

	private void DigRoad (int x, int y, int range, float depth, float[,] heights) {
		float maxRange = Distance (x, y, x + range, y + range);
		for (int i = Mathf.Max(0, x - range); i < Mathf.Min(cells.GetLength(0) - 1, x + range + 1); i++) {
			for (int j = Mathf.Max(0, y - range); j < Mathf.Min(cells.GetLength(1) - 1, y + range + 1); j++) {
				if (cells [i, j].Type == CellType.ROAD)
                {
                    cells[i, j].Texture = Texture.ASPHALT;
                    cells[i, j].Height = heights[i, j] - depth;
                }
				else {
                    cells[i, j].Texture = Texture.GROUND;
                    float dist = Distance (x, y, i, j);
					cells [i, j].Height = Mathf.Min (
						cells [i, j].Height,
						heights [i, j] - depth * (maxRange - dist) / maxRange
					);
				}
			}
		}
	}

    #endregion Mapping;

    #endregion Generation;
}
