using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    //generalize to weapons later
    [SerializeField] Weapon weapon;
    [SerializeField] Text ammo;
    [SerializeField] Image slowMotionBar;
    FPSController player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<FPSController>();
    }

    void Update()
    {
        slowMotionBar.fillAmount = player.GetSlowMotionAmmount() / 100;
        ammo.text = weapon.GetCurrentAmmo().ToString() + " / " + weapon.GetMaxAmmo().ToString();
    }
}
