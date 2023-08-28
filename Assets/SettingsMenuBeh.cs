using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingsMenuBeh : MonoBehaviour
{
    public Settings gameManagerSettings;
    public Toggle _fullscreen;


    public void ExitWithoutSaving()
    {
        _fullscreen.isOn = gameManagerSettings.fullscreen;
        gameObject.SetActive(false);
    }
    public void ApplyChanges()
    {
        UpdateGraphics();
    }
    public void UpdateGraphics()
    {
        gameManagerSettings.fullscreen = _fullscreen.isOn;
        gameManagerSettings.UpdateScreenSettings();
        gameObject.SetActive(false);
    }
    
}