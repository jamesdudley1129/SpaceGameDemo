using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VelocityDirIconbeh : MonoBehaviour
{
    public Text speed;
    public void SetSpeedText(float meters_per_second)
    {
        
        if(meters_per_second >= 1000)
        {
            speed.text = (meters_per_second / 1000f) + " km/s";
        }
        else
        {
            speed.text = meters_per_second + " m/s";
        }
    }
}
