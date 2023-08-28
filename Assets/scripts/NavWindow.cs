using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavWindow : MonoBehaviour
{
    public GameObject cmdConsole;

    public void Cmd()//will pass ships or a ship to cmd
    {
        if (cmdConsole.activeInHierarchy)
        {
            cmdConsole.SetActive(false);
        }
        else
        {
            cmdConsole.SetActive(true);
        }
    }

}
