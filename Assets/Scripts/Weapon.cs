using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Weapon : MonoBehaviour
{
    [SerializeField] AmmoType ammoType;
    [Space]
    [SerializeField] protected int ammo;
    [SerializeField] protected float fireRate;
    [SerializeField] protected float damage;
    [Space]
    [SerializeField] protected float verticalRecoil;
    [SerializeField] protected float horizontalRecoil;
    [Space]
    [SerializeField] protected float range;
    [Space]
    [SerializeField] protected float spreadX = 0.25f;
    [SerializeField] protected float spreadY = 0.03f;
    [Space]
    [SerializeField] protected ParticleSystem muzzleFlash;
    [SerializeField] protected ParticleSystem hitEnvironment;
    [SerializeField] protected ParticleSystem hitEnemy;
    [SerializeField] protected Camera cam;
    [SerializeField] protected MouseLook mouseLook;
    [Space]
    [SerializeField] protected AudioSource fire;
    [SerializeField] protected AudioSource reload;

    int pellets = 8;

    enum AmmoType
    {
        shell, bullet
    };

    protected bool canShoot = true;

    protected float shootTimer = 0;

    protected int currentAmmo;

    public static event Action<Weapon, Transform> HitDamage;


    protected void Shoot(ref int currentAmmo)
    {
        RaycastHit hit;
        Vector3 mousePos = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(mousePos);

        switch (ammoType)
        {
            case AmmoType.bullet:
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
                break;
            case AmmoType.shell:
                Vector3 spread = Vector3.zero;
                Vector3 direction = ray.direction;
                for(int i = 0; i < pellets; i++)
                {
                    spread = new Vector3(direction.x + UnityEngine.Random.Range(-spreadX, spreadX), direction.y + UnityEngine.Random.Range(-spreadY, spreadY), direction.z);
                    if (Physics.Raycast(ray.origin, spread, out hit, range))
                    {
                        //hit target
                        if (hit.collider.CompareTag("Enemy"))
                        {
                            Instantiate(hitEnemy, hit.point, Quaternion.LookRotation(hit.normal),hit.transform);
                            HitDamage?.Invoke(this, hit.transform);
                        }
                        else
                        {
                            Instantiate(hitEnvironment, hit.point, Quaternion.LookRotation(hit.normal));
                        }
                    }
                }
                break;
        }
        
        muzzleFlash.Play();
        currentAmmo--;
        shootTimer = fireRate;
    }
    public float GetDamage()
    {
        return damage;
    }
    protected IEnumerator Reload(float timer)
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
