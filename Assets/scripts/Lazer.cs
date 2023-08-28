using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Lazer : MonoBehaviour
{
    public float Damage;
    public float maxDistance;
    
    public Vector3 hitPoint = new Vector3();
    LineRenderer lineRenderer;
    public List<GameObject> ImmuneObjects = new List<GameObject>();//assigned from lazerWeapon.cs defines the object or set of objects that fired it
    float lasttime;
    bool ishit = false;
    NetComponent this_netComponent;
    string MannagedBylocalNetworkTag = null;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lasttime = Time.time;
        hitPoint = transform.TransformPoint(Vector3.forward * maxDistance);
        this_netComponent = GetComponent<NetComponent>();
        


    }

    private void FixedUpdate()
    {
        //Debug.DrawRay(lazerBeam.origin,lazerBeam.direction, Color.white);        
        Debug.DrawLine(transform.position, transform.up,Color.red);//shows orgin
        RaycastHit hit = new RaycastHit();
        ishit = Physics.Raycast(transform.position,transform.forward, out hit, Vector3.Distance(transform.position,transform.TransformPoint(Vector3.forward * maxDistance)));
        if (ishit)
        {
            //if 1sec passed run
            Debug.DrawLine(hit.point, transform.up, Color.green);//shows hit point
            hitPoint = hit.point;
            if (!ImmuneObjects.Contains(hit.collider.gameObject))
            {
                if (Time.time - lasttime >= 1f)
                {
                    RaycastHit(hit);
                    lasttime = Time.time;
                   
                }
            }
            Debug.Log(hit.collider.gameObject.name+"||hit");
        }
        else
        {
            Debug.Log("Null Test :" + hit.point);
        }
    }
    private void Update()
    {
        if(MannagedBylocalNetworkTag == null)
        {
            MannagedBylocalNetworkTag = this_netComponent.netID.Substring(0, this_netComponent.netID.IndexOf('/') - 1);
            Debug.Log(MannagedBylocalNetworkTag);
        }
        Set_ImmuneObjects();
        if (!ishit)
        {
            hitPoint = transform.TransformPoint(Vector3.forward * maxDistance);

            //hitPoint = transform.forward * maxDistance;
        }
        SetLineRender();
    }
    public void Set_ImmuneObjects()//code wont if client tries to destroy a client object or if the host tries to destroy a object managed on the host machine 
    {
        //DONT USE NetworkID for immunity it is not a team system it is the objects managed by client or host endpoints 
        
        
        //the best way to do this would be to place the lazer infront of the player weapon so it cant hit the player

    }
    public void SetLineRender()
    {
        lineRenderer.SetPosition(1, transform.InverseTransformPoint(hitPoint));
    }
    public void RaycastHit(RaycastHit hit)
    {

        lasttime = Time.time;

        if (hit.collider.gameObject.layer == 8)
        {

            Debug.Log(hit.collider.gameObject.GetComponent<NetComponent>().netID);
            ObjectStatus status = hit.collider.gameObject.GetComponent<ObjectStatus>();
            if (Damage >= status.Shield)
            {
                float hp_damage = Damage - status.Shield;
                status.Shield = 0f;
                status.HP -= hp_damage;
            }
            else
            {
                status.Shield -= Damage;
            }

        }

    }
}
