using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Windows;
public class Settings : MonoBehaviour
{

    public bool fullscreen;
    public string left_shift = "left shift";
    public string left_ctrl = "left ctrl";
    public string backspace = "backspace";
    public string up = "up";
    public string down = "down";
    public string left = "left";
    public string right = "right";
    public string MouseX = "mouse x";
    public string MouseY = "mouse y";
    public string tab = "tab";
    public string escape = "escape";
    public string space = "space";
    public string equal = "=";
    public string minus = "-";
    public string leftclick = "mouse0";
    public string rightclick = "mouse1";
    public string Return = "return";
    public string _F1 = "f1";
    public string _F2 = "f2";
    public string _F3 = "f3";
    public List<string> alphabit = new List<string>();//need to be assigned
    public List<string> numbers = new List<string>();//need to be assigned
    // Start is called before the first frame update
    void Start()
    {
        UpdateScreenSettings();

    }
    private void Update()
    {

    }
    public void UpdateScreenSettings()
    {
        if (fullscreen)
        {

            Screen.SetResolution(1080, 720, true);
        }
        else
        {
            //Screen.fullScreen = !Screen.fullScreen;
            Screen.SetResolution(1080, 720, false);
        }

    }
}
