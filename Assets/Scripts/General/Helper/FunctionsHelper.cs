using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionsHelper : MonoBehaviour {
    public static Vector2 RandomVector2() {
        return RandomVector2(Mathf.PI * 2f, 0);
    }
    public static Vector2 RandomVector2(float angle, float angleMin) {
        float random = Random.value * angle + angleMin;
        return new Vector2(Mathf.Cos(random), Mathf.Sin(random));
    }
}