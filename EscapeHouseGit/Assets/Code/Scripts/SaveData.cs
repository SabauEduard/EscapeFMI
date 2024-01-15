using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SaveData
{
    public Vector3 playerPosition;
    public Quaternion playerRotation;

    public bool isDoorLocked;
    public List<int> usedKeys;

    public float gameVolume;

}
