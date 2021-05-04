using UnityEngine;
//true for Odd, false for even

public class Vector3Bool {
    private bool _x;
    public bool X {
        get { return _x; }
        set { _x = value; }
    }
    private bool _y;
    public bool Y {
        get { return _y; }
        set { _y = value; }
    }
    private bool _z;
    public bool Z {
        get { return _z; }
        set { _z = value; }
    }
    public Vector3Bool(bool x, bool y, bool z) {
        _x = x;
        _y = y;
        _z = z;
    }
    public Vector3Bool(Vector3 vec3) {
        if (vec3.x % 2 == 0) {
            X = false;
        } else {
            X = true;
        }
        if (vec3.y % 2 == 0) {
            Y = false;
        } else {
            Y = true;
        }
        if (vec3.z % 2 == 0) {
            Z = false;
        } else {
            Z = true;
        }
    }

}