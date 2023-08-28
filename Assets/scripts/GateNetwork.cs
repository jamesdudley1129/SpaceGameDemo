using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateNetwork : MonoBehaviour
{
    public List<GameObject> _Gates = new List<GameObject>();
    // Update is called once per frame
    void Start()
    {
        //simple config

        _Gates[0].GetComponent<GateLink>().LinkedGate = _Gates[1];
        _Gates[1].GetComponent<GateLink>().LinkedGate = _Gates[0];
    }
}
