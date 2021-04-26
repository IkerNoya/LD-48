using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    
    void Start()
    {
        currentAmmo = ammo;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && canShoot && shootTimer<=0 && currentAmmo>0)
        {
            Shoot(ref currentAmmo);
            mouseLook.AddRecoil(verticalRecoil, Random.Range(-horizontalRecoil, horizontalRecoil));
            if (audioSource != null)
                audioSource.Play();
        }
        if(Input.GetKeyDown(KeyCode.R) && currentAmmo < ammo)
        {
            StartCoroutine(Reload(2));
            if (reload != null)
                audioSource.Play();
        }
        shootTimer -= Time.deltaTime;


    }

}
