using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyble : MonoBehaviour
{
    public void DestroyObject(float time)
    {
        Destroy(gameObject, time);
    }
}
