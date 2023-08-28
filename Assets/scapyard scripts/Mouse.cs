using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    public Vector2 center = new Vector2();
    internal Vector3 mousePos = new Vector3();
    public GameObject standerdCursorObj;//might be exlusive to debuging
    public GameObject flightCursorObj;
    public bool EngagedMouse;  
    RectTransform standardCursor;
    public RectTransform flightCursor;//accessed by ShipControls
    public float sensitivity;
    public float flight_stick_magnitude;
    public float flight_stick_distance;
    public float fligh_stick_angle;
    public Vector2 rawInput = new Vector2();
    private void Awake()
    {
    flightCursor = flightCursorObj.GetComponent<RectTransform>();
    standardCursor = standerdCursorObj.GetComponent<RectTransform>();
    }

    private void Update()
    {
        //standerd Cursor
        standardCursor.localPosition = mousePos;
        UpdateCursor();
        PlayerCalculateMouse();
        //flight Cursor
        rawInput = new Vector3(Input.GetAxisRaw("mouse x"), Input.GetAxisRaw("mouse y"));


    }
    public void UpdateCursor()
    {
        mousePos.Set(mousePos.x + Input.GetAxisRaw("mouse x") * sensitivity, mousePos.y + Input.GetAxisRaw("mouse y") * sensitivity, 0);        
    }
    public void PlayerCalculateMouse()
    {
        fligh_stick_angle = Mathf.Atan2(flightCursor.localPosition.y, flightCursor.localPosition.x) * Mathf.Rad2Deg;
        flight_stick_magnitude = Vector2.SqrMagnitude(flightCursor.localPosition);
        flight_stick_distance = Vector2.Distance(center, flightCursor.localPosition);
        //Debug.Log("angle:" + angle.ToString() + "   magnitude:" + magnitude.ToString());
    }
    public void mouse_reset()
    {
        mousePos = Vector3.zero;
    }




}






