using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionGeneratorBasedMyPosition : MonoBehaviour
{
    // Start is called before the first frame update

    public float minPositionX;
    public float maxPositionX;

    public float minPositionZ;
    public float maxPositionZ;

    public float heigthGenerateRayCast;
    public float rangeRayCast = 1000;

    [SerializeField] private string nameTagEnvarioment = "Environment";

    public Vector3 GetPositionGenerated()
    {
        float X = Random.Range(minPositionX, maxPositionX);
        float Z = Random.Range(minPositionZ, maxPositionZ);

        RaycastHit hit;

        Vector3 positonRayCast = new Vector3(X, heigthGenerateRayCast, Z);

        Vector3 positionReturns = Vector3.zero;

        float Y = 0;

        if (Physics.Raycast(positonRayCast, Vector3.down, out hit, rangeRayCast))
        {
            Y = hit.point.y;
            positionReturns = new Vector3(X, Y, Z);           
        }

        return positionReturns;
    }
}
