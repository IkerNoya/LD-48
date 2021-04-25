using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bailif : Weapon
{
    void Start()
    {
        currentAmmo = ammo;
    }

    void Update()
    {
        if (fire != null)
        {
            fire.pitch = Time.timeScale;
            fire.volume = DataManager.instance.GetSFXVolume();
        }
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
