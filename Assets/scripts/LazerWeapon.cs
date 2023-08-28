using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerWeapon : MonoBehaviour
{
    
    public GameObject lazerPrefab;
    public GameObject lazerObj;
    public float MaxRange;
    public Vector3 DefaultRotation_Offset;
    WorldTracker GameManagerRefrence;
    public Transform lazer_spawn;
    public float Damage;

    private void Start()
    {
        GameManagerRefrence = transform.GetComponent<GameManagerRefrence>().worldTracker;
        lazer_spawn = transform.GetComponent<NetComponent>().AttachmentNodes[0].transform;

    }
    private void FixedUpdate()
    {
    }
    private void Update()
    {
        if (lazerObj != null)
        {
            lazerObj.GetComponent<Lazer>().Damage = Damage;
            lazerObj.GetComponent<Lazer>().maxDistance = MaxRange;
            lazerObj.transform.rotation = lazer_spawn.rotation;//rotates the lazor
            lazerObj.transform.position = lazer_spawn.position;//sets the lazor position
        }
    }
    public void LazerStart()
    {
        //object must be spawned into the game area the game area is anything below the game manager in hiarchy
        lazerObj = Instantiate(lazerPrefab, lazer_spawn.position, lazer_spawn.rotation, GameManagerRefrence.transform);
        Lazer lazer = lazerObj.GetComponent<Lazer>();


    }

    public void LazerStop()
    {
        lazerObj.GetComponent<GameManagerRefrence>().DestroyObject();
        lazerObj = null;
    }
}
