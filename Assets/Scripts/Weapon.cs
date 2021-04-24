using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected int ammo;

    [SerializeField] protected float fireRate;
    [SerializeField] protected float damage;
    [Space]
    [SerializeField] protected float verticalRecoil;
    [SerializeField] protected float horizontalRecoil;
    [Space]
    [SerializeField] protected float range;
    [Space]
    [SerializeField] protected ParticleSystem muzzleFlash;
    [SerializeField] protected ParticleSystem hitEnvironment;
    [SerializeField] protected ParticleSystem hitEnemy;
    [SerializeField] protected Camera cam;
    [Space]
    [SerializeField] protected AudioSource fire;
    [SerializeField] protected AudioSource reload;

    protected bool canShoot = true;

    protected float shootTimer = 0;

    public static event Action<Weapon, Transform> HitDamage;

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
                HitDamage?.Invoke(this, hit.transform);
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
    protected void Recoil(float recoilValue)
    {

    }
    public float GetDamage()
    {
        return damage;
    }
}
