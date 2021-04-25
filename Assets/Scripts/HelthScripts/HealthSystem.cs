using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] protected float life;
    [SerializeField] protected float maxLife;
    [SerializeField] protected UnityEvent OnDie;

    public void SubstractLife(float damage) { life -= damage; }

    public bool CheckDie()
    {
        if (life <= 0)
        {
            return true;
        }

        return false;
    }

    public void CheckDieEvent()
    {
        if (life <= 0)
        {
            OnDie?.Invoke();
        }
    }

    public void ResetLife()
    {
        life = maxLife;
    }

    public float GetLife() { return life; }
}
