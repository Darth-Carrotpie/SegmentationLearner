using System;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable 0414
//idea for upgrade: this could be composite, made out of generic MessagePage or smth like it, whithin which would contain isSet states and other things if needed;
public class GameMessage : BaseMessage {
    public static GameMessage Write() {
        return new GameMessage();
    }
    //example how could handle empty messages better
    private string _strMessage;
    public string strMessage {
        get {
            if (strMessageSet)
                return _strMessage;
            else throw new Exception("No strMessage was set before request for GameMessage: " + this);
        }
        set { _strMessage = value; }
    } //must not be type of bool (if bool needed, use int)
    private bool strMessageSet; // must be private bool
    public GameMessage WithStringMessage(string value) {
        strMessage = value;
        strMessageSet = true;
        return this;
    }

    public Vector2 coordinates;
    private bool coordinatesSet;
    public GameMessage WithCoordinates(Vector2 value) {
        coordinates = value;
        coordinatesSet = true;
        return this;
    }

    public Transform transform;
    private bool transformSet;
    public GameMessage WithTransform(Transform value) {
        transform = value;
        transformSet = true;
        return this;
    }

    public GameObject gameObject;
    private bool gameObjectSet;
    public GameMessage WithGameObject(GameObject value) {
        gameObject = value;
        gameObjectSet = true;
        return this;
    }
}