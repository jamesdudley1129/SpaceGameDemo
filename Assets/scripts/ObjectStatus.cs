using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStatus : MonoBehaviour
{
    public bool InfinateHP = false;
    public float HP;
    public float Shield;
    public float MaxHP;
    public float MaxShield;
    public Relation RelationStatus = Relation.Nutural;
    public bool detectable_onRadar = false;
    public string faction;
    private void Update()
    {
        if (!InfinateHP)
        {
            if (HP <= 0)
            {
                GetComponent<GameManagerRefrence>().DestroyObject();
            }
        }
        else
        {
            HP = MaxHP;
        }
    }
}
