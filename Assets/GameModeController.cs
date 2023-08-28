using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeController : MonoBehaviour//SetsGameMode for all depended scripts
{

    public GameMode current = GameMode.NULL;
    public GameMode Set_to = GameMode.NULL;
    LocalFactionController localFactionControler;
    bool GameActive = false;
    private void Start()
    {
        localFactionControler = GetComponent<LocalFactionController>();
    }

    private void Update()
    {
        if (GetComponent<Network>().IsHost)
        {
            if (Set_to != current)
            {
                SetGameMode(Set_to);  
            }
            switch (current)
            {
                case GameMode.TDM:
                    break;
                case GameMode.FFA:
                    break;
                case GameMode.Conquest:
                    break;
                case GameMode.FreeMode:
                    break;
                case GameMode.Peaceful:
                    break;
                case GameMode.Survival:
                    RunSurvivalGame();
                    break;
                case GameMode.NULL:

                    break;
                default:
                    break;
            }
        }
        else
        {
            
        }
    }

    public void SetGameMode(GameMode set)//SetsGameMode for all depended scripts
    {
        current = Set_to;
        GameActive = false;
        localFactionControler.gameMode = current;
        localFactionControler.clear = true;
        //this will override Update once console is implemented
        //resets GameMode Staging a new game when called no matter if its the same as existing gamemode
    }

    public void RunSurvivalGame()
    {
        WorldTracker wt = GetComponent<WorldTracker>();
        if (wt.player != null)
        foreach (GameObject item in wt.world)
        {
            if(item.tag == EngineTags.Station.ToString())
            {
                    if (wt.player.GetComponent<ObjectStatus>().faction != "")
                    {
                        foreach (Faction fac in localFactionControler.factions)
                        {
                            if (fac.name == wt.player.GetComponent<ObjectStatus>().faction)
                            {
                                fac.AddSubordinate(item.GetComponent<ObjectStatus>());
                                wt._God.GetComponent<GodBeh>().target = item.GetComponent<ObjectStatus>();
                                GameActive = true;
                                break;
                            }
                        }
                         
                    }
            }
        }
    }


    
    

}
