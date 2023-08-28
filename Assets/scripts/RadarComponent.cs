using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarComponent : MonoBehaviour
{
    public List<ObjectStatus> localObjects = new List<ObjectStatus>();
    public float ScanRadius;

    void Update()
    {
        localObjects.Clear();
        Collider[] found_objects = Physics.OverlapSphere(transform.position, ScanRadius);
        foreach(Collider found in found_objects)
        {
            if (found.GetComponent<ObjectStatus>())
            {
                if (found.GetComponent<ObjectStatus>().detectable_onRadar)
                {
                    localObjects.Add(found.GetComponent<ObjectStatus>());
                }
            }
        }
    }
}
