using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : MonoBehaviour
{
    public float exit_distence;
    //public GateLink this_gatelink;
    public GameObject _Gate;

    private void OnTriggerEnter(Collider other)
    {


        //Debug.Log("trigerActive");
        other.transform.position = _Gate.transform.position + _Gate.transform.forward *exit_distence;

    }
}