using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerControlMode
{
    ship, onfoot, spacewalk


}

public enum UpdateType
{
    GameObject, buffer, EndOfPackets, Error, Destroy

}

public enum MSG_Type
{
    POST, GET, IGNORE, ERROR
}

public enum WeaponType
{
    Static,Tracking,Limited
}
public enum AmmoType
{
    Lazer,Kinetic
}

public enum Relation
{
    Nutural,Freindly,Hostile
}

public enum GameMode
{
    TDM,FFA,Conquest,FreeMode,Peaceful,Survival,NULL
}

public enum EngineTags
{
    Untagged,Respawn,Finish,EditorOnly,
    MainCamera,Player,GameController,astroid,
    JumpGate,Drops,SectorControl,Station,AI,
    Ship,nonPlayer,EventObject,Playable 
}