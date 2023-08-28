using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedIconBeh : MonoBehaviour
{
    public GameObject innerRing;
    public GameObject OuterRing;
    public float innerSpeed;
    public float outerSpeed;
    void Update()
    {
        innerRing.transform.Rotate(Vector3.forward * innerSpeed * Time.deltaTime);
        OuterRing.transform.Rotate(-Vector3.forward * outerSpeed * Time.deltaTime);
    }


    
}
