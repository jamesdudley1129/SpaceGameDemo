using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationAnimation : MonoBehaviour
{
    public float speed;
    public GameObject station;
    void Update()
    {
        transform.RotateAround(station.transform.position, transform.forward, speed);

    }
}
