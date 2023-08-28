using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NetObject
{
    public string _ParentID;
    public string _ID;
    public string _PrefabName;
    public Vector3 _Position;
    public Quaternion _Rotation;
    public List<string> SubNodeIDs = new List<string>();
    //<Prefab Name>,<ID>,<Position>,<Rotation>
    public NetObject(string _ParentID, string _PrefabName, string _ID, Vector3 _Position, Vector3 _Rotation, List<string> SubNodeIDs)
    {
        this._ParentID = _ParentID;
        this._ID = _ID;
        this._PrefabName = _PrefabName;
        this._Position = _Position;
        this._Rotation = new Quaternion();
        this._Rotation.eulerAngles = _Rotation;
        this.SubNodeIDs = SubNodeIDs;
    }
}
