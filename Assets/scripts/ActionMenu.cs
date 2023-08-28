using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMenu : MonoBehaviour
{
    public GameObject NavWindow;
    public GameObject NavButtion;
    public GameObject DebugConsole;
    public GameObject SettingsMenuUI;
    public void ToggleNavWindow()
    {
        if (NavWindow.activeInHierarchy == false)
        {
            NavWindow.SetActive(true);
        }
        else
        {
            NavWindow.SetActive(false);
        }
    }

    public void ToggleDebugMenu()
    {

        if (DebugConsole.activeInHierarchy)
        {
            DebugConsole.SetActive(false);
        }
        else
        {
            DebugConsole.SetActive(true);
        }
    }
    public void LoadControlsMenu()
    {
        if (DebugConsole.activeInHierarchy)
        {
            DebugConsole.SetActive(false);
        }
        else
        {
            DebugConsole.SetActive(true);
            DebugConsole.GetComponent<DebugConsole>().text_Input.text = "/controls";
            DebugConsole.GetComponent<DebugConsole>().ProccessInput();
        }
    }
    public void OpenSettingsUI()
    {
        SettingsMenuUI.SetActive(true);
    }
}
