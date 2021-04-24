using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    //generalize to weapons later
    [SerializeField] Weapon weapon;
    [SerializeField] Text ammo;
    void Start()
    {
        
    }

    void Update()
    {
        ammo.text = weapon.GetCurrentAmmo().ToString() + " / " + weapon.GetMaxAmmo().ToString();
    }
}
