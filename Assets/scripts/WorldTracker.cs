using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTracker : MonoBehaviour
{
    //world will exclude player
    //consider using rigidbodys instead of gameobjects for list
    public List<GameObject> world = new List<GameObject>();
    public List<GameObject> everything = new List<GameObject>();
    public List<GameObject> _DestroyGameObject = new List<GameObject>();
    public List<NetObject> _SpawnNetObject = new List<NetObject>();
    NetworkSpawner netSpawner;
    LocalSpawner localSpawner;
    public GameObject _God;
    public GameObject player;
    public GameObject OtherPlayer;
    int emergincy_Exit = 0;
    public bool Loaded = false;
    //temp code
    public GameObject camrea;
    public GameObject PlayerAsset;
    public GameObject SpawnObject;
    public GameObject lazorCannonMk1;
    public GameObject radarComponent;
    //end of temp
    private void Awake()
    {
        netSpawner = GetComponent<NetworkSpawner>();
        localSpawner = GetComponent<LocalSpawner>();
    }

    private void Update()
    {
        DestroyGameObject();
        if (Loaded)
        {
            localSpawner.SpawnPlayer();
            SpawnNetObject();
            UpdateCollections();
            UpdateHierarchy();
        }
    }
    public void StartGame()
    {
        Loaded = true;
        localSpawner.SpawnPlayer();
        UpdateCollections();
        UpdateHierarchy();


    }
    public void UpdateHierarchy()
    {
        if (Loaded)
        {
            foreach (GameObject child in everything)
            {
                if (child != null)
                {
                    if (child.GetComponent<NetComponent>() != null)
                    {
                        NetComponent net = child.GetComponent<NetComponent>();
                        if (net.NodeIDs.Count != net.AttachmentNodes.Count)
                        {
                            net.SetHierarchy();
                        }
                    }
                    else if (child.GetComponent<NetNode>() != null)
                    {
                        NetNode net = child.GetComponent<NetNode>();
                        if (net.NodeIDs.Count != net.gameObject.transform.childCount)
                        {
                            net.SetHierarchy();
                        }
                    }
                }
                else { Debug.Log("? object null in everything NullRemove was already called"); }
            }
        }
    }
    public void SpawnNetObject()
    {
        foreach (NetObject net in _SpawnNetObject)
        {
            //Debug.Log(net._ParentID);
            netSpawner.Spawn(net._ParentID, net._ID, net._PrefabName, net._Position, net._Rotation, net.SubNodeIDs);
        }
        _SpawnNetObject.Clear();
    }
    public void DestroyGameObject()
    {
        int count = _DestroyGameObject.Count;
        List<GameObject> newWorld = new List<GameObject>();
        List<GameObject> newEverything = new List<GameObject>();
        foreach (GameObject item in world)
        {
            if (item != null)
            {
                if (!_DestroyGameObject.Contains(item))
                {
                    newWorld.Add(item);
                }
            }
        }
        foreach (GameObject item in everything)
        {
            if (item != null)
            {
                if (!_DestroyGameObject.Contains(item))
                {
                    newEverything.Add(item);
                }
            }
        }
        world = newWorld;
        everything = newEverything;
        foreach (GameObject destroy in _DestroyGameObject)
        {
            bool resetCam = false;
            if(destroy == player)//temp
            {
                camrea.transform.SetParent(transform);
                resetCam = true;
            }

            string id = destroy.GetComponent<NetComponent>().netID;
            Destroy(destroy);
            GetComponent<Network>().DeletedObjects.Add(id);
            if (resetCam)
            {
                camrea.GetComponent<CameraBeh>().CamreaSet = false;
            }
        }
        _DestroyGameObject.Clear();
    }
    public void AddToDestroyQue(GameObject go)
    {
        _DestroyGameObject.Add(go);
    }
    public void AddToNetSpawnQue(NetObject NetworkObject)
    {
        _SpawnNetObject.Add(NetworkObject);
        
    }

    public void GrabEverything(Transform level)
    {

        List<Transform> sibleings = new List<Transform>();
        if (level.childCount > 0)
        {
            foreach (Transform child in level)
            {
                //condition for testing i think objects that are null are being added to everything
                if (child != null)
                {
                    sibleings.Add(child);
                    if (!everything.Contains(child.gameObject))
                    {
                        everything.Add(child.gameObject);
                    }
                }
            }
            foreach (Transform lowerlevel in sibleings)
            {
                GrabEverything(lowerlevel);
            }
        }

    }

    public void UpdateCollections()
    {
        GrabEverything(transform);
        foreach (GameObject child in everything)
        {
            if (child != null)
            {
                if (child.tag == "Player")
                {
                    if (Loaded == true)
                    {
                        if (child.GetComponent<ShipControls>().player_control)
                        {
                            player = child;
                            child.GetComponent<GameManagerRefrence>().SetWorldTracker(this);
                            if (GetComponent<Network>().IsHost == true)
                            {
                                player.GetComponent<NetComponent>().netID = "HOST" + '/';
                            }
                            if (GetComponent<Network>().IsHost == false)
                            {
                                player.GetComponent<NetComponent>().netID = "CLIENT" + '/';
                            }
                        }
                        else
                        {
                            OtherPlayer = child;
                            child.GetComponent<GameManagerRefrence>().SetWorldTracker(this);
                        }
                    }
                }

                else if (child.tag == "AI")
                {
                    //ID_Generator(child,"AI");
                }
                //else if ("NULL"?) debug objects not working over network cannot be tagged host or clients sence they dont have the ability to freindly fire need to fix

                else if (player != null)//only need to check becouse prefix can defualt to player.id !need to change later
                {

                    if (child.tag != "Untagged")
                    {
                        string ID = "";
                        //?
                        /*
                        if (child.transform.parent.GetComponent<NetComponent>() != null)
                        {
                            Prefix = child.transform.parent.GetComponent<NetComponent>().netID;
                        }
                        else if (child.transform.parent.GetComponent<NetNode>() != null)
                        {
                            Prefix = child.transform.parent.GetComponent<NetNode>().NodeID;
                        }
                        */
                        //end of ?
                        if (child.GetComponent<NetComponent>() != null)
                        {
                            ID = child.GetComponent<NetComponent>().netID;
                        }

                        if (ID == "")
                        {
                            world.Add(child);
                            child.GetComponent<GameManagerRefrence>().SetWorldTracker(this);
                            ID_Generator(child, player.GetComponent<NetComponent>().netID);
                        }
                        else
                        {
                            if (child.GetComponent<NetComponent>() != null)
                            {
                                if (!world.Contains(child))
                                {
                                    world.Add(child);
                                    child.GetComponent<GameManagerRefrence>().SetWorldTracker(this);
                                }
                            }
                        }
                        //child.GetComponent<GameManagerRefrence>().SetWorldTracker(this);
                        //world.Add(child);
                    }

                }
            }
            else
            {
                Debug.Log("? object null in everything NullRemove was already called");
            }
        }
    }

    public void ID_Generator(GameObject obj, string IDprefix)//Need To Create Script Containing OwnerShip for Example AI or HOST or CLIENT if isHost!= true 
    {
        NetComponent net = obj.GetComponent<NetComponent>();
        List<string> failedID = new List<string>();

        do
        {

            net.netID = IDprefix + Random.Range(100000000f, 999999999f).ToString();

            foreach (GameObject world_obj in everything)
            {
                if (world_obj.GetComponent<NetComponent>() != null)
                {
                    if (world_obj.GetComponent<NetComponent>().netID == net.netID)
                    {
                        failedID.Add(net.netID);
                    }
                }
                else if (world_obj.GetComponent<NetNode>() != null)
                {
                    if (world_obj.GetComponent<NetNode>().NodeID == net.netID)
                    {
                        failedID.Add(net.netID);
                    }
                }
            }
            if (emergincy_Exit >= everything.Count) break;
            emergincy_Exit++;
        } while (failedID.Contains(net.netID));
        if (emergincy_Exit >= everything.Count)
        {
            Debug.Log(emergincy_Exit);
        }


    }

}
