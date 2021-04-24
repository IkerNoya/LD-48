using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    int currentAmmo;
    
    void Start()
    {
        currentAmmo = ammo;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && canShoot && shootTimer<=0)
        {
            Shoot(ref currentAmmo);
        }
        if(Input.GetKeyDown(KeyCode.R) && currentAmmo < ammo)
        {
            StartCoroutine(Reload(2));
        }
        shootTimer -= Time.deltaTime;
    }
    IEnumerator Reload(float timer)
    {
        canShoot = false;
        yield return new WaitForSeconds(timer);
        currentAmmo = ammo;
        canShoot = true;
        yield return null;
    }
    public int GetMaxAmmo()
    {
        return ammo;
    }
    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }
}
