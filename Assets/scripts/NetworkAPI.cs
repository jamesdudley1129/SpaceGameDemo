using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.Serialization;

//access the world from here and not Networking?
public class NetworkAPI : MonoBehaviour
{
    public string Serialize_GameObject(NetObject netObject)
    {

        string position = "{" +
            netObject._Position.x.ToString() + "," +
            netObject._Position.y.ToString() + "," +
            netObject._Position.z.ToString() +
            "}";
        string rotation = "{" +
            netObject._Rotation.x.ToString() + "," +
            netObject._Rotation.y.ToString() + "," +
            netObject._Rotation.z.ToString() + "," +
            netObject._Rotation.w.ToString() +
            "}";
        string SubNetIDs = "{";
        for (int i = 0; i < netObject.SubNodeIDs.Count; i++)
        {
            SubNetIDs += "ID:" +
                netObject.SubNodeIDs[i];
            if (i < netObject.SubNodeIDs.Count)
            {
                SubNetIDs += ",";
            }
        }
        SubNetIDs += "}";
        return ("object:" +
            "{" +
            "ParentID:" + netObject._ParentID + "," +
            "ID:" + netObject._ID + "," +
            "PreFabName:" + netObject._PrefabName + "," +
            "Position:" + position + "," +
            "Rotation:" + rotation + "," +
            "SubNetIDs:" + SubNetIDs +
            "}");
        //returns example >> object:{ID:120391,PreFabName:objectA,Position:{10,2,4},Rotation{2,3,4,1}}
    }
    //called after updatetype has been determend as object type
    public NetObject Deserialize_GameObject(string _inputObject)
    {
        //input example >> POST:object:{ID:120391,PreFabName:objectA,Position:{10,2,4},Rotation{ 2,3,4,1}}
        string ParentID = "";
        string ID = "";
        string PrefabName = "";
        Vector3 pos = new Vector3();
        Vector3 rot = new Vector3();
        List<string> SubNetIDs = new List<string>();
        _inputObject = _inputObject.Remove(0, _inputObject.IndexOf('{') + 1);
        _inputObject = _inputObject.Remove(_inputObject.LastIndexOf('}'));
        //_inputExample after change ParentID:12353463,ID:120391,PreFabName:objectA,Position:{10,2,4},Rotation{ 2,3,4,1}
        if (_inputObject.StartsWith("ParentID"))
        {
            //remove TAG
            _inputObject = _inputObject.Remove(0, _inputObject.IndexOf(":") + 1);
            //set value of ID to everything from beggining to ','
            ParentID = _inputObject.Substring(0, _inputObject.IndexOf(','));
            _inputObject = _inputObject.Remove(0, _inputObject.IndexOf(',') + 1);
        }
        if (_inputObject.StartsWith("ID"))
        {
            //remove TAG
            _inputObject = _inputObject.Remove(0, _inputObject.IndexOf(":") + 1);
            //set value of ID to everything from beggining to ','
            ID = _inputObject.Substring(0, _inputObject.IndexOf(','));
            _inputObject = _inputObject.Remove(0, _inputObject.IndexOf(',') + 1);
        }
        if (_inputObject.StartsWith("PreFabName"))
        {
            //remove TAG
            _inputObject = _inputObject.Remove(0, _inputObject.IndexOf(":") + 1);
            //set value of PreFab to everything from beggining to ','
            PrefabName = _inputObject.Substring(0, _inputObject.IndexOf(','));
            _inputObject = _inputObject.Remove(0, _inputObject.IndexOf(',') + 1);
        }
        if (_inputObject.StartsWith("Position"))
        {
            float x;
            float y;
            float z;
            string subsection;
            //remove ID
            _inputObject = _inputObject.Remove(0, _inputObject.IndexOf(":") + 1);
            //set value of ID to everything drom beggining to "},"
            subsection = _inputObject.Substring(0, _inputObject.IndexOf('}') + 1);
            subsection = subsection.Remove(0, subsection.IndexOf('{') + 1);
            float.TryParse(subsection.Substring(0, subsection.IndexOf(',')), out x);
            subsection = subsection.Remove(0, subsection.IndexOf(',') + 1);
            float.TryParse(subsection.Substring(0, subsection.IndexOf(',')), out y);
            subsection = subsection.Remove(0, subsection.IndexOf(',') + 1);
            float.TryParse(subsection.Substring(0, subsection.IndexOf('}')), out z);

            _inputObject = _inputObject.Remove(0, _inputObject.IndexOf("},") + +2);
            pos.Set(x, y, z);

        }
        if (_inputObject.StartsWith("Rotation"))
        {
            float x;
            float y;
            float z;
            float w;
            string subsection;
            //remove ID
            _inputObject = _inputObject.Remove(0, _inputObject.IndexOf(":") + 1);
            //set value of ID to everything drom beggining to "},"
            subsection = _inputObject.Substring(0, _inputObject.IndexOf('}') + 1);
            subsection = subsection.Remove(0, subsection.IndexOf('{') + 1);
            float.TryParse(subsection.Substring(0, subsection.IndexOf(',')), out x);
            subsection = subsection.Remove(0, subsection.IndexOf(',') + 1);
            float.TryParse(subsection.Substring(0, subsection.IndexOf(',')), out y);
            subsection = subsection.Remove(0, subsection.IndexOf(',') + 1);
            float.TryParse(subsection.Substring(0, subsection.IndexOf(',')), out z);
            subsection = subsection.Remove(0, subsection.IndexOf(',') + 1);
            float.TryParse(subsection.Substring(0, subsection.IndexOf('}')), out w);

            _inputObject = _inputObject.Remove(0, _inputObject.IndexOf("}") + 1);
            //Debug.Log(x + "," + y + "," + z + "," + w);
            rot = new Quaternion(x, y, z, w).eulerAngles;
        }
        if (_inputObject.StartsWith("SubNetIDs"))
        {

            string subsection;
            _inputObject = _inputObject.Remove(0, _inputObject.IndexOf(":") + 1);
            subsection = _inputObject.Substring(0, _inputObject.IndexOf('}') + 1);
            subsection = subsection.Remove(0, subsection.IndexOf('{') + 1);
            //ID:123123,ID:1231241,ID:123123
            string[] NodeIDs = subsection.Split("ID:".ToCharArray());
            foreach (string id in NodeIDs)
            {
                SubNetIDs.Add(id);
            }
            //remove ID
            //something like do while index != '}' for the List of SubNetNodes

            _inputObject = _inputObject.Remove(0, _inputObject.IndexOf("}") + 1);
            //Debug.Log(x + "," + y + "," + z + "," + w);
        }
        if (ID == "" || PrefabName == "")
        {
            return new NetObject("Error", "Error", "Error", Vector3.zero, Vector3.zero, SubNetIDs);
        }
        return new NetObject(ParentID, PrefabName, ID, pos, rot, SubNetIDs);
    }
    public UpdateType SubjectSorter(string msg)//before calling remove the MSG type ex (POST GET)
    {
        msg = msg.Remove(0, msg.IndexOf(':') + 1);
        UpdateType updateType = UpdateType.Error;
        if (msg.StartsWith("object"))
        {
            updateType = UpdateType.GameObject;
        }
        if (msg.StartsWith("buffer"))
        {
            updateType = UpdateType.buffer;
        }
        if (msg.StartsWith("EndOfPackets"))
        {
            updateType = UpdateType.EndOfPackets;
        }
        if (msg.StartsWith("Destroy"))
        {
            updateType = UpdateType.Destroy;
        }
        return updateType;
    }

