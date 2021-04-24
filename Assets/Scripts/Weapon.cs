using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected int ammo;

    [SerializeField] protected float fireRate;
    [SerializeField] protected float damage;
    [SerializeField] protected float recoil;
    [SerializeField] protected float range;

    protected bool canShoot = true;

    protected float shootTimer = 0;

    protected void Shoot(ref int currentAmmo)
    {
        Ray ray;
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Input.mousePosition, out hit, range))
        {
            //hit target
            if (hit.collider.CompareTag("enemy"))
            {
                //blood particle
                //damage
            }
            else
            {
                //spark particle
            }
        }
        currentAmmo--;
        shootTimer = fireRate;
        Debug.Log("Pew Pew");
    }
}
