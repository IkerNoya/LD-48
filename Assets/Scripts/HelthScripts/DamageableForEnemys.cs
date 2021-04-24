using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableForEnemys : MonoBehaviour
{
    [SerializeField] HealthSystem healthSystem;
    void OnEnable()
    {
        Perseguidor.OnDamagePerseguidor += TakeDamage;
    }

    void OnDisable()
    {
        Perseguidor.OnDamagePerseguidor -= TakeDamage;
    }

    void TakeDamage(float damage, Transform targetTransform)
    {
        if (targetTransform = transform)
        {
            TakeDamage(damage);
        }
    }

    public void TakeDamage(float damage)
    {
        healthSystem.SubstractLife(damage);
        healthSystem.CheckDieEvent();
    }
}
