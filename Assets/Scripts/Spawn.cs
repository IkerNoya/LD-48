using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField]public float minRangeX;
    [SerializeField]public float maxRangeX;
    [SerializeField]public float minRangeZ;
    [SerializeField]public float maxRangeZ;

    [SerializeField][Range(1,10)] public float valueRange;
    private void Awake()
    {
        minRangeX = transform.position.x - valueRange;
        maxRangeX = transform.position.x + valueRange;
        minRangeZ = transform.position.z - valueRange;
        maxRangeZ = transform.position.z + valueRange;
    }
    public float GetMinX()
    {
        return minRangeX;
    }
    public float GetMaxX()
    {
        return maxRangeX;
    }
    public float GetMinZ()
    {
        return minRangeZ;
    }
    public float GetMaxZ()
    {
        return maxRangeZ;
    }
}
