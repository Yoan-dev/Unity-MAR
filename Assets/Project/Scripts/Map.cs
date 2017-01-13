using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map {

	private Cell[,] cells;
	IList<Elevation> elevations;

	#region Metrics;

	private int minElevations = 2;
	private int maxElevations = 100;
	private float baseElevation = 0.3f;
	private float minElevationHeight = 0.2f;
	private float maxElevationHeight = 0.7f;
	private int minElevationRadius = 10;
	private int maxElevationRadius = 100;
	private float elevationsMinGapFactor = 0.5f;

	#endregion Metrics;

	#region Generic;

	public void Initialize (
		int width, 
		int height,
		int minElevations,
		int maxElevations,
		float baseElevation,
		float minElevationHeight,
		float maxElevationHeight,
		int minElevationRadius,
		int maxElevationRadius,
		float elevationsMinGapFactor
	) {
		cells = CreateCells (width, height);
		elevations = new List<Elevation>();

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

	private float Distance (int x1, int y1, int x2, int y2) {
		return Mathf.Sqrt (Mathf.Pow (x1 - x2, 2) + Mathf.Pow (y1 - y2, 2));
	}

	#endregion Generic;

	#region Generation;

	public void Generate () {
		GenerateElevations ();
		GenerateRoad ();
		GenerateMap ();
	}

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
				x = Resources.RandInt (0, cells.GetLength (0) - 1);
				z = Resources.RandInt (0, cells.GetLength (1) - 1);
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
		for (int i = 0; i < cells.GetLength (0); i++) {
			for (int j = 0; j < cells.GetLength (1); j++) {
				cells [i, j].Height = baseElevation;
			}
		}

		float[,] heights = GetHeights ();
		foreach (Elevation elevation in elevations) {
			// Mise a jour dans la matrice res de la base de la montagne sur le sol
			for (int baseX = Mathf.Max (0, (elevation.X - elevation.Radius)); baseX <= Mathf.Min (cells.GetLength(0) - 1, (elevation.X + elevation.Radius)); baseX++) {
				for (int baseZ = Mathf.Max (0, (elevation.Y - elevation.Radius)); baseZ <= Mathf.Min (cells.GetLength(1) - 1, (elevation.Y + elevation.Radius)); baseZ++) {
					if(Distance(baseX, baseZ, elevation.X, elevation.Y) <= elevation.Radius)
						cells[baseX, baseZ].Height = Mathf.Max(cells[baseX, baseZ].Height, heights[baseX, baseZ] + /**/elevation.Height/**/);
				}
			}
		}
	}

	private void GenerateRoad() {
		for (int i = 0; i < cells.GetLength (0); i++) {
			for (int j = 0; j < cells.GetLength (1); j++) {
				if (
					((i < 50 || i > cells.GetLength (0) - 50) || (j < 50 || j > cells.GetLength (1) - 50)) &&
					(i > 30 && i < cells.GetLength (0) - 30 && j > 30 && j < cells.GetLength (1) - 30))
					cells [i, j].Type = CellType.ROAD;
				else cells [i, j].Type = CellType.GRASS;
			}
		}
	}

	private void GenerateMap () {
		float[,] heights = GetHeights ();
		for (int i = 0; i < cells.GetLength (0); i++) {
			for (int j = 0; j < cells.GetLength (1); j++) {
				if (cells [i, j].Type == CellType.ROAD)
					PlaceRoad (i, j, 5, 0.075f, heights);
			}
		}
	}

	private void PlaceRoad (int x, int y, int range, float depth, float[,] heights) {
		float maxRange = Distance (x, y, x + range, y + range);
		for (int i = Mathf.Max(0, x - range); i < Mathf.Min(cells.GetLength(0) - 1, x + range + 1); i++) {
			for (int j = Mathf.Max(0, y - range); j < Mathf.Min(cells.GetLength(1) - 1, y + range + 1); j++) {
				if (cells [i, j].Type == CellType.ROAD)
					cells [i, j].Height = heights[i, j] - depth;
				else {
					float dist = Distance (x, y, i, j);
					cells [i, j].Height = Mathf.Min (
						cells [i, j].Height,
						heights [i, j] - depth * (maxRange - dist) / maxRange
					);
				}
			}
		}
	}

	#endregion Generation;
}
