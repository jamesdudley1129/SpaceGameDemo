using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalSpawner : MonoBehaviour
{
    WorldTracker worldtracker;

    private void Start()
    {
        worldtracker = GetComponent<WorldTracker>();
    }

    public void SpawnPlayer()
    {
        if (worldtracker.player == null)//temparary need to spawn from player or station or a start game script
        {


            worldtracker.player = Instantiate(worldtracker.PlayerAsset, worldtracker.SpawnObject.transform.position, worldtracker.SpawnObject.transform.rotation, transform);
            worldtracker.player.GetComponent<ShipControls>().player_control = true;
            int currentAttachmentNode = 0;

            for (int i = 0; i < worldtracker.player.GetComponent<ShipSpecs>().WeaponPlatforms.Count; i++)
            {
                Instantiate(worldtracker.lazorCannonMk1, worldtracker.player.GetComponent<NetComponent>().AttachmentNodes[currentAttachmentNode].transform);
                currentAttachmentNode++;
            }
            Instantiate(worldtracker.radarComponent, worldtracker.player.GetComponent<NetComponent>().AttachmentNodes[currentAttachmentNode].transform);
            currentAttachmentNode++;
        }
    }
}
