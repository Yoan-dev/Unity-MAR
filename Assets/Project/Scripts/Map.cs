﻿using System.Collections;
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
				res [i, res.GetLength(1) - 1 - j] = cells [i, j].Height;
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
                    if (cells[i, j].Texture == k) res[i, res.GetLength(1) - 1 - j, k] = 1.0f;
                    else res[i, res.GetLength(1) - 1 - j, k] = 0;
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
        int borders = 100,
            bordersNoise = 10,
            minTurnings = 50,
            maxTurnings = 60,
            minAmplitude = 30,
            maxAmplitude = 50,
            minZigZag = 2;
        int zigZag = 0;
        IList<int[]> coords = new List<int[]>();
        Direction[] directions = new Direction[] { Direction.East, Direction.North, Direction.West, Direction.South };
        for (int i = 0; i < directions.Length; i++)
        {
            int turning = Random.Range(minTurnings, maxTurnings + 1),
                length;
            if (i == 0) coords.Add(new int[] { borders + maxTurnings + bordersNoise * 2, borders }); // + Start
            if (i == directions.Length - 1) length = coords[coords.Count - 1][1] - coords[0][1] - turning;
            else switch (directions[i])
            {
                case Direction.East: length = cells.GetLength(0) - coords[coords.Count - 1][0] - borders - turning - Random.Range(0, bordersNoise + 1); break;
                case Direction.West: length = cells.GetLength(0) - (cells.GetLength(0) - coords[coords.Count - 1][0]) - borders - turning - Random.Range(0, bordersNoise + 1); break;
                case Direction.North: length = cells.GetLength(1) - coords[coords.Count - 1][1] - borders - turning - Random.Range(0, bordersNoise + 1); break;
                default: length = cells.GetLength(1) - (cells.GetLength(1) - coords[coords.Count - 1][1]) - borders - turning - Random.Range(0, bordersNoise + 1); break;
            }
            int sectionsCount = 1;//
            for (int j = 0; j < sectionsCount; j++)
            {
                Debug.Log(((directions.Length - i) +" <= "+ (minZigZag - zigZag)));
                if ((directions.Length - i <= minZigZag - zigZag) || Random.Range(0, 2) == 0)
                {
                    zigZag++;
                    coords = ZigZag(coords, directions[i], Random.Range(1, length / sectionsCount / 50), length / sectionsCount);
                }
                else coords = Generic(coords, directions[i], length / sectionsCount, Random.Range(minAmplitude, maxAmplitude));
            }
            coords = Turning(coords, directions[i], directions[(i + 1) % directions.Length], turning);
        }
        coords = LinkPoints(coords, coords[0][0], coords[0][1], coords[coords.Count - 1][0], coords[coords.Count - 1][1]);
        foreach (int[] current in coords)
			PlaceRoad (current [0], current [1], 5);
	}

    private IList<int[]> Turning(IList<int[]>  coords, Direction direction1, Direction direction2, int length)
    {
        length = length * 7 / 5;//
        int x = coords[coords.Count - 1][0],
            y = coords[coords.Count - 1][1];
        for (int i = 0; i < length; i++)
        {
            int tempX = 0, tempY = 0;
            float turning = Mathf.Abs((float)length / 2 - i) / (length / 2);
            if (i < length / 2)
            {
                tempX++;
                if (turning < 0.75f && (i % 2 == 0 || turning < 0.1f)) tempY++;
            }
            else
            {
                tempY++;
                if (turning < 0.75f && (i % 2 == 0 || turning < 0.1f)) tempX++;
            }
            if (direction1 == Direction.North)
            {
                y += tempX;
                if (direction2 == Direction.East) x += tempY;
                else if (direction2 == Direction.West) x -= tempY;
            }
            else if (direction1 == Direction.South)
            {
                y -= tempX;
                if (direction2 == Direction.East) x += tempY;
                else if (direction2 == Direction.West) x -= tempY;
            }
            else if (direction1 == Direction.East)
            {
                x += tempX;
                if (direction2 == Direction.North) y += tempY;
                else if (direction2 == Direction.South) y -= tempY;
            }
            else if (direction1 == Direction.West)
            {
                x -= tempX;
                if (direction2 == Direction.North) y += tempY;
                else if (direction2 == Direction.South) y -= tempY;
            }
            coords.Add(new int[] { x, y });
        }
        return coords;
    }

    private IList<int[]> Straight (IList<int[]> coords, Direction direction, int length)
    {
        int x = coords[coords.Count - 1][0],
            y = coords[coords.Count - 1][1];
        for (int i = 0; i < length; i++)
        {
            switch (direction)
            {
                case Direction.North: y++; break;
                case Direction.South: y--; break;
                case Direction.West: x--; break;
                case Direction.East: x++; break;
            }
            coords.Add(new int[] { x, y });
        }
        return coords;
    }

    private IList<int[]> ZigZag (IList<int[]> coords, Direction direction, int number, int length)
    {
        length = (int)(length * 5.7f / 5);//
        Direction d1;
        Direction d2;
        Direction d3;
        switch (direction)
        {
            case Direction.East:
                d1 = Direction.East;
                d2 = Direction.North;
                d3 = Direction.South;
                break;
            case Direction.West:
                d1 = Direction.West;
                d2 = Direction.South;
                d3 = Direction.North;
                break;
            case Direction.North:
                d1 = Direction.North;
                d2 = Direction.West;
                d3 = Direction.East;
                break;
            default:
                d1 = Direction.South;
                d2 = Direction.East;
                d3 = Direction.West;
                break;
        }
        coords = Turning(coords, d1, d2, length / (5 * number + 2));
        for (int i = 0; i < number; i++)
        {
            coords = Turning(coords, d2, d1, length / (5 * number + 2));
            coords = Turning(coords, d1, d3, length / (5 * number + 2));
            coords = Straight(coords, d3, length / 5);
            coords = Turning(coords, d3, d1, length / (5 * number + 2));
            coords = Turning(coords, d1, d2, length / (5 * number + 2));
            if (i < number - 1) coords = Straight(coords, d2, length / 5);
        }
        coords = Turning(coords, d2, d1, length / 6);
        return coords;
    }

    private IList<int[]> Generic(IList<int[]> coords, Direction direction, int length, int amplitude)
    {
        int x = coords[coords.Count - 1][0],
            y = coords[coords.Count - 1][1],
            oX = x, oY = y;
        for (int i = 0; i < length; i++)
        {
            switch (direction)
            {
                case Direction.North: y++; break;
                case Direction.South: y--; break;
                case Direction.West: x--; break;
                case Direction.East: x++; break;
            }
            if (direction == Direction.North || direction == Direction.South)
                x += Random.Range((int)((oX - amplitude < x) ? -1 : 0), (int)((oX + amplitude > x) ? 2 : 1));
            else
                y += Random.Range((int)((oY - amplitude < y) ? -1 : 0), (int)((oY + amplitude > y) ? 2 : 1));
            coords.Add(new int[] { x, y });
        }
        return coords;
    }

    private IList<int[]> LinkPoints(IList<int[]> coords, int x1, int y1, int x2, int y2)
    {
        while (x1 != x2 || y1 != y2)
        {
            if (x1 > x2) x1--;
            else if (x1 < x2) x1++;
            if (y1 > y2) y1--;
            else if (y1 < y2) y1++;
            coords.Add(new int[] { x1, y1 });
        }
        return coords;
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
