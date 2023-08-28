using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectUISpecUpdate : MonoBehaviour
{
    public GameObject NameField;
    public GameObject ShieldField;
    public GameObject HullField;



    public void SetName(string name)
    {
        NameField.GetComponent<Text>().text = name;
    }
    public void SetShield(string Shield)
    {
        ShieldField.GetComponent<Text>().text = Shield;
    }
    public void SetHull(string Hull)
    {
        HullField.GetComponent<Text>().text = Hull;
    }
}
