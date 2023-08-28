using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBeh : MonoBehaviour
{
    public WorldTracker worldTracker;
    public Vector3 _defaultOffset;
    public Vector3 _default_Rotation; //need to add functionality
    public Vector3 freeCam_position;
    public Vector3 freeCam_rotation;
    public bool CamreaSet = false;
    public Vector2 fov;
    // Start is called before the first frame update
    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {
        fov = GetFOV();
        if (CamreaSet == false)
        {
            if (worldTracker.player != null)
            {
                transform.SetParent(worldTracker.player.transform);
                transform.localPosition = Vector3.zero;
                CamreaSet = true;

            }

        }
        else
        {
            if (worldTracker.player == null)
            {
                CamreaSet = false;
                transform.SetParent(worldTracker.transform);
            }

            transform.localPosition = _defaultOffset;
            transform.localRotation = Quaternion.Euler(_default_Rotation);
            //maybe if i set the rotation z value = parents z value and apply x and y values from the rotation var

        }


    }

    private Vector2 GetFOV()
    {
        Vector2 Fov = new Vector2();
        float FOVangleV = GetComponent<Camera>().fieldOfView / 2;
        float FOVangleH = Camera.VerticalToHorizontalFieldOfView(GetComponent<Camera>().fieldOfView, GetComponent<Camera>().aspect) / 2;    
        //Debug.Log("FOV_V:" + FOVangleV + "FOV_H:" + FOVangleH + "|| Aspect:" + _camrea.GetComponent<Camera>().aspect);
        Fov.Set(FOVangleH, FOVangleV);
        return Fov;
    }

    public Vector2 ObjectToScreenPos(Vector3 WorldPos) //not sure how this works
    {
        Vector2 ObjectPointAngle = new Vector2();
        if (WorldPos != null)
        {
            Vector2 local_dir = transform.InverseTransformDirection(WorldPos - transform.position);
            Vector3 axis = transform.forward;//global fwd direction
            Vector3 point = WorldPos - transform.position;//global direction 
            float angle = Vector3.Angle(point, axis);//angle of direction from fwd axis
            ObjectPointAngle = new Vector3(Mathf.Abs(local_dir.normalized.x * angle), Mathf.Abs(local_dir.normalized.y * angle));//this returns the local direction (objectdirection.normalized * the angel)         
        }
        return ObjectPointAngle;
    }


}
