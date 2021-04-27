﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableForBullet : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private HealthSystem healthSystem;
    FPSController player;

    void Start()
    {
        player = GetComponent<FPSController>();
    }
    void OnCollisionEnter(Collision collision)
    {
        Bullet bullet = collision.collider.GetComponent<Bullet>();
        if (bullet != null)
        {
            healthSystem.SubstractLife(bullet.GetDamage());
            healthSystem.CheckDieEvent();
            Destroy(bullet.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Bullet bullet = other.GetComponent<Bullet>();
        if (bullet != null)
        {
	    if(player != null)
               StartCoroutine(player.TakeDamage(0.2f));
            healthSystem.SubstractLife(bullet.GetDamage());
            healthSystem.CheckDieEvent();
            Destroy(bullet.gameObject);
        }
    }
}
