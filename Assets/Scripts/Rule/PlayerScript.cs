using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerScript 
{
    public List<CardScript> cardList = new List<CardScript>();
    public Transform[] cardTrs;
}

[Serializable]
public class PRS
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    public PRS(Vector3 pos, Quaternion quat, Vector3 scl)
    {
        position = pos;
        rotation = quat;
        scale = scl;
    }
}
