using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shipforwrd : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime);
    }
}
