using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSceneManager : MonoBehaviour
{
    public List<GameObject> non_Player_objects = new List<GameObject>();
    private void Update()
    {
       GameObject[] otherObjects = GameObject.FindGameObjectsWithTag("nonPlayer");
       foreach(GameObject go in otherObjects)
        {
            if (!non_Player_objects.Contains(go))
            {
                non_Player_objects.Add(go);
            }
            
        }
    }
    //called from player
    public void Addforce(Vector3 force ,ForceMode mode)
    {
        foreach(GameObject  go in non_Player_objects)
        {
            go.GetComponent<Rigidbody>().AddForce(-force, mode);
        }
    }

    /*
    public void ActiveObjectCheck()
    {
        //checks the distence of a object determining weither it should be despawned and managed over a resource document based on distence

    }
    */





}
