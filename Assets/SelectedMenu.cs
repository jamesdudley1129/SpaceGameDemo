using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedMenu : MonoBehaviour
{
    GameObject RefrenceGO;
    public List<GameObject> Buttons = new List<GameObject>();
    public GameObject InfoUpdate;
    ObjectUISpecUpdate InfoTab;
    ObjectStatus refrenceStatus;

    public void Update()
    {
        if (RefrenceGO != null)
        {
            InfoTab = InfoUpdate.GetComponent<ObjectUISpecUpdate>();
            InfoTab.SetName(refrenceStatus.name);
            InfoTab.SetShield(MoreMath.Percintage(refrenceStatus.Shield, refrenceStatus.MaxShield).ToString());
            InfoTab.SetHull(MoreMath.Percintage(refrenceStatus.HP, refrenceStatus.MaxHP).ToString());
        }
    }
    public void SetRefrence(GameObject refrence)
    {
        RefrenceGO = refrence;
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}
