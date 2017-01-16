using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell {

	private float height;
    private int texture = 0;
	private CellType type;

	#region Accessors;

	public float Height {
		set {
			height = value;
		}
		get {
			return height;
		}
	}

    public int Texture
    {
        get
        {
            return texture;
        }

        set
        {
            texture = value;
        }
    }

    public CellType Type {
		set {
			type = value;
		}
		get {
			return type;
		}
	}

    #endregion Accessors;
}
