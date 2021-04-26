using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bailif : Weapon
{
    void Start()
    {
        currentAmmo = weaponData[weaponIdChoice].ammo;
    }

    void Update()
    {
        if (audioSource != null)
        {
            audioSource.pitch = Time.timeScale;
            audioSource.volume = DataManager.instance.GetSFXVolume();
        }
        if (Input.GetKey(KeyCode.Mouse0) && canShoot && shootTimer <= 0 && currentAmmo > 0)
        {
            anim.SetTrigger("Shoot");
            Shoot(ref currentAmmo);
            mouseLook.AddRecoil(weaponData[weaponIdChoice].verticalRecoil, Random.Range(-weaponData[weaponIdChoice].horizontalRecoil, weaponData[weaponIdChoice].horizontalRecoil));
            if (audioSource != null)
                audioSource.Play();
        }
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < weaponData[weaponIdChoice].ammo)
        {
            StartCoroutine(Reload(weaponData[weaponIdChoice].reloadSpeed));
            if (audioSource != null)
                audioSource.Play();
            
        }
        shootTimer -= Time.deltaTime;
        ChangeWeaponMode();
    }

}
