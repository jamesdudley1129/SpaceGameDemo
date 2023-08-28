using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugActions : MonoBehaviour
{
    //Debug AREA
    public GameObject debugObjectAsset;
    public List<GameObject> debugobjects = new List<GameObject>();
    public bool fire = false;
    //DebugAREA
    // Update is called once per frame
    void Update()
    {
        List<GameObject> keepobj = new List<GameObject>();
        foreach (GameObject item in debugobjects)
        {
            if(item != null)
            {
                keepobj.Add(item);
            }
        }
        debugobjects = keepobj;
        //DEBUG AREA
        if (Input.GetButtonDown("1"))
        {
            debugobjects.Add(Instantiate(debugObjectAsset, gameObject.transform));
        }
        if (Input.GetButtonDown("2"))
        {
            if (debugobjects.Capacity >= 1)
            {
                if (debugobjects[0] != null)
                {
                    debugobjects[0].GetComponent<GameManagerRefrence>().DestroyObject();
                }
            }
        }
        if (Input.GetButtonDown("3"))
        {
            fire = !fire;
            ShipSpecs specs = GetComponent<WorldTracker>().player.GetComponent<ShipSpecs>();
            
            foreach (GameObject weapon in specs.Weapons)
            {

                if (fire == true)
                {
                    weapon.GetComponent<Weapon>().Manual_Fireing();
                }
                else
                {
                    weapon.GetComponent<Weapon>().Manual_StopFireing();
                }
            }
        }

        //DEBUG AREA
    }
}
