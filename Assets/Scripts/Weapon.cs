using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Weapon : MonoBehaviour
{
    [Serializable]
    protected class WeaponData
    {
        public AmmoType ammoType;
        
        public int ammo;
        public float fireRate;
        public float damage;

        public float verticalRecoil;
        public float horizontalRecoil;

        public float range;
        public float reloadSpeed;

        public float spreadX = 0.25f;
        public float spreadY = 0.03f;

        public ParticleSystem muzzleFlash;
        public ParticleSystem hitEnvironment;
        public ParticleSystem hitEnemy;

        public AudioClip fire;
        public AudioClip reload;

    }
    [SerializeField] protected List<WeaponData> weaponData;

    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected Camera cam;
    [SerializeField] protected MouseLook mouseLook;

    [SerializeField] protected Animator anim;

    int pellets = 8;
    bool isReloading = false;
    bool isChangingWeaponMode = false;

    public enum AmmoType
    {
        shell, bullet
    };

    protected int weaponIdChoice = 0;

    protected bool canShoot = true;

    protected float shootTimer = 0;

    protected int currentAmmo;

    public static event Action<Weapon, Transform> HitDamage;

    void Start()
    {
        canShoot = false;
    }

    protected void Shoot(ref int currentAmmo)
    {
        RaycastHit hit;
        Vector3 mousePos = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(mousePos);

        switch (weaponData[weaponIdChoice].ammoType)
        {
            case AmmoType.bullet:
                audioSource.clip = weaponData[weaponIdChoice].fire;
                if (Physics.Raycast(ray, out hit, weaponData[weaponIdChoice].range))
                {
                    //hit target
                    if (hit.collider.CompareTag("Enemy"))
                    {
                        Instantiate(weaponData[weaponIdChoice].hitEnemy, hit.point, Quaternion.LookRotation(hit.normal));
                        HitDamage?.Invoke(this, hit.transform);
                    }
                    else
                    {
                        Instantiate(weaponData[weaponIdChoice].hitEnvironment, hit.point, Quaternion.LookRotation(hit.normal));
                    }
                }
                break;
            case AmmoType.shell:
                audioSource.clip = weaponData[weaponIdChoice].fire;
                Vector3 spread = Vector3.zero;
                Vector3 direction = ray.direction;
                for(int i = 0; i < pellets; i++)
                {
                    spread = new Vector3(direction.x + UnityEngine.Random.Range(-weaponData[weaponIdChoice].spreadX, weaponData[weaponIdChoice].spreadX), direction.y + UnityEngine.Random.Range(-weaponData[weaponIdChoice].spreadY, weaponData[weaponIdChoice].spreadY), direction.z);
                    if (Physics.Raycast(ray.origin, spread, out hit, weaponData[weaponIdChoice].range))
                    {
                        //hit target
                        if (hit.collider.CompareTag("Enemy"))
                        {
                            Instantiate(weaponData[weaponIdChoice].hitEnemy, hit.point, Quaternion.LookRotation(hit.normal),hit.transform);
                            HitDamage?.Invoke(this, hit.transform);
                        }
                        else
                        {
                            Instantiate(weaponData[weaponIdChoice].hitEnvironment, hit.point, Quaternion.LookRotation(hit.normal));
                        }
                    }
                }
                break;
        }

        weaponData[weaponIdChoice].muzzleFlash.Play();
        currentAmmo--;
        shootTimer = weaponData[weaponIdChoice].fireRate;
    }
    public float GetDamage()
    {
        return weaponData[weaponIdChoice].damage;
    }
    protected IEnumerator Reload(float timer)
    {
        audioSource.clip = weaponData[weaponIdChoice].reload;
        isChangingWeaponMode = true;
        canShoot = false;
        yield return new WaitForSeconds(timer);
        currentAmmo = weaponData[weaponIdChoice].ammo;
        canShoot = true;
        isChangingWeaponMode = false;
        isReloading = false;
        yield return null;
    }
    public int GetMaxAmmo()
    {
        return weaponData[weaponIdChoice].ammo;
    }
    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    protected void ChangeWeaponMode()
    {
        //horrible, but works for now
        if(Input.GetKeyDown(KeyCode.Mouse1) && weaponIdChoice == 0 && !isChangingWeaponMode)
        {
            weaponIdChoice = 1;
            StartCoroutine(Reload(1));
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1) && weaponIdChoice == 1 && !isChangingWeaponMode)
        {
            weaponIdChoice = 0;
            StartCoroutine(Reload(1));
        }
    }
    
    public void CanShootAfterSpawn()
    {
        canShoot = true;
    }
}
