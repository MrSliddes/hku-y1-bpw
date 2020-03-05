﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Vars")]
    public bool isEquipped = false;
    public bool isAutomatic;
    public int damage = 1;
    public float fireRate = 1;
    private float fireRateTimer;
    public int ammoMagazineCurrent;
    public int ammoMagazineMax = 1;
    public int ammoCurrent;
    public int ammoMax = 1;
    public float reloadTime = 1;
    public float weaponRange = 1000;

    public Transform barrelEnd;

    private bool releasedTrigger = true;
    private bool isReloading = false;

    private void Start()
    {
        // Set vars
        fireRateTimer = fireRate;
        // Reload weapon
        ammoMagazineCurrent = ammoMagazineMax;
    }

    private void Update()
    {
        CheckTrigger();

        // Reloading
        if(Input.GetButtonDown("Input R"))
        {
            if(!isReloading)
            {
                StartCoroutine(ReloadMagazine());
            }
        }
    }

    public virtual void CheckTrigger()
    {
        // Check for player input
        if(isEquipped)
        {
            // Is the weapon fully automatic
            if(Input.GetAxis("Fire1") == 1)
            {
                FireWeapon();
                Debug.Log("Hit Trigger");
            }
            else
            {
                releasedTrigger = true;
            }
        }
    }

    public virtual void FireWeapon()
    {
        // Check firemode
        if(isAutomatic)
        {
            // Check ammo
            if(ammoMagazineCurrent >= 1)
            {
                if(fireRateTimer <= 0)
                {
                    // Fire
                    FireBullet();
                    ammoMagazineCurrent--;
                    // Reset
                    fireRateTimer = fireRate;
                }
                else
                {
                    fireRateTimer -= Time.deltaTime;
                }
            }
        }
        else
        {
            // Check ammo
            if (ammoMagazineCurrent >= 1)
            {
                if (releasedTrigger)
                {
                    // Fire
                    FireBullet();
                    ammoMagazineCurrent--;
                    // Reset
                    releasedTrigger = false;
                }
            }
        }
    }

    public virtual void FireBullet()
    {
        Debug.Log("Pew!");
        RaycastHit hit;

        // Check if our raycast has hit anything
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, weaponRange))
        {
            // Check if you hit an enemy
            Enemy enemy = hit.transform.GetComponentInParent<Enemy>();

            if (enemy != null)
            {
                // Deal damage
                enemy.RecieveDamage(damage);
            }
            else
            {
                Debug.Log("hit: " + hit.transform.name);
            }
        }
    }

    public virtual IEnumerator ReloadMagazine()
    {
        isReloading = true;

        while(isReloading)
        {
            yield return new WaitForSeconds(reloadTime);
            isReloading = false;
        }
        
        // Reload
        int bulletSpaceLeft = ammoMagazineMax - ammoMagazineCurrent;
        if (ammoCurrent >= bulletSpaceLeft)
        {
            ammoMagazineCurrent += bulletSpaceLeft;
            ammoCurrent -= bulletSpaceLeft;
        }
        else
        {
            ammoMagazineCurrent += ammoCurrent;
            ammoCurrent = 0;
        }

        Debug.Log("Reloaded");

        yield break;
    }
}
