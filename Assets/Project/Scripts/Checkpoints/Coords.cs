using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coords {

    private Vector3 position;
    private Vector3 rotation;

    #region Accessors;

    public Vector3 Position
    {
        get
        {
            return position;
        }

        set
        {
            position = value;
        }
    }

    public Vector3 Rotation
    {
        get
        {
            return rotation;
        }

        set
        {
            rotation = value;
        }
    }

    #endregion Accessors;
}
