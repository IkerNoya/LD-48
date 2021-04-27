using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableForEnemys : MonoBehaviour
{
    [SerializeField] HealthSystem healthSystem;
	
    FPSController player;
    void OnEnable()
    {
        Perseguidor.OnDamagePerseguidor += TakeDamage;
    }

    void OnDisable()
    {
        Perseguidor.OnDamagePerseguidor -= TakeDamage;
    }

    void Start()
    {
        player = GetComponent<FPSController>();
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
       if(player != null)
               StartCoroutine(player.TakeDamage(0.2f));
        healthSystem.SubstractLife(damage);
        healthSystem.CheckDieEvent();
    }
}
