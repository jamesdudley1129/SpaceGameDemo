using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ShipControls : MonoBehaviour
{
    //debug
    public float drawTime = 0;
    //debug

    ShipSpecs shipSpecs;
    [SerializeField]
    [Range(0f, 1f)]
    public float MouseRadialSensitivity;
    [Range(0f, 1f)]
    public float rotateSensitivityX;
    [Range(0f, 1f)]
    public float rotateSensitivityY;
    [Range(0f, 1f)]
    public float rotateSensitivityZ;
    [Range(-100f, 100f)]
    public float throttle;
    public bool player_control;
    public Vector3 angularVelocity;
    public bool StableFlightAssist;
    Rigidbody rb;
    //playerVariables
    public WorldTracker worldTracker;
    public Mouse mouse;

    public float mouse_deadzone;
    public float mouse_elasticity;
    public float mouse_range;
    public float mouseSinsitivityDivsor;
    float lastclockTime = 0f;
    public float resetMouseWaitTimer;
    public Vector3 localVelocity;
    public float MaxAngularVelocity;
    public List<ObjectStatus> OnScreen; //move to function after debugging
    public ObjectStatus activeTarget;
    
    private void Start()
    {
        shipSpecs = gameObject.GetComponent<ShipSpecs>();
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = MaxAngularVelocity;
        if (worldTracker == null)
        {
            worldTracker = GetComponentInParent<WorldTracker>();
        }
        if (mouse == null)
        {
            mouse = GetComponentInParent<Mouse>();
        }
    }
    private void Update()
    {
        Debug.DrawLine(transform.position, transform.position + transform.TransformDirection(Vector3.forward) * 5, Color.blue,drawTime, false);//this provides the fwd axis of ship
        Debug.DrawLine(transform.position, transform.position + transform.TransformDirection(localVelocity), Color.red, drawTime,false);
    }
    public void FixedUpdate()
    {
        PassDataDownToComponents();
    }

    public void UpdateVelocity()
    {
        localVelocity = transform.InverseTransformDirection(rb.velocity);
        //fixes floating point errors checking for a range for 0
        for (int i = 0;i <= 2 ; i++)
        {
           if(0.01f >= Mathf.Abs(localVelocity[i]))
            {
                localVelocity[i] = 0f;
            }
        }

        angularVelocity = transform.InverseTransformDirection(rb.angularVelocity);
    }
    //Player Mouse functions//----------------------------------<*>
    public void UpdateJoystick()
    {
            
        if (mouse.EngagedMouse)
        {

            if (Input.GetAxisRaw("mouse x") != 0 || Input.GetAxisRaw("mouse y") != 0)//if input is provided
            {
                Vector2 input = new Vector2(Input.GetAxisRaw("mouse x") * mouse.sensitivity, Input.GetAxisRaw("mouse y") * mouse.sensitivity);
                Vector2 cursorSlope = new Vector2();
                ///////
                ///////

                //if input can be applyed
                if ((mouse.flightCursor.localPosition + new Vector3(input.x, input.y, 0)).magnitude >= mouse_range)
                {
                    cursorSlope = (mouse.flightCursor.localPosition + new Vector3(input.x, input.y, 0)).normalized;//need to get simplified slope
                    mouse.flightCursor.localPosition = (cursorSlope) * mouse_range;
                }
                else
                {
                    mouse.flightCursor.localPosition = new Vector3(mouse.flightCursor.localPosition.x + input.x, mouse.flightCursor.localPosition.y + input.y, 0);
                }
            }
            
            else
            {
                //rubber band
                float y1 = mouse.flightCursor.localPosition.magnitude;
                float y2 = mouse.flightCursor.localPosition.magnitude - mouse_elasticity;
                float x1 = 1;
                float x3 = y1;
                float y3 = x1 * y2;
                float scale = y3 / x3;

                if ((mouse.flightCursor.localPosition * scale).magnitude >= mouse_deadzone)
                {
                    //scales back this back to max range
                    mouse.flightCursor.localPosition *= scale;
                }
                else
                {
                    //centers
                    mouse.flightCursor.localPosition = mouse.center;
                }
            }
            
        }
        else
        {
            //centers
            mouse.flightCursor.localPosition = mouse.center;
        }
    }

    //Player aim functions//------------------------------------0</>need to fix PlayerMouseAim
    //Mouse Aim Incomplete
    public void PlayerMouseStableize()
    {

        //not working properly when mouse is in the four quadrents. try makeing conditons multiplying based on angle

        Vector3 translate = new Vector3(-mouse.flightCursor.localPosition.y, mouse.flightCursor.localPosition.x, 0f);

        if (!Input.GetButton("down") && !Input.GetButton("up") && !Input.GetButton("down") & !Input.GetButton("up") )
        {
            if (mouse.flightCursor.localPosition.magnitude >= mouse_deadzone)
            {

                float percent_of_max_velocity = (translate.magnitude / (mouse_range + mouse_deadzone));
                float TargetForce = MaxAngularVelocity * percent_of_max_velocity;
                Vector3 cancle_vector = new Vector3(angularVelocity.normalized.x - translate.normalized.x, angularVelocity.normalized.y - translate.normalized.y, 1 - translate.normalized.z);//cancle vector isnt doing anuf

                if (mouse.rawInput.magnitude != 0)
                {
                    rb.AddRelativeTorque(translate.normalized * shipSpecs.ThrusterMaxAcceleration * TargetForce * Time.deltaTime, ForceMode.Force);
                    //rb.AddRelativeTorque( * shipSpecs.ManeuverThrusterForce * Time.deltaTime, ForceMode.Force);
                }
                rb.AddRelativeTorque(-cancle_vector *shipSpecs.ThrusterMaxAcceleration * TargetForce * Time.deltaTime, ForceMode.Force);
                //correct existing angular velocity  thats not in the direction of the translate

            }
        }
    }
    public void PlayerRotate()
    {
        if (Input.GetButton("e") && !Input.GetButton("q"))
        {
            rb.AddRelativeTorque(-Vector3.forward * shipSpecs.ThrusterMaxAcceleration * rotateSensitivityZ * Time.deltaTime, ForceMode.Impulse);
            
        }
        if (Input.GetButton("q") && !Input.GetButton("e"))
        {

            rb.AddRelativeTorque(Vector3.forward * shipSpecs.ThrusterMaxAcceleration * rotateSensitivityZ * Time.deltaTime, ForceMode.Impulse);
        }
    }
    public void PlayerPitch()
    {

        if (Input.GetButton("down") && !Input.GetButton("up"))
        {
            rb.AddRelativeTorque(Vector3.right * shipSpecs.ThrusterMaxAcceleration * rotateSensitivityX * Time.deltaTime, ForceMode.Impulse);
        }
        if (Input.GetButton("up") && !Input.GetButton("down"))
        {
            rb.AddRelativeTorque(-Vector3.right * shipSpecs.ThrusterMaxAcceleration * rotateSensitivityX * Time.deltaTime, ForceMode.Impulse);
        }
    }
    public void PlayerYaw()
    {
        if (Input.GetButton("left") && !Input.GetButton("right"))
        {
            rb.AddRelativeTorque(-Vector3.up * shipSpecs.ThrusterMaxAcceleration * rotateSensitivityY * Time.deltaTime, ForceMode.Impulse);
        }
        if (Input.GetButton("right") && !Input.GetButton("left"))
        {
            rb.AddRelativeTorque(Vector3.up * shipSpecs.ThrusterMaxAcceleration * rotateSensitivityY * Time.deltaTime, ForceMode.Impulse);
        }
    }

    public void PlayerStableizePitch()
    {

        if (!Input.GetButton("up") && !Input.GetButton("down") && mouse.flight_stick_distance == 0)
        {
            //no imput at all
            if (angularVelocity.x >= shipSpecs.ThrusterMaxAcceleration)
            {
                rb.AddRelativeTorque(-Vector3.right * shipSpecs.ThrusterMaxAcceleration * Time.deltaTime, ForceMode.Impulse);
            }
            else if (angularVelocity.x <= -shipSpecs.ThrusterMaxAcceleration)
            {
                rb.AddRelativeTorque(Vector3.right * shipSpecs.ThrusterMaxAcceleration * Time.deltaTime, ForceMode.Impulse);
            }
            else
            {
                rb.AddRelativeTorque(Vector3.right * -angularVelocity.x * Time.deltaTime, ForceMode.Impulse);
            }
        }
        else if(!Input.GetButton("up") && !Input.GetButton("down") && mouse.flight_stick_distance != 0)
        {
            //mouse is in use
            //create some sort of angular drag??
        }
    }
    public void PlayerStableizeYaw()
    {
        if (!Input.GetButton("left") && !Input.GetButton("right") && mouse.flight_stick_distance == 0)
        {
            if (angularVelocity.y >= shipSpecs.ThrusterMaxAcceleration)
            {
                rb.AddRelativeTorque(-Vector3.up * shipSpecs.ThrusterMaxAcceleration * Time.deltaTime, ForceMode.Impulse);
            }
            else if (angularVelocity.y <= -shipSpecs.ThrusterMaxAcceleration)
            {
                rb.AddRelativeTorque(Vector3.up * shipSpecs.ThrusterMaxAcceleration * Time.deltaTime, ForceMode.Impulse);
            }
            else
            {
                rb.AddRelativeTorque(Vector3.up * -angularVelocity.y * Time.deltaTime, ForceMode.Impulse);
                //rb.AddRelativeTorque(Vector3.up * -angularVelocity.y * Time.deltaTime, ForceMode.VelocityChange);//doesnt work
            }
        }
        else if (!Input.GetButton("up") && !Input.GetButton("down") && mouse.flight_stick_distance != 0)
        {
            //mouse is in use
            //Create some sort of angular drag
        }
        /*
        if (!Input.GetButton("left") && !Input.GetButton("right"))
            {
                if (angularVelocity.y >= shipSpecs.ManeuverThrusterForce)
                {
                    rb.AddRelativeTorque(-Vector3.up * shipSpecs.ManeuverThrusterForce * Time.deltaTime, ForceMode.Impulse);
                }
                else if (angularVelocity.y <= -shipSpecs.ManeuverThrusterForce)
                {
                    rb.AddRelativeTorque(Vector3.up * shipSpecs.ManeuverThrusterForce * Time.deltaTime, ForceMode.Impulse);
                }
                else if (mouse.fligh_stick_distance == 0)
                {
                    rb.AddRelativeTorque(Vector3.up * -angularVelocity.y * Time.deltaTime, ForceMode.Impulse);   
                    //rb.AddRelativeTorque(Vector3.up * -angularVelocity.y * Time.deltaTime, ForceMode.VelocityChange);//doesnt work
                }
            }
        */

    }
    public void PlayerStableizeRoll()
    {

        if (!Input.GetButton("q") && !Input.GetButton("e"))
        {
            if (angularVelocity.z >= shipSpecs.ThrusterMaxAcceleration)
            {
                rb.AddRelativeTorque(Vector3.forward * -shipSpecs.ThrusterMaxAcceleration * Time.deltaTime, ForceMode.Impulse);
            }
            else if (angularVelocity.z <= -shipSpecs.ThrusterMaxAcceleration)
            {
                rb.AddRelativeTorque(Vector3.forward * shipSpecs.ThrusterMaxAcceleration * Time.deltaTime, ForceMode.Impulse);
            }
            else
            {
                rb.AddRelativeTorque((Vector3.forward * -angularVelocity.z) * Time.deltaTime, ForceMode.Impulse);
                //rb.AddRelativeTorque((Vector3.forward * -angularVelocity.z) * Time.deltaTime, ForceMode.VelocityChange);//didnt work
            }
        }
    }

    //Player Movement//-----------------------------------------1<>
    public void PlayerStrafeYaxis()
    {
        if (Input.GetButton("w") && !Input.GetButton("left shift") && !Input.GetButton("left ctrl"))
        {
            rb.AddRelativeForce(Vector3.up * shipSpecs.ThrusterMaxAcceleration * Time.deltaTime, ForceMode.Impulse);

        }
        if (Input.GetButton("s") && !Input.GetButton("left shift") && !Input.GetButton("left ctrl"))
        {
            rb.AddRelativeForce(Vector3.up * -shipSpecs.ThrusterMaxAcceleration * Time.deltaTime, ForceMode.Impulse);
        }
    }
    public void PlayerStrafeXaxis()
    {
        if (Input.GetButton("a") && !Input.GetButton("left shift") && !Input.GetButton("left ctrl"))
        {
            rb.AddRelativeForce(Vector3.right * -shipSpecs.ThrusterMaxAcceleration * Time.deltaTime, ForceMode.Impulse);
        }
        if (Input.GetButton("d") && !Input.GetButton("left shift") && !Input.GetButton("left ctrl"))
        {
            rb.AddRelativeForce(Vector3.right * shipSpecs.ThrusterMaxAcceleration * Time.deltaTime, ForceMode.Impulse);
            
        }
    }

    public void PlayerUpdateThrottle()
    {
        if (throttle != 0)
        {
            Vector3 force = new Vector3();
            if(throttle > 0) {
               force = Vector3.forward * shipSpecs.RearEngineForce * (throttle / 100) * Time.deltaTime;
            }
            else if( throttle < 0)
            {
               force = Vector3.forward * shipSpecs.FrontEngineForce * (throttle / 100) * Time.deltaTime;
            }
            rb.AddRelativeForce(force, ForceMode.Impulse);
            //Debug.DrawLine(transform.position, transform.position + transform.TransformDirection(-(Vector3.forward * shipSpecs.RearEngineForce * (throttle / 100) * Time.deltaTime)),Color.white, drawTime, false);
        }
        
    }
    public void PlayerStableizeXDrift()
    {
        if (!Input.GetButton("a") && !Input.GetButton("d"))
        {

            if (localVelocity.x >= shipSpecs.ThrusterMaxAcceleration)
            {
                rb.AddRelativeForce(Vector3.left * shipSpecs.ThrusterMaxAcceleration * Time.deltaTime, ForceMode.Impulse);
                Debug.DrawLine(transform.position, transform.position + transform.TransformDirection(Vector3.right * shipSpecs.ThrusterMaxAcceleration * Time.deltaTime), Color.white, drawTime, false);
            }
            else if (localVelocity.x <= -shipSpecs.ThrusterMaxAcceleration)
            {
                rb.AddRelativeForce(Vector3.right * shipSpecs.ThrusterMaxAcceleration * Time.deltaTime, ForceMode.Impulse);
                Debug.DrawLine(transform.position, transform.position + transform.TransformDirection(Vector3.left * shipSpecs.ThrusterMaxAcceleration * Time.deltaTime), Color.white, drawTime, false);
            }
            else
            {
                //once close to zero set to zero
                //rb.AddRelativeForce(Vector3.right * -localVelocity.x * Time.deltaTime, ForceMode.Impulse);
                //rb.AddRelativeForce(Vector3.right * -localVelocity.x * Time.deltaTime, ForceMode.VelocityChange);
                rb.AddRelativeForce(new Vector3(-localVelocity.x, 0f, 0f) * Time.deltaTime, ForceMode.Impulse);
                //rb.velocity = transform.TransformDirection(new Vector3(0f, localVelocity.y, localVelocity.z));
            }
        }

    }
    public void PlayerStableizeYDrift()
    {

        if (!Input.GetButton("w") && !Input.GetButton("s"))
        {
            if (localVelocity.y >= shipSpecs.ThrusterMaxAcceleration)
            {
                rb.AddRelativeForce(Vector3.down * shipSpecs.ThrusterMaxAcceleration * Time.deltaTime, ForceMode.Impulse);
                Debug.DrawLine(transform.position, transform.position + transform.TransformDirection(Vector3.up * shipSpecs.ThrusterMaxAcceleration * Time.deltaTime), Color.white, drawTime, false);
            }
            else if (localVelocity.y <= -shipSpecs.ThrusterMaxAcceleration)
            {
                rb.AddRelativeForce(Vector3.up * shipSpecs.ThrusterMaxAcceleration * Time.deltaTime, ForceMode.Impulse);
                Debug.DrawLine(transform.position, transform.position + transform.TransformDirection(Vector3.down * shipSpecs.ThrusterMaxAcceleration * Time.deltaTime), Color.white, drawTime, false);
            }
            else
            {
                //once close to zero set to zero
                //rb.AddRelativeForce(Vector3.up * -localVelocity.y * Time.deltaTime, ForceMode.Impulse);
                //rb.AddRelativeForce(Vector3.up * -localVelocity.y * Time.deltaTime, ForceMode.VelocityChange);
                rb.AddRelativeForce(new Vector3(0f, -localVelocity.y, 0f) * Time.deltaTime, ForceMode.Impulse);
                //rb.velocity = transform.TransformDirection(new Vector3(localVelocity.x,0f,localVelocity.z));
            }
        }
    }
    public void PlayerSlowStop()
    {
        if (throttle == 0f)//based on 
        {
            if (localVelocity.z >= shipSpecs.FrontEngineForce)
            {
                rb.AddRelativeForce(Vector3.back * shipSpecs.FrontEngineForce * Time.deltaTime, ForceMode.Impulse);
                //Debug.DrawLine(transform.position, transform.position + transform.TransformDirection(Vector3.forward * shipSpecs.RearEngineForce * 0.5f * Time.deltaTime), Color.white, drawTime, false);
            }
            else if (localVelocity.z <= -shipSpecs.RearEngineForce)
            {
                rb.AddRelativeForce(Vector3.forward * shipSpecs.RearEngineForce * Time.deltaTime, ForceMode.Impulse);
                //Debug.DrawLine(transform.position, transform.position + transform.TransformDirection(Vector3.back * shipSpecs.RearEngineForce * Time.deltaTime), Color.white, drawTime, false);
            }
            else
            {
                //rb.AddRelativeForce(Vector3.forward * -localVelocity.z * Time.deltaTime, ForceMode.Impulse);
                //rb.AddRelativeForce(Vector3.forward * -localVelocity.z * Time.deltaTime, ForceMode.VelocityChange);
                //!!! \/ <MUST Convert this vector 3 back to world velocity> \/ !!!
                rb.AddRelativeForce(new Vector3(0f, 0f,-localVelocity.z) * Time.deltaTime, ForceMode.Impulse);
                //rb.velocity = transform.TransformDirection(new Vector3(localVelocity.x, localVelocity.y, 0f));
            }
        }
    }
    //Player Ship Action Functions---------------------------------2<>
    public void UpdatePlayerFire()
    {
        if (mouse.EngagedMouse)
        {
            if (Input.GetButtonDown("mouse0"))
            {
                foreach (GameObject Weapon in shipSpecs.Weapons)
                {
                    Weapon script = Weapon.GetComponent<Weapon>();
                    //script.focus = activeTarget.gameObject;
                    script.Manual_Fireing();
                }
            }
            if (Input.GetButtonUp("mouse0"))
            {
                foreach (GameObject Weapon in shipSpecs.Weapons)
                {

                    Weapon script = Weapon.GetComponent<Weapon>();
                    script.Manual_StopFireing();
                }
            }
        }
        else
        {
            foreach (GameObject Weapon in shipSpecs.Weapons)
            {

                Weapon script = Weapon.GetComponent<Weapon>();
                if (script.fireing)
                {
                    script.Manual_StopFireing();
                }
            }
        }
    }
    //Player Target Functions -------------------------------------3<>
    public void SelectFwdObject()
    {
        CameraBeh camScript = worldTracker.camrea.GetComponent<CameraBeh>();

        if (transform.GetComponent<ShipSpecs>().RadarPlatform.transform != null)
        {
            //refrence to local objects from radair
            List<ObjectStatus> local_objects = transform.GetComponent<ShipSpecs>().RadarPlatform.transform.GetChild(0).GetComponent<RadarComponent>().localObjects;



            Vector2 fov = camScript.fov;

            if (Input.GetButton("t") && !Input.GetButton("left shift") && !Input.GetButton("left ctrl"))
            {

                OnScreen = new List<ObjectStatus>(); //this member can be privet to function
                foreach (ObjectStatus obj in local_objects)
                {
                    if (obj.gameObject != worldTracker.player)
                    {
                        Vector2 objpos = camScript.ObjectToScreenPos(obj.transform.position);

                        if (objpos.x <= fov.x && objpos.y <= fov.y)
                        {
                            OnScreen.Add(obj);
                        }
                    }
                }
                //find witch object is closest to orgin on screen
                ObjectStatus closestObjToOrgin = null;
                if (OnScreen.Count != 0)
                {
                    foreach (ObjectStatus obj in OnScreen)
                    {
                        if (closestObjToOrgin == null)
                        {
                            closestObjToOrgin = obj;
                        }
                        if (camScript.ObjectToScreenPos(obj.transform.position).magnitude <= camScript.ObjectToScreenPos(closestObjToOrgin.transform.position).magnitude)
                        {
                            closestObjToOrgin = obj;
                        }

                    }
                }
                activeTarget = closestObjToOrgin;

            }

            if (mouse.EngagedMouse == false && Input.GetButtonDown("mouse0"))
            {
                OnScreen = new List<ObjectStatus>(); //this member can be privet to function
                foreach (ObjectStatus obj in local_objects)
                {
                    if (obj.gameObject != worldTracker.player)
                    {
                        Vector2 objpos = camScript.ObjectToScreenPos(obj.transform.position);

                        if (objpos.x <= fov.x && objpos.y <= fov.y)
                        {
                            OnScreen.Add(obj);
                        }
                    }
                }
                //select object closest to the mouse pos to world space 
                ObjectStatus ClosestToMouse = null;
                foreach (ObjectStatus obj in OnScreen)
                {
                    float mouse_distnace = mouse.mousePos.magnitude;
                    Vector2 mousepos = new Vector2(mouse.mousePos.x, mouse.mousePos.y);
                    if (ClosestToMouse != null)
                    {
                        float _MagnitudeToLast = Vector2.SqrMagnitude(camScript.ObjectToScreenPos(ClosestToMouse.transform.position) - mousepos);
                        float _MagnitudeToCurrent = Vector2.SqrMagnitude(camScript.ObjectToScreenPos(obj.transform.position) - mousepos);
                        if (_MagnitudeToCurrent <= _MagnitudeToLast){
                            ClosestToMouse = obj;
                        }
                    }
                    else
                    {
                        ClosestToMouse = obj;
                    }       
                }
                activeTarget = ClosestToMouse;
            }

        }
    }
    //subsystems
    public void PassDataDownToComponents()
    {


        foreach (GameObject weapon in transform.GetComponent<ShipSpecs>().Weapons)
        {
            weapon.GetComponent<Weapon>().restingDir = transform;
            if (activeTarget != null)
            {
                weapon.GetComponent<Weapon>().focus = activeTarget.gameObject;
                
            }
            else
            {
                weapon.GetComponent<Weapon>().focus = null;
                
            }
        }
    }
    //Ship Camrea Actions
    public void CamreaModeCheck()
    {
        CameraBeh cam = GetComponent<ObjectSettings>().cam;
        if (Input.GetButton("F1"))//first person
        {
            GetComponent<ObjectSettings>().isFirstPerson = true;
            cam.CamreaSet = true;
        }
        if (Input.GetButton("F2"))//third person
        {
            GetComponent<ObjectSettings>().isFirstPerson = false;
            cam.CamreaSet = true;
        }
        if (Input.GetButton("F3"))//free cam will allow player to fly camrea
        {
            GetComponent<ObjectSettings>().isFirstPerson = false;
            cam.CamreaSet = false;
        }
    }

    //End of Player Functions ---------------------------------








}