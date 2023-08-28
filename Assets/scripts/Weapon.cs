using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponType weaponType;
    public AmmoType ammoType;
    public GameObject focus;
    public Transform restingDir;
    public bool AutoFire;
    public bool fireing = false;

    private void FixedUpdate()
    {
        switch (weaponType)
        {
            case WeaponType.Static:
                //no tracking
                break;
            case WeaponType.Tracking:
                GetComponent<WeaponTracking>().track = focus; 
                break;
            default:
                break;
        }

    }
    public void Manual_Fireing()
    {
        if (!AutoFire)
        {
            if (!fireing)
            {
                switch (ammoType)
                {
                    case AmmoType.Lazer:
                        GetComponent<LazerWeapon>().LazerStart();
                        break;
                    case AmmoType.Kinetic:
                        break;
                    default:
                        break;
                }
            }
            fireing = true;
        }
    }

    public void Manual_StopFireing()
    {
        if (!AutoFire)
        {


            switch (ammoType)
            {
                case AmmoType.Lazer:
                    GetComponent<LazerWeapon>().LazerStop();
                    break;
                case AmmoType.Kinetic:
                    break;
                default:
                    break;
            }
            fireing = false;
        }
    }

    public void AutoFireing()
    {
        if (GetComponent<WeaponTracking>().OnTarget == true && AutoFire && focus != null)
        {
            if (!fireing)
            {
                fireing = true;
                switch (ammoType)
                {
                    case AmmoType.Lazer:
                        GetComponent<LazerWeapon>().LazerStart();
                        break;
                    case AmmoType.Kinetic:
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            fireing = false;
            switch (ammoType)
            {
                case AmmoType.Lazer:
                    GetComponent<LazerWeapon>().LazerStop();
                    break;
                case AmmoType.Kinetic:
                    break;
                default:
                    break;
            }

        }

    }
}
