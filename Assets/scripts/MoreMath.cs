using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class MoreMath
{
    public static bool IsPos(float input)
    {
        if (input >= 0)
        {
            return true;
        }
        if (input < 0)
        {
            return false;
        }
        else
        {
            return false;
        }
    }

    public static float Percintage(float current, float max)
    {
        float results = 0.0f;

        results = (current/max) * 100f;

        return results;
    }

    public static bool IsWithenRange(float number, float min_range, float Max_range)
    {
        
        if(min_range <= number && number <= Max_range)
        {
            Debug.LogWarning("Is Withen range # =" + number + ": is true");
            return true;
        }
        else
        {
            Debug.LogWarning("Is Withen range # =" + number + ": is false");
            return false;
        }
        
    }
    public static float RoundToClosestOfSet(float input,float[] numbers)
    {
        float[] shortest = new float[2];//[num,distance]
        foreach (float num in numbers)
        {
            if (shortest[1] == 0)
            {
                shortest[0] = num;
                shortest[1] = Mathf.Abs(input - num);
            }
            else if(Mathf.Abs(input - num) < shortest[1])
            {
                shortest[0] = num;
                shortest[1] = Mathf.Abs(input - num);
            }
        }
        return shortest[0];
        
    }
}