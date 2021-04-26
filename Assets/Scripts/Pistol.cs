using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    
    void Start()
    {
        currentAmmo = weaponData[weaponIdChoice].ammo;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && canShoot && shootTimer<=0 && currentAmmo>0)
        {
            Shoot(ref currentAmmo);
            mouseLook.AddRecoil(weaponData[weaponIdChoice].verticalRecoil, Random.Range(-weaponData[weaponIdChoice].horizontalRecoil, weaponData[weaponIdChoice].horizontalRecoil));
            if (audioSource != null)
                audioSource.Play();
        }
        if(Input.GetKeyDown(KeyCode.R) && currentAmmo < weaponData[weaponIdChoice].ammo)
        {
            StartCoroutine(Reload(weaponData[weaponIdChoice].reloadSpeed));
            if (weaponData[weaponIdChoice].reload != null)
                audioSource.Play();
        }
        shootTimer -= Time.deltaTime;


    }

}
