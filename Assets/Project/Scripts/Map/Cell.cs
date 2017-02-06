public class Cell {

	private float height;
    private int texture = 0;
	private CellType type = CellType.GRASS;
    private bool tree = false;

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

    public bool Tree
    {
        get
        {
            return tree;
        }

        set
        {
            tree = value;
        }
    }

    #endregion Accessors;
}
