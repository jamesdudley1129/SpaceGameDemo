using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Network : MonoBehaviour
{


    WorldTracker worldTracker;
    NetworkAPI networkAPI;
    Client client;
    HostClient host;
    public bool IsHost = false;
    public bool multiplayer_enabled = false;

    public List<NetObject> netData = new List<NetObject>();//this variable will contain data that needs to be pushed to the other endpoint
    public List<string> DeletedObjects = new List<string>();
    List<NetComponent> netComponents = new List<NetComponent>();

    private void Start()
    {
        networkAPI = GetComponent<NetworkAPI>();
        worldTracker = GetComponent<WorldTracker>();
        
    }

    private void Update()
    {
        if (multiplayer_enabled)
        {

            AssignSendableObjects();//temp process for finding out what needs to be send perferibly world tracker would assign 
            NetworkConnection();
        }
    }
    public void NetworkConnection()
    {


        if (IsHost == true)
        {

            if (host == null)
            {
                host = new HostClient();
            }
            if (host.connected == false)
            {
                host.Listen();
                //Debug.Log("Listening");
                host.FirstReceive();
                //Debug.Log("First. Sending");
            }
            if (host.remoteEP != null)
            {

                HostActions();
                //host.Pong();
            }
        }
        else
        {
            if (client == null)
            {
                client = new Client();
                client.cleanedUp = false;
            }
            if (client.connected == false)
            {
                client.Connect();
                //Debug.Log("Sending connection request");
                client.ConnectResponse();
                //Debug.Log("reciveing connection response");
            }
            if (client.remoteEP != null)
            {

                ClientActions();
                //client.Ping();
            }

        }

    }
    public void RECVDestroy(List<string> ids)
    {
        foreach (string id in ids)
        {
            for (int i = 0; i < worldTracker.world.Count; i++)
            {
                GameObject go = worldTracker.world[i];
                if (go != null)
                {
                    if (id == go.GetComponent<NetComponent>().netID)
                    {
                        worldTracker.AddToDestroyQue(go);
                    }
                }
            }
        }
    }
    public void AssignSendableObjects()//temp process for finding what needs to be sent
    {
        netData.Clear();
        //can filter client only needs to send objects with a id starting with CLIENT
        //could also do the same for Host but idk if thats a good ideal to keep everything synced
        if (worldTracker.player != null)
        {
            NetComponent playerCmp = worldTracker.player.GetComponent<NetComponent>();
            string Parent = null;
            List<string> SubNodeIDs = new List<string>();
            SubNodeIDs = playerCmp.NodeIDs;
            if (playerCmp.gameObject.transform.parent.GetComponent<NetComponent>() != null)
            {
                Parent = playerCmp.gameObject.transform.parent.GetComponent<NetComponent>().netID;

            }
            else if (playerCmp.gameObject.transform.parent.GetComponent<NetNode>() != null)
            {
                Parent = playerCmp.gameObject.transform.parent.GetComponent<NetNode>().NodeID;
            }

            if (Parent != null)
            {
                netData.Add(new NetObject(Parent, playerCmp.prefab, playerCmp.netID, playerCmp.worldPos, playerCmp.worldRot, SubNodeIDs));
            }
            else
            {
                netData.Add(new NetObject("NULL", playerCmp.prefab, playerCmp.netID, playerCmp.worldPos, playerCmp.worldRot, SubNodeIDs));
            }
        }
        if (IsHost)
        {
            foreach (GameObject gameObject in worldTracker.world)
            {
                string Parent = null;
                List<string> SubNodeIDs = new List<string>();
                //Debug.Log(gameObject.GetInstanceID())
                if (gameObject != null)
                {

                    if (gameObject.transform.parent.GetComponent<NetComponent>() != null)
                    {
                        Parent = gameObject.transform.parent.GetComponent<NetComponent>().netID;

                    }
                    else if (gameObject.transform.parent.GetComponent<NetNode>() != null)
                    {
                        Parent = gameObject.transform.parent.GetComponent<NetNode>().NodeID;
                    }
                    NetComponent netComponent = gameObject.GetComponent<NetComponent>();
                    SubNodeIDs = netComponent.NodeIDs;
                    if (netComponent.netID.StartsWith("HOST") || netComponent.netID.StartsWith("AI"))
                    {
                        if (Parent != null)
                        {
                            netData.Add(new NetObject(Parent, netComponent.prefab, netComponent.netID, netComponent.worldPos, netComponent.worldRot, SubNodeIDs));

                        }
                        else
                        {
                            netData.Add(new NetObject("NULL", netComponent.prefab, netComponent.netID, netComponent.worldPos, netComponent.worldRot, SubNodeIDs));
                        }
                    }
                }
            }
        }
        else
        {
            foreach (GameObject gameObject in worldTracker.world)
            {
                string Parent = null;
                List<string> SubNodeIDs = new List<string>();
                //Debug.Log(gameObject.GetInstanceID())
                if (gameObject != null)
                {
                    if (gameObject.transform.parent.GetComponent<NetComponent>() != null)
                    {
                        Parent = gameObject.transform.parent.GetComponent<NetComponent>().netID;

                    }
                    else if (gameObject.transform.parent.GetComponent<NetNode>() != null)
                    {
                        Parent = gameObject.transform.parent.GetComponent<NetNode>().NodeID;
                    }
                    NetComponent netComponent = gameObject.GetComponent<NetComponent>();
                    SubNodeIDs = netComponent.NodeIDs;
                    if (netComponent.netID.StartsWith("CLIENT"))
                    {
                        if (Parent != null)
                        {
                            netData.Add(new NetObject(Parent, netComponent.prefab, netComponent.netID, netComponent.worldPos, netComponent.worldRot, SubNodeIDs));

                        }
                        else
                        {
                            netData.Add(new NetObject("NULL", netComponent.prefab, netComponent.netID, netComponent.worldPos, netComponent.worldRot, SubNodeIDs));
                        }
                    }
                }
            }
        }
    }
    public void HostActions()
    {
        //allways send gameobjects excludeing client player
        //sending section
        foreach (NetObject netObject in netData)
        {
            string msg = networkAPI.Serialize_GameObject(netObject);
            host.Send(networkAPI.PostFormat(msg), 2024);
            //maybe shoud recive a AlirtSayingPacketRecived
        }
        foreach (string ID in DeletedObjects)
        {
            string command = networkAPI.PostDeletedObjects(ID);
            host.Send(command, 2024);
        }
        DeletedObjects.Clear();
        host.Send(networkAPI.POST_EndOfPackets(), 2024);
        //maybe shoud recive a AlirtSayingPacketRecived
        //endof sending section

        //reciveing section
        bool endOfTransmission = false;
        List<NetObject> RECV_NetObjects = new List<NetObject>();
        List<string> RECV_DestroyID = new List<string>();
        while (endOfTransmission == false)
        {

            string msg = "";
            msg = host.Receive(2024);
            // maybe should send a AlirtSayingPacketRecived
            MSG_Type mSG_Type = networkAPI.SortIncomming(msg);
            UpdateType updateType = networkAPI.SubjectSorter(msg);
            if (mSG_Type == MSG_Type.POST)
            {
                if (updateType == UpdateType.GameObject)
                {
                    RECV_NetObjects.Add(networkAPI.Deserialize_GameObject(msg));
                }
                if (updateType == UpdateType.buffer)
                {

                }
                if (updateType == UpdateType.Error)
                {
                    Debug.LogWarning("error type recived in msg: " + msg);
                }
                if (updateType == UpdateType.EndOfPackets)
                {
                    endOfTransmission = true;
                }
                if (updateType == UpdateType.Destroy)
                {
                    RECV_DestroyID.Add(networkAPI.ReadDestroyID(msg));
                }
            }
            if (mSG_Type == MSG_Type.GET)//unsure of gets purpose atm maybe will use for requesting packet size??
            {

            }
            if (mSG_Type == MSG_Type.ERROR)
            {
                Debug.LogWarning("error msg recived: " + msg);
            }

            //end of else
        }
        //endof reciveing section       
        UpdateHostGame(RECV_NetObjects);
        RECVDestroy(RECV_DestroyID);

    }

    public void ClientActions()
    {
        //client Recives
        bool endOfTransmission = false;
        List<NetObject> RECV_NetObjects = new List<NetObject>();
        List<string> RECV_DestroyID = new List<string>();
        while (endOfTransmission == false)
        {

            string msg = "";
            msg = client.Receive(2024);
            //maybe shoud send a AlirtSayingPacketRecived
            MSG_Type mSG_Type = networkAPI.SortIncomming(msg);
            UpdateType updateType = networkAPI.SubjectSorter(msg);
            if (mSG_Type == MSG_Type.POST)
            {
                if (updateType == UpdateType.GameObject)
                {
                    RECV_NetObjects.Add(networkAPI.Deserialize_GameObject(msg));
                }
                if (updateType == UpdateType.buffer)
                {

                }
                if (updateType == UpdateType.Error)
                {
                    Debug.LogWarning("error type recived in msg: " + msg);
                }
                if (updateType == UpdateType.EndOfPackets)
                {
                    endOfTransmission = true;
                }
                if (updateType == UpdateType.Destroy)
                {
                    RECV_DestroyID.Add(networkAPI.ReadDestroyID(msg));
                }
            }
            if (mSG_Type == MSG_Type.GET)//unsure of gets purpose atm maybe will use for requesting packet size??
            {

            }
            if (mSG_Type == MSG_Type.ERROR)
            {
                Debug.LogWarning("error msg recived: " + msg);
            }

            //end of else
        }

        //allways send player excludeing other game objects unless there made then handover there info to the server to take control
        foreach (NetObject netObject in netData)
        {
            string msg = networkAPI.Serialize_GameObject(netObject);
            client.Send(networkAPI.PostFormat(msg), 2024);
            //maybe shoud recive a AlirtSayingPacketRecived
        }
        foreach (string ID in DeletedObjects)
        {
            string command = networkAPI.PostDeletedObjects(ID);
            client.Send(command, 2024);
        }
        DeletedObjects.Clear();
        client.Send(networkAPI.POST_EndOfPackets(), 2024);
        //maybe shoud recive a AlirtSayingPacketRecived

        UpdateClientGame(RECV_NetObjects);
        RECVDestroy(RECV_DestroyID);
    }

    public void UpdateHostGame(List<NetObject> net_obj_data)
    {
        List<NetObject> notfound = new List<NetObject>();
        foreach (NetObject netObject in net_obj_data)
        {
            NetComponent net_component = null;
            //will only update player controlled data in the future (bullet created)
            foreach (GameObject item in worldTracker.world)//updates world objects
            {
                if (item != null)
                {
                    if (item.GetComponent<NetComponent>().netID == netObject._ID)
                    {
                        net_component = item.GetComponent<NetComponent>();
                    }
                }
            }
            if (worldTracker.OtherPlayer != null)//updates ither player (players are not in world list)
            {
                if (netObject._ID == worldTracker.OtherPlayer.GetComponent<NetComponent>().netID)
                {
                    net_component = worldTracker.OtherPlayer.GetComponent<NetComponent>();
                }
            }

            if (net_component != null)
            {
                net_component.AssignNewData(netObject._Position, netObject._Rotation.eulerAngles);
            }
            else if (netObject._ID != worldTracker.player.GetComponent<NetComponent>().netID)
            {
                notfound.Add(netObject);
            }

        }
        foreach (NetObject net in notfound)
        {
            if (net._ID.StartsWith("CLIENT"))
            {
                worldTracker.AddToNetSpawnQue(net);
            }
        }

    }

    public void UpdateClientGame(List<NetObject> net_obj_data)
    {
        List<NetObject> notfound = new List<NetObject>();
        foreach (NetObject netObject in net_obj_data)
        {
            NetComponent net_component = null;
            //will only update player controlled data in the future (bullet created)
            foreach (GameObject item in worldTracker.world)
            {
                if (item != null)
                {
                    if (item.GetComponent<NetComponent>().netID == netObject._ID)
                    {
                        net_component = item.GetComponent<NetComponent>();
                    }
                }
            }

            if (worldTracker.OtherPlayer != null)
            {
                if (netObject._ID == worldTracker.OtherPlayer.GetComponent<NetComponent>().netID)
                {
                    net_component = worldTracker.OtherPlayer.GetComponent<NetComponent>();
                }
            }

            if (net_component != null)
            {
                net_component.AssignNewData(netObject._Position, netObject._Rotation.eulerAngles);
            }
            else if (netObject._ID != worldTracker.player.GetComponent<NetComponent>().netID)
            {
                if(netObject._ID == "")
                {
                    Debug.LogWarning("NetObject<" + netObject._ID + "> does not have a ID");
                }
                else
                {
                    notfound.Add(netObject);
                }
                
            }
        }
        foreach (NetObject net in notfound)
        {
            if (net._ID.StartsWith("HOST"))
            {
                worldTracker.AddToNetSpawnQue(net);
            }
        }
    }
}

