using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodBeh : MonoBehaviour
{
    public List<GameObject> _Assets = new List<GameObject>();
    Faction GODfaction;
    public bool isHost = false;
    WorldTracker world;
    public ObjectStatus target;
    public float MaxNumberOfNuturals;
    public float MaxNumberOfHostiles;
    [SerializeField]
    public float[] _MinMax_SpawnRadius = new float[2];
    public float[] _MinMax_Velocity = new float[2];
    public float[] _MinMax_size = new float[2];
    public float[] _MinMax_mass = new float[2];

    private void Update()
    {
        if(target == null && world.player != null)
        {
            target = world.player.GetComponent<ObjectStatus>();
        }
        if (world == null)
        {
            world = GetComponent<GameManagerRefrence>().worldTracker;
        }else
        { 
            world._God = gameObject;
            if (world.GetComponent<LocalFactionController>().factions.Count >= 1)
            {
                if (GODfaction == null)
                {
                    foreach (Faction faction in world.GetComponent<LocalFactionController>().factions)
                    {
                        if (faction.leader.gameObject.name == gameObject.name)
                        {
                            GODfaction = faction;
                        }
                    }
                    isHost = world.GetComponent<Network>().IsHost;
                }
            }
            if (isHost == true & GODfaction != null)
            {
                float numOfHostileSubordinates = 0;
                float numOfNutrualSubordinates = 0;
                float numOfFreindlySubordinates = 0;
                if (GODfaction.subordinates.Count > 0)
                {
                    GODfaction.CheckForNulls();
                    foreach (ObjectStatus x in GODfaction.subordinates)
                    {
                        switch (x.RelationStatus)
                        {
                            case Relation.Nutural:
                                numOfNutrualSubordinates++;
                                break;
                            case Relation.Freindly:
                                numOfFreindlySubordinates++;
                                break;
                            case Relation.Hostile:
                                numOfHostileSubordinates++;
                                break;
                            default:
                                break;
                        }
                    }
                }
                if (numOfNutrualSubordinates < MaxNumberOfNuturals)
                {

                    GameObject go = CreateObject(_Assets[0].name);//assets[0] is astroid asset
                    CustomizeAstroid(go);
                    go.GetComponent<ObjectStatus>().RelationStatus = Relation.Nutural;
                }
                if (numOfHostileSubordinates < MaxNumberOfHostiles)
                {
                    if (target != null)
                    {
                        GameObject go = CreateObject(_Assets[0].name);//assets[0] is astroid asset
                        CustomizeAstroid(go);
                        SlingObject(go, target.gameObject);
                        go.GetComponent<ObjectStatus>().RelationStatus = Relation.Hostile;
                    }
                }

            }
        }
        
    }
    public GameObject CreateObject(string _AssetName)
    {
        foreach (GameObject asset in _Assets)
        {
            if(asset.name == _AssetName)
            {
                GameObject go = Instantiate(asset,transform.parent);
                //go.GetComponent<ObjectStatus>().faction = GetComponent<ObjectStatus>().faction;//need to apply when added to subordinates
                GODfaction.AddSubordinate(go.GetComponent<ObjectStatus>());
                return go;
            }
        }
        return null;
    }
    public void CustomizeAstroid(GameObject obj)
    {
        float size = Random.Range(_MinMax_size[0], _MinMax_size[1]);
        obj.transform.localScale = Vector3.one * size;
        float mass = Random.Range(_MinMax_mass[0], _MinMax_mass[1]);
        obj.GetComponent<Rigidbody>().mass = mass;
        float SpawnRange = Random.Range(_MinMax_SpawnRadius[0], _MinMax_SpawnRadius[1]);
        Vector3 relocation = new Vector3(Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10)).normalized * SpawnRange;
        obj.transform.position = relocation;
    }
    public void SlingObject(GameObject go ,GameObject target)
    {
        float force = Random.Range(_MinMax_Velocity[0], _MinMax_Velocity[1]);
        Vector3 Dir = (target.transform.position - go.transform.position).normalized * force;
        go.GetComponent<Rigidbody>().AddForce(Dir, ForceMode.VelocityChange);
    }
}
