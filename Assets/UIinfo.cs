using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIinfo : MonoBehaviour
{
    public WorldTracker GameManager;
    public GameObject _camera;
    public GameObject _PlayerShip;

    private void Update()
    {
        if (_camera == null && GameManager.camrea != null)
        {
            _camera = GameManager.camrea;
        }
        if (_PlayerShip == null && GameManager.player != null)
        {
            _PlayerShip = GameManager.player;
        }else if (GameManager.player == null)
        {
            _PlayerShip = null;
        }
    }
}
