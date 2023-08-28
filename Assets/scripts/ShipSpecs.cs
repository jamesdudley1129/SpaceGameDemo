using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSpecs : MonoBehaviour
{
    //actual parts will hold values down the road each with there own hull so parts can be broken
    public int hull = 1;
    public int shield = 1;
    ObjectStatus objectStatus;
    [SerializeField]
    public float RearEngineForce;
    public float FrontEngineForce;
    public float ThrusterMaxAcceleration;
    //mass will come from each part down the road
    //will need to be more specific to the type of platform or have weopon platform contain a script with a idinifyer 
    public List<GameObject> WeaponPlatforms = new List<GameObject>();
    public GameObject RadarPlatform; // radarcomponent
    public List<GameObject> Weapons = new List<GameObject>();
    private void Awake()
    {
        objectStatus = GetComponent<ObjectStatus>();
        SetObjectStatus();
    }
    public void Update()
    {
        PartUpdate();
    }
    public void SetObjectStatus()
    {
        objectStatus.MaxHP = hull;
        objectStatus.HP = hull;
        objectStatus.MaxShield = shield;
        objectStatus.Shield = shield;
    }
    public void PartUpdate()
    {
        for (int i = 0; i < Weapons.Count; i++)
        {
            if (Weapons[i] == null)
            {
                Weapons.RemoveAt(i);
            }
        }

        foreach (GameObject WeaponPlatform in WeaponPlatforms)
        {
            if (WeaponPlatform.transform.childCount != 0)
            {
                if (WeaponPlatform.transform.GetChild(0) != null)
                {
                    if (!Weapons.Contains(WeaponPlatform.transform.GetChild(0).gameObject))
                    {
                        Weapons.Add(WeaponPlatform.transform.GetChild(0).gameObject);
                    }
                }
            }
        }

    }

}
