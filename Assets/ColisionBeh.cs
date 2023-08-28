using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionBeh : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        GameObject collisonObject = collision.gameObject;
        ObjectStatus status = null;
        if (collisonObject.GetComponent<ObjectStatus>() == null)
        {
            while (status == null)
            {
                collisonObject = collisonObject.gameObject.transform.parent.gameObject;
                if (collisonObject.GetComponent<ObjectStatus>() != null)
                {
                    status = collisonObject.GetComponent<ObjectStatus>();
                }
            }
        }
        else
        {
            status = collisonObject.GetComponent<ObjectStatus>();
        }
        //Damage = mass*size* Relitive speed
        float relitive_Speed = GetComponent<Rigidbody>().velocity.magnitude;
        if (status.gameObject.GetComponent<Rigidbody>() != null)
        {
            relitive_Speed = (status.gameObject.GetComponent<Rigidbody>().velocity - GetComponent<Rigidbody>().velocity).magnitude;
        }
        float Damage = GetComponent<Rigidbody>().mass * transform.localScale.magnitude * relitive_Speed;// wonder if this is the speed after impact?
        if (Damage >= status.Shield)
        {
            float hp_damage = Damage - status.Shield;
            status.Shield = 0f;
            status.HP -= hp_damage;
        }
        else
        {
            status.Shield -= Damage;
        }
        GetComponent<ObjectStatus>().HP -= Damage;
    }
}
