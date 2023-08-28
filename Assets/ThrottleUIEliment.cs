using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrottleUIEliment : MonoBehaviour
{
    public Text AccelerationText;
    public Text ThrottleText;
    public int Decimal_AccuracyIndex = 2;

    public void SetThrottle(int pos)
    {
        ThrottleText.text = "|Throttle" + pos.ToString() + "%|";
    }
    public void SetAcceleration(float Meters_second)
    {
        string MetersSecString = Meters_second.ToString();
        string[] splitDecimal = new string[2];
        splitDecimal = MetersSecString.Split('.');
        MetersSecString = "";
        if(splitDecimal[0] != "0")//if a whole number is present
        {
            MetersSecString = splitDecimal[0];
        }
        if (splitDecimal.Length > 1)//numbers exist after decimal
        {
            if (splitDecimal[1].Length > Decimal_AccuracyIndex)//if decimal is larger then decimal accuracy
            {
                MetersSecString += '.' + splitDecimal[1].Substring(0, Decimal_AccuracyIndex);
            }
            else
            {
                MetersSecString += '.' + splitDecimal[1];
            }
        }



        AccelerationText.text = "|ACC " + MetersSecString + " M/s|";
    }
}
