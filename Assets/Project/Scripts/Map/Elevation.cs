using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevation {

	private int x;
	private int y;
	private float height;
	private int radius;

	#region Accessors;

	public int X {
		set {
			x = value;
		}
		get {
			return x;
		}
	}

	public int Y {
		set {
			y = value;
		}
		get {
			return y;
		}
	}

	public float Height {
		set {
			height = value;
		}
		get {
			return height;
		}
	}

	public int Radius {
		set {
			radius = value;
		}
		get {
			return radius;
		}
	}

	#endregion Accessors;

}