    public MSG_Type SortIncomming(string transmission)
    {
        MSG_Type mSG_Type = MSG_Type.ERROR;
        if (transmission.StartsWith("POST"))
        {
            mSG_Type = MSG_Type.POST;
        }
        if (transmission.StartsWith("GET"))
        {
            mSG_Type = MSG_Type.GET;
        }

        //should there be a switch request???
        return mSG_Type;
    }

    public string POST_EndOfPackets()
    {
        return "POST:EndOfPackets";
    }

    public string PostFormat(string data)
    {
        return "POST:" + data;
    }

    public string PostDeletedObjects(string ID)
    {
        return "POST:Destroy{ID:" + ID + "}";
    }

    public string ReadDestroyID(string command)
    {
        string msg = command.Remove(0, command.IndexOf('{') + 1);
        msg = msg.Remove(msg.IndexOf('}'));
        return msg.Remove(0, msg.IndexOf(':') + 1);

    }

}
//NetworkSyntax
/*
 * POST:GOUpdate={<Prefab Name>,<ID>,<Position>,<Rotation>},{<Prefab Name>,<ID>,<Position>,<Rotation>},{<Prefab Name>...
 * GET:GOUpdate
 * POST:BufferSize={xxxxxx}
 * GET:BufferSize
 * 
 * EX:
 * Client>>GET:BufferSize
 * Host<<GET:BufferSize
 * Host>>POST:BufferSize={2024}
 * Client<<POST:BufferSIze{2024}
 * Client>>GET:GOUpdate
 * Host<<GET:GOUpdate
 * Host>>POST:GOUpdate={<Prefab Name>,<ID>,<Position>,<Rotation>},{<Prefab Name>,<ID>,<Position>,<Rotation>},{<Prefab Name>...
 * Client<<POST:GoUpdate={<Prefab Name>,<ID>,<Position>,<Rotation>},{<Prefab Name>,<ID>,<Position>,<Rotation>},{<Prefab Name>...
 * Client>>WAITING:
 * 
 * Host<<WAITING:
 * Host>>GET:BufferSize
 * Client<<GET:BufferSize
 * Client>>POST:BufferSize={2024}
 * Host<<POST:BufferSIze{2024}
 * Host>>GET:GOUpdate
 * Client<<GET:GOUpdate
 * CLient>>POST:GOUpdate={<Prefab Name>,<ID>,<Position>,<Rotation>},{<Prefab Name>,<ID>,<Position>,<Rotation>},{<Prefab Name>...
 * Host<<POST:GoUpdate={<Prefab Name>,<ID>,<Position>,<Rotation>},{<Prefab Name>,<ID>,<Position>,<Rotation>},{<Prefab Name>...
 * Host>>WAITING:
 * 
*/


