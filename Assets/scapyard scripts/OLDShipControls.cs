using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class OLDShipControls : MonoBehaviour
{
    
    public bool is_player = false;
    public bool is_client_player = false;
    public Player player;
    //public List<GameObject> engines;
    //public List<GameObject> maneuveringTrusters;
    public GameObject target;
    public GameObject celestial_Target;
    public bool StableFlightAssist;
    public bool combatAssist;
    public bool targetRendezvousMode;
    public GameObject GameManager;
    public ShipSpecs shipspecs;
    public Rigidbody ship;
    public Vector3 velocity;
    public Vector3 angularVelocity;


    [SerializeField]
    [Range(0f, 1f)]
    public float rotateSensitivityX;
    [Range(0f, 1f)]
    public float rotateSensitivityY;
    [Range(0f, 1f)]
    public float rotateSensitivityZ;
    [Range(-50f, 100f)]
    public float throttle;

    //universe moves around the player player stays at 0,0,0 (Future Implication)
    void Awake()
    {

        shipspecs = GetComponent<ShipSpecs>();
        ship = gameObject.GetComponent<Rigidbody>();
        ship.maxAngularVelocity = float.PositiveInfinity;



    }

    private void FixedUpdate()
    {

        if (is_player)
        {
            angularVelocity = transform.InverseTransformDirection(ship.angularVelocity);
            //velocity = transform.InverseTransformDirection(ship.velocity);



            if (throttle == 0 && StableFlightAssist == true)
            {
                PlayerSlowstop();

            }
            else
            {
                PlayerThrottleUpdate();

            }


        }
        else
        {
            angularVelocity = transform.InverseTransformDirection(ship.angularVelocity);
            velocity = transform.InverseTransformDirection(ship.velocity);
            if (throttle == 0 && StableFlightAssist == true)
            {
                Slowstop();
            }
            else
            {
                ThrottleUpdate();
            }
        }
    }
    //functions for all
    public void StableizeRotation()
    {
        if (angularVelocity.z >= shipspecs.ManeuverThrusterForce || angularVelocity.z <= -shipspecs.ManeuverThrusterForce)
        {
            if (angularVelocity.z >= shipspecs.ManeuverThrusterForce)
            {
                ship.AddRelativeTorque(Vector3.forward * -shipspecs.ManeuverThrusterForce * rotateSensitivityZ * Time.deltaTime, ForceMode.Impulse);
            }
            else
            {
                ship.AddRelativeTorque(Vector3.forward * shipspecs.ManeuverThrusterForce * rotateSensitivityZ * Time.deltaTime, ForceMode.Impulse);
            }
        }
        else
        {
            ship.AddRelativeTorque(Vector3.forward * -angularVelocity.z * Time.deltaTime, ForceMode.Impulse);
        }
    }
    public void StableizePitch()
    {
        if (angularVelocity.x >= shipspecs.ManeuverThrusterForce || angularVelocity.x <= -shipspecs.ManeuverThrusterForce)
        {
            if (angularVelocity.x >= shipspecs.ManeuverThrusterForce)
            {
                ship.AddRelativeTorque(Vector3.right * -shipspecs.ManeuverThrusterForce * rotateSensitivityX * Time.deltaTime, ForceMode.Impulse);
            }
            else
            {
                ship.AddRelativeTorque(Vector3.right * shipspecs.ManeuverThrusterForce * rotateSensitivityX * Time.deltaTime, ForceMode.Impulse);
            }
        }
        else
        {
            ship.AddRelativeTorque(Vector3.right * -angularVelocity.x * Time.deltaTime, ForceMode.Impulse);
        }
    }
    public void StableizeYaw()
    {
        if (angularVelocity.y >= shipspecs.ManeuverThrusterForce || angularVelocity.y <= -shipspecs.ManeuverThrusterForce)
        {
            if (angularVelocity.y >= shipspecs.ManeuverThrusterForce)
            {
                ship.AddRelativeTorque(Vector3.up * -shipspecs.ManeuverThrusterForce * rotateSensitivityY * Time.deltaTime, ForceMode.Impulse);
            }
            else
            {
                ship.AddRelativeTorque(Vector3.up * shipspecs.ManeuverThrusterForce * rotateSensitivityY * Time.deltaTime, ForceMode.Impulse);
            }
        }
        else
        {
            ship.AddRelativeTorque(Vector3.up * -angularVelocity.y * Time.deltaTime, ForceMode.Impulse);
            //ship.AddRelativeTorque(Vector3.up * (-angularVelocity.y * rotateSensitivityY) * Time.deltaTime,ForceMode.Impulse);
        }



    }
    //functions for player
    
    public void PlayerStabelizeYDrift()
    {
        if (velocity.y >= shipspecs.ManeuverThrusterForce || velocity.y <= -shipspecs.ManeuverThrusterForce)
        {
            if (velocity.y >= shipspecs.ManeuverThrusterForce)
            {
                player.playerSceneManager.Addforce(transform.up * -shipspecs.ManeuverThrusterForce * Time.deltaTime, ForceMode.Impulse);
                velocity += Vector3.up * -shipspecs.ManeuverThrusterForce * Time.deltaTime;
            }
            else
            {
                player.playerSceneManager.Addforce(transform.up * shipspecs.ManeuverThrusterForce * Time.deltaTime, ForceMode.Impulse);
                velocity += Vector3.up * -shipspecs.ManeuverThrusterForce * Time.deltaTime;
            }
        }
        else
        {
            player.playerSceneManager.Addforce(transform.up * -velocity.y * Time.deltaTime, ForceMode.Impulse);
            velocity += Vector3.up * -velocity.y * Time.deltaTime;
        }
    }
    public void PlayerStabelizeXDrift()
    {
        if (velocity.x >= shipspecs.ManeuverThrusterForce || velocity.x <= -shipspecs.ManeuverThrusterForce)
        {
            if (velocity.x >= shipspecs.ManeuverThrusterForce)
            {
                player.playerSceneManager.Addforce(transform.right * -shipspecs.ManeuverThrusterForce * Time.deltaTime, ForceMode.Impulse);
                velocity += Vector3.right * -shipspecs.ManeuverThrusterForce * Time.deltaTime;
            }
            else
            {
                player.playerSceneManager.Addforce(transform.right * shipspecs.ManeuverThrusterForce * Time.deltaTime, ForceMode.Impulse);
                velocity += Vector3.right * shipspecs.ManeuverThrusterForce * Time.deltaTime;
            }
        }
        else
        {
            player.playerSceneManager.Addforce(transform.right * -velocity.x * Time.deltaTime, ForceMode.Impulse);
            velocity += Vector3.right * -velocity.x * Time.deltaTime;
        }
    }
    
    public void PlayerSlowstop()
    {

        if (velocity.z >= shipspecs.EngineForce * 0.5f || velocity.z <= -shipspecs.EngineForce)
        {

            if (velocity.z >= shipspecs.EngineForce * 0.5f)
            {
                player.playerSceneManager.Addforce(transform.forward * -shipspecs.EngineForce * 0.5f * Time.deltaTime, ForceMode.Impulse);
                velocity += Vector3.forward * -shipspecs.EngineForce * 0.5f * Time.deltaTime;
            }
            else
            {
                player.playerSceneManager.Addforce(transform.forward * shipspecs.EngineForce * Time.deltaTime, ForceMode.Impulse);
                velocity += Vector3.forward * shipspecs.EngineForce * Time.deltaTime;
            }
        }
        else
        {
            player.playerSceneManager.Addforce(transform.forward * -velocity.z * Time.deltaTime, ForceMode.Impulse);
            velocity += Vector3.forward * -velocity.z * Time.deltaTime;

        }
    }
    public void PlayerThrottleUpdate()
    {
        player.playerSceneManager.Addforce(transform.forward * shipspecs.EngineForce * (throttle / 100) * Time.deltaTime, ForceMode.Impulse);
        velocity += transform.forward * shipspecs.EngineForce * (throttle / 100) * Time.deltaTime;

    }
    //functions for AI
    public void StabelizeYDrift()
    {
        if (velocity.y >= shipspecs.ManeuverThrusterForce || velocity.y <= -shipspecs.ManeuverThrusterForce)
        {
            if (velocity.y >= shipspecs.ManeuverThrusterForce)
            {
                ship.AddRelativeForce(Vector3.up * -shipspecs.ManeuverThrusterForce * Time.deltaTime, ForceMode.Impulse);
            }
            else
            {
                ship.AddRelativeForce(Vector3.up * shipspecs.ManeuverThrusterForce * Time.deltaTime, ForceMode.Impulse);
            }
        }
        else
        {
            ship.AddRelativeForce(Vector3.up * -velocity.y * Time.deltaTime, ForceMode.Impulse);
        }
    }
    public void StabelizeXDrift()
    {
        if (velocity.x >= shipspecs.ManeuverThrusterForce || velocity.x <= -shipspecs.ManeuverThrusterForce)
        {
            if (velocity.x >= shipspecs.ManeuverThrusterForce)
            {
                ship.AddRelativeForce(Vector3.right * -shipspecs.ManeuverThrusterForce * Time.deltaTime, ForceMode.Impulse);
            }
            else
            {
                ship.AddRelativeForce(Vector3.right * shipspecs.ManeuverThrusterForce * Time.deltaTime, ForceMode.Impulse);
            }
        }
        else
        {
            ship.AddRelativeForce(Vector3.right * -velocity.x * Time.deltaTime, ForceMode.Impulse);
        }
    }
    public void Slowstop()
    {

        if (velocity.z >= shipspecs.EngineForce * 0.5f || velocity.z <= -shipspecs.EngineForce)
        {

            if (velocity.z >= shipspecs.EngineForce * 0.5f)
            {
                ship.AddRelativeForce(Vector3.forward * -shipspecs.EngineForce * 0.5f * Time.deltaTime, ForceMode.Impulse);
            }
            else
            {
                ship.AddRelativeForce(Vector3.forward * shipspecs.EngineForce * Time.deltaTime, ForceMode.Impulse);
            }
        }
        else
        {
            ship.AddRelativeForce(Vector3.forward * -velocity.z * Time.deltaTime, ForceMode.Impulse);

        }
    }
    public void ThrottleUpdate()
    {
        ship.AddRelativeForce(Vector3.forward * shipspecs.EngineForce * (throttle / 100) * Time.deltaTime, ForceMode.Impulse);

    }

    //all objects will need to be on a virtual graph (with higher pos accuracy) for the client to have a syncernised game and the host to refrence there pos of objects when out of floating point range


}

    */
