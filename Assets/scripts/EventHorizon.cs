using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHorizon : MonoBehaviour
{
    public float spinSpeed;
    
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(transform.forward, spinSpeed * Time.deltaTime);
    }
}
