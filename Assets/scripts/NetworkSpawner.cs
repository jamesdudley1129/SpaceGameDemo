using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkSpawner : MonoBehaviour
{
    public List<GameObject> Prefabs = new List<GameObject>();
    public Dictionary<string, GameObject> Asset = new Dictionary<string, GameObject>();

    private void Awake()
    {
        for (int i = 0; i < Prefabs.Count; i++)
        {
            Asset.Add(Prefabs[i].name, Prefabs[i]);
        }
    }

    public bool Spawn(string ParentID, string ID, string prefab_name, Vector3 position, Quaternion rotation, List<string> SubNodeIDs)
    {
        bool canSpawn = true;
        Transform ParentTransform = transform;
        if (ParentID != "NULL")
        {
            foreach (GameObject parent in GetComponent<WorldTracker>().everything)
            {
                if (parent.GetComponent<NetComponent>() != null)
                {
                    if (parent.GetComponent<NetComponent>().netID == ParentID)
                    {
                        //Debug.Log("ParentID =" + ParentID);
                        ParentTransform = parent.transform;
                        canSpawn = true;
                        break;
                    }
                }
                else if (parent.GetComponent<NetNode>() != null)
                {
                    //Debug.Log("goal Node ID:"+ ParentID+" Recived:" +parent.GetComponent<NetNode>().NodeID);
                    if (parent.GetComponent<NetNode>().NodeID == ParentID)
                    {
                        //Debug.Log("ParentID =" + ParentID);
                        ParentTransform = parent.transform;
                        canSpawn = true;
                        break;
                    }
                }
                else
                {
                    canSpawn = false;
                    //cant spawn becouse parrent doesnt exist yet
                }
                //check NetNode if ParentTransform Not Found
            }
        }
        if (canSpawn == true)
        {
            if (Asset.TryGetValue(prefab_name, out GameObject gameObject))
            {
                gameObject = Instantiate(gameObject, position, rotation, ParentTransform);
                Debug.Log(gameObject.name);
                gameObject.GetComponent<NetComponent>().netID = ID;
                //set its SubNodeIds
                gameObject.GetComponent<NetComponent>().NodeIDs = SubNodeIDs;
            }
            else
            {
                Debug.LogWarning("prefab name = " + prefab_name + "/n Error createing object");
            }
        }
        else
        {
            //Debug.Log("CantSpawn<" + ID + "> PID:" + ParentID);
        }
        return canSpawn;
    }


}
