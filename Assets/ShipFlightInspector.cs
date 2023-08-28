using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipFlightInspector : MonoBehaviour
{
    public UIinfo info;
    GameObject _camera;
    Camera Cam;
    public GameObject UIShip;
    public Vector3 UI_pos;
    public Vector3 UI_rot;
    public float VelIconDistance;

    public Vector3 ActualRot;

    public GameObject Velocity_Prefab;
    public GameObject Velocity_Instance;
    // Update is called once per frame
    void Update()
    {
        if(_camera == null && info._camera != null)
        {
            _camera = info._camera;
            Cam = _camera.GetComponent<Camera>();
        }
        else
        {
            SetUIShip();
            SetUIShipVelocity();
        }

    }
    public void SetUIShip()
    {
        Vector3 UIposWithCam = new Vector3();
        Quaternion UIrotWithCam = new Quaternion();
        UIposWithCam = Cam.ScreenToWorldPoint(new Vector3(UI_pos.x * Cam.pixelWidth, UI_pos.y * Cam.pixelHeight, UI_pos.z));
        UIrotWithCam = Quaternion.Euler(new Vector3(-Cam.gameObject.transform.rotation.eulerAngles.x, Cam.gameObject.transform.rotation.eulerAngles.y * 2, Cam.gameObject.transform.rotation.eulerAngles.z) + UI_rot);
        UIShip.transform.localPosition = UIposWithCam;
        UIShip.transform.localRotation = UIrotWithCam;
        //for debuging in inspector
        ActualRot = UIShip.transform.rotation.eulerAngles;
    }
    public void SetUIShipVelocity()
    {
        Vector3 VelDir = new Vector3();
        if(info._PlayerShip != null)
        {
            //will need to change when Velocity Is over ridden
            VelDir = info._PlayerShip.GetComponent<ShipControls>().localVelocity;
        }
        if (VelDir != Vector3.zero)
        {
            if (Velocity_Instance == null)
            {
                //create
                Velocity_Instance = Instantiate(Velocity_Prefab, transform);
            }
            Velocity_Instance.transform.localPosition = UIShip.transform.localPosition;

            Velocity_Instance.transform.localRotation = Quaternion.LookRotation(VelDir);//Strafe left and right is fucked
            Velocity_Instance.transform.localPosition += Velocity_Instance.transform.forward;

        }
        else
        {
            if (Velocity_Instance != null)
            {
                Transform.Destroy(Velocity_Instance);
            }
        }
    }
}
