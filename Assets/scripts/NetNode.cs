using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetNode : MonoBehaviour
{
    //Extention to the NetComponent ID of this prefab
    public string NodeID;
    public List<string> NodeIDs = new List<string>();

    private void Update()
    {

    }
    public void SetHierarchy()
    {
        Debug.Log("SetHierarchy");
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).GetComponent<NetComponent>().parentID = NodeID;
            gameObject.transform.GetChild(i).GetComponent<NetComponent>().netID = NodeID + i.ToString() + '/';
            NodeIDs.Add(gameObject.transform.GetChild(i).GetComponent<NetComponent>().netID);
        }

    }
    //will need to check for this component if NetComponent Is not on Parent

    //this wil allow for Spawning Objects Under This Transform
}
