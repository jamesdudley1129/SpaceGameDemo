using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSettings : MonoBehaviour
{
    public Vector3 defaultThirdPersonCamPos;
    public Vector3 defaultThirdPersonCamRot;
    public Vector3 defaultFirstPersonCamPos;
    public Vector3 defaultFirstPersonCamRot;
    public bool isFirstPerson;
    public CameraBeh cam;
    private void Update()
    {
        if (cam != null)
        {
            if (isFirstPerson)
            {
                cam._defaultOffset = defaultFirstPersonCamPos;
                cam._default_Rotation = defaultFirstPersonCamRot;
            }
            else
            {
                cam._defaultOffset = defaultThirdPersonCamPos;
                cam._default_Rotation = defaultThirdPersonCamRot;

            }
        }
        else
        {
            cam = GetComponent<GameManagerRefrence>().worldTracker.camrea.GetComponent<CameraBeh>();
        }
    }
}
