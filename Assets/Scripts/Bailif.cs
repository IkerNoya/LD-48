using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bailif : Weapon
{
    void Start()
    {
        currentAmmo = ammo;
        float counterX = 0;
        float counterY = 0;
        for (int i = 0; i < 8; i++)
        {
            spreadXValues.Add(-spreadX + counterX);
            counterX += 0.1f;
        }
        for (int i = 0; i < 8; i++)
        {
            spreadYValues.Add(-spreadY + counterY);
            counterY += 0.01f;
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && canShoot && shootTimer <= 0 && currentAmmo > 0)
        {
            Shoot(ref currentAmmo);
            mouseLook.AddRecoil(verticalRecoil, Random.Range(-horizontalRecoil, horizontalRecoil));
            if (fire != null)
                fire.Play();
        }
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < ammo)
        {
            StartCoroutine(Reload(2));
            if (reload != null)
                reload.Play();
        }
        shootTimer -= Time.deltaTime;
    }

 
}
