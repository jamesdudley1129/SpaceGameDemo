using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerRefrence : MonoBehaviour
{

    public WorldTracker worldTracker;

    private bool destroy = false;
    // Start is called before the first frame update
    public void SetWorldTracker(WorldTracker worldTracker)
    {
        this.worldTracker = worldTracker;
    }
    private void Update()
    {
        if (destroy)
        {
            DestroyObject();
        }
    }
    public void DestroyObject()
    {
        worldTracker.AddToDestroyQue(gameObject);
    }
}
