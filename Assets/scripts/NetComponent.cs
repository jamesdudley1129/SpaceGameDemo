using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetComponent : MonoBehaviour
{
    //can assign List Of Child NetNodes IDs (i would have to come up with a diffrent way to Deserilize this 
    //network ID <assigned by host Network : GameManager.WorldTracker || on runtime by client (EX:new bullet)>
    public string parentID;
    public string netID;
    public Vector3 worldPos;
    public Vector3 worldRot;
    public GameObject Asset;
    public string prefab;
    public List<GameObject> AttachmentNodes = new List<GameObject>();
    public List<string> NodeIDs = new List<string>();
    private void Awake()
    {
        prefab = Asset.name;
        if (prefab.Contains("(Clone)") == true)
        {
            prefab = prefab.Remove(prefab.IndexOf("(Clone)", 0));
        }

    }

    public void Update()
    {
        worldPos = transform.position;
        worldRot = transform.rotation.eulerAngles;

        //if (net.NodeIDs.Count != net.AttachmentNodes.Count)
        if(transform.childCount < NodeIDs.Capacity)
        {
            NodeIDs.Clear();
        }

    }
    public void SetHierarchy()
    {
        
        for (int i = 0; i < AttachmentNodes.Count; i++)
        {
            if(AttachmentNodes[i] != gameObject)
            { 
                if (AttachmentNodes[i].GetComponent<NetComponent>() != null)
                {

                    AttachmentNodes[i].GetComponent<NetComponent>().netID = netID + i.ToString() + '/';
                    NodeIDs.Add(AttachmentNodes[i].GetComponent<NetComponent>().netID);
                }
                if (AttachmentNodes[i].GetComponent<NetNode>() != null)
                {
                    AttachmentNodes[i].GetComponent<NetNode>().NodeID = netID + i.ToString() + '/';
                    NodeIDs.Add(AttachmentNodes[i].GetComponent<NetNode>().NodeID);
                }
            }
            else
            {
                for (int i2 = 0; i2 < gameObject.transform.childCount; i2++)
                {
                    GameObject child = gameObject.transform.GetChild(i2).gameObject;
                    if (child.GetComponent<NetComponent>() != null)
                    {
                        child.GetComponent<NetComponent>().parentID = netID;
                        child.GetComponent<NetComponent>().netID = netID + i.ToString() + '/';                        
                        NodeIDs.Add(child.GetComponent<NetComponent>().netID);
                    }
                    if (child.GetComponent<NetNode>() != null)
                    {
                        child.GetComponent<NetNode>().NodeID = netID + i.ToString() + '/';
                        NodeIDs.Add(child.GetComponent<NetNode>().NodeID);
                    }
                }
            }
        }

    }
    public void AssignNewData(Vector3 pos, Vector3 rot)
    {
        transform.position = pos;
        transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
    }
}
