using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] protected float life;
    [SerializeField] protected float maxLife;

    public bool CheckDie()
    {
        if (life <= 0)
        {
            return true;
        }

        return false;
    }

    public void ResetLife()
    {
        life = maxLife;
    }
}
