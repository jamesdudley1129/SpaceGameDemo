using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainMenuWindow : MonoBehaviour
{
    public GameObject GameManager;
    public GameObject SettingsUI;

    public void StartHost()
    {
        Network network = GameManager.GetComponent<Network>();
        network.IsHost = true;
        network.multiplayer_enabled = true;
        GameManager.GetComponent<WorldTracker>().StartGame();
        GameManager.GetComponent<Mouse>().EngagedMouse = true;
        Cursor.lockState = CursorLockMode.Locked;
        gameObject.SetActive(false);


    }
    public void StartClient()
    {
        Network network = GameManager.GetComponent<Network>();
        network.IsHost = false;
        network.multiplayer_enabled = true;
        GameManager.GetComponent<WorldTracker>().StartGame();
        GameManager.GetComponent<Mouse>().EngagedMouse = true;
        Cursor.lockState = CursorLockMode.Locked;
        gameObject.SetActive(false);

    }
    public void StartSinglePlayer()
    {
        Network network = GameManager.GetComponent<Network>();
        network.IsHost = true;
        network.multiplayer_enabled = false;
        GameManager.GetComponent<WorldTracker>().StartGame();
        GameManager.GetComponent<Mouse>().EngagedMouse = true;
        Cursor.lockState = CursorLockMode.Locked;
        gameObject.SetActive(false);

    }
    public void OpenSettings()
    {
        SettingsUI.SetActive(true);
    }
}
