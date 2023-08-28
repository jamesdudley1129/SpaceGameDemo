using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CollisionUIAlertBeh : MonoBehaviour
{
    public GameObject timer;
    public GameObject Alert;
    public GameObject AlertIcon;
    public Color TimerColor1;
    public Color TimerColor2;
    public float animationTimeDiffrence;
    float last_Time = 0f;

    float icon_scale = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= last_Time + animationTimeDiffrence)
        {
            last_Time = Time.time;
            if (icon_scale <= 1)
            {
                AlertIcon.GetComponent<RectTransform>().localScale = new Vector3(icon_scale, icon_scale, 1f);
                icon_scale += 0.1f;
            }
            else
            {
                icon_scale = 0.1f;
            }
            if(timer.GetComponent<Text>().color == TimerColor1)
            {
                timer.GetComponent<Text>().color = TimerColor2;
            }
            else
            {
                timer.GetComponent<Text>().color = TimerColor1;
            }
        }
    }
}
