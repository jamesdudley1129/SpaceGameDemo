using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool is_paused;
    public GameObject player;
    ShipControls shipControls;
    Mouse mouse;
    public PlayerControlMode playerControlMode;
    public GameObject actionMenu;
    bool actionMenuToggle = false;

    private void Awake()
    {   
        mouse = GetComponent<Mouse>();
    }
    private void Update()
    {
        player = GetComponent<WorldTracker>().player;


        if (Input.GetButtonDown("space"))
        {
            ToggleMouseAim();
        }

        if (Input.GetButtonDown("tab"))
        {
            ToggleActionMenu();
        }
        mouse.UpdateCursor();

        switch (playerControlMode)
        {
            case PlayerControlMode.ship:

                if (shipControls == null)
                {
                    if (GetComponent<WorldTracker>().player != null)
                    {
                        shipControls = GetComponent<WorldTracker>().player.GetComponent<ShipControls>();
                    }
                }
                else
                {
                    if (actionMenuToggle == false)
                    {
                        shipControls.UpdatePlayerFire();
                        //throttle value input NEED TO MOVE TO SHIP_CONTROLS!!
                        if (Input.GetButton("x") && !Input.GetButton("left shift") && !Input.GetButton("left ctrl"))
                        {
                            if (shipControls.throttle < 100)
                            {
                                shipControls.throttle++;
                            }
                        }
                        if (Input.GetButton("z") && !Input.GetButton("left shift") && !Input.GetButton("left ctrl"))
                        {
                            if (shipControls.throttle > -100)
                            {
                                shipControls.throttle--;
                            }
                        }
                        if (Input.GetButton("-") && !Input.GetButton("left shift") && !Input.GetButton("left ctrl"))
                        {
                            shipControls.throttle = 0;
                        }
                        if (Input.GetButton("=") && !Input.GetButton("left shift") && !Input.GetButton("left ctrl"))
                        {
                            shipControls.throttle = 100;
                        }


                    }
                }
                break;
            case PlayerControlMode.onfoot:
                break;
            case PlayerControlMode.spacewalk:
                break;
            default:
                break;

        }
    }
    void FixedUpdate()
    {
        switch (playerControlMode)
        {
            case PlayerControlMode.ship:
                //code for ship input
                //direct access to the players ship
                if (shipControls == null)
                {
                    if (GetComponent<WorldTracker>().player != null)
                    {
                        shipControls = GetComponent<WorldTracker>().player.GetComponent<ShipControls>();
                    }


                }
                else
                {
                    //update velocity each time its changed
                    //mouse input and yaw and pitch input//-----------------------------------

                    
                    
                    //update velocity
                    shipControls.UpdateVelocity();
                    shipControls.CamreaModeCheck();
                    
                    if (actionMenuToggle == false)
                    {
                        shipControls.SelectFwdObject();

                        shipControls.PlayerRotate();


                        shipControls.PlayerPitch();

                        shipControls.PlayerYaw();
   
                        shipControls.PlayerStrafeXaxis();

                        shipControls.PlayerStrafeYaxis();

                        shipControls.PlayerUpdateThrottle();
                    }
                    //stablization of roll pitch and yaw input//------------------------------
                    shipControls.UpdateJoystick();
                    shipControls.PlayerMouseStableize();

                    shipControls.UpdateVelocity();
                    shipControls.PlayerStableizeRoll();

                    shipControls.PlayerStableizePitch();

                    shipControls.PlayerStableizeYaw();

                    shipControls.PlayerSlowStop();

                    shipControls.PlayerStableizeYDrift();

                    shipControls.PlayerStableizeXDrift();

                }
                //end of ship input//-----------------------------------------------------
                break;
            case PlayerControlMode.onfoot:
                break;
            case PlayerControlMode.spacewalk:
                break;
            default:
                break;
        }
    }

    public void ToggleActionMenu()
    {
        if (actionMenuToggle == true)
        {
            
            actionMenuToggle = false;
            actionMenu.SetActive(false);
        }
        else
        {
            mouse.mouse_reset();
            actionMenuToggle = true;
            actionMenu.SetActive(true);
        }
        ToggleMouseAim();
    }
    public void ToggleMouseAim()
    {
        if (actionMenuToggle != true)
        {
            if (mouse.EngagedMouse == true)
            {
                Cursor.lockState = CursorLockMode.None;
                mouse.EngagedMouse = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                mouse.EngagedMouse = true;
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            mouse.EngagedMouse = false;
        }
    }

}
