using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected int ammo;

    [SerializeField] protected float fireRate;
    [SerializeField] protected float damage;
    [SerializeField] protected float recoil;
    [SerializeField] protected float range;
    [SerializeField] protected ParticleSystem muzzleFlash;
    [SerializeField] protected ParticleSystem hitEnvironment;
    [SerializeField] protected ParticleSystem hitEnemy;
    [SerializeField] protected Camera cam;
    protected bool canShoot = true;

    protected float shootTimer = 0;

    public static event Action<Weapon> HitDamage;

    protected void Shoot(ref int currentAmmo)
    {
        RaycastHit hit;
        Vector3 mousePos = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(mousePos);
        Debug.DrawRay(ray.origin, ray.direction * range, Color.red);
        if (Physics.Raycast(ray, out hit, range))
        {
            //hit target
            if (hit.collider.CompareTag("Enemy"))
            {
                Instantiate(hitEnemy, hit.point, Quaternion.LookRotation(hit.normal));
                HitDamage?.Invoke(this);
            }
            else
            {
                Instantiate(hitEnvironment, hit.point, Quaternion.LookRotation(hit.normal));                
            }
        }
        muzzleFlash.Play();
        currentAmmo--;
        shootTimer = fireRate;
    }
}
