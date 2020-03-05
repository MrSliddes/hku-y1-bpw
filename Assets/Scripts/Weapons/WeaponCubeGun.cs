using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCubeGun : MonoBehaviour
{
    public bool inUse = true;
    public GameObject projectile;
    public Transform gunEnd;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Fire2") == 1)
        {
            FireWeapon();
        }
    }

    void FireWeapon()
    {
        if(inUse)
        {
            GameObject a = Instantiate(projectile, gunEnd.position, Quaternion.identity);
        }
    }
}
