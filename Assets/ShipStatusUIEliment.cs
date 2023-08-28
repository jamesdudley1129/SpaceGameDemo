using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipStatusUIEliment : MonoBehaviour
{
    public Text Shield;
    public Text Hull;
    public bool isPercentage;
    public void SetHull(float max,float current)
    {
        if (isPercentage)
        {
            Hull.text = "|Hull " + MoreMath.Percintage(current,max).ToString() + "%|";
        }
        else
        {
            Hull.text = "|Hull " + Mathf.RoundToInt(current).ToString() + "/" + Mathf.RoundToInt(max).ToString() + "|";
        }
    }
    public void SetShield(float max, float current)
    {
        if (isPercentage)
        {
            Shield.text = "|Shield " + MoreMath.Percintage(current,max).ToString() + "%|";
        }
        else
        {
            Shield.text = "|Shield " + Mathf.RoundToInt(current).ToString() + "/" + Mathf.RoundToInt(max).ToString() + "|";
        }
    }
    public void IsPercintageDisplay(bool awnser)
    {
        isPercentage = awnser;
    }
}
