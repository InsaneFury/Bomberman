using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    public Vector3 start;
    public Vector3 offset;
    [Header("Prefabs")]
    public GameObject wall;
    public GameObject innerWall;
    public GameObject destructableWall;
    public GameObject floor;
    public GameObject portal;
    [Header("PlaceHolder")]
    public Transform outerWallHolder;
    public Transform innerWallHolder;
    public Transform destructablesHolder;
    [Header("Set Grid Size > 5")]
    public int gridSizeX;
    public int gridSizeZ;
    [Header("Amount of random destructables walls")]
    [Tooltip("Examples 1 = 100% | 0,50 = 50% | 0,25 = 25%")]
    public float percent;
    [Header("LayerMask")]
    public LayerMask layermask;
}
