using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalFactionController : MonoBehaviour
{
    WorldTracker worldtracker;
    public List<ObjectStatus> ALL_controlers = new List<ObjectStatus>();//includes refrence to all Objects with Object Status
    public List<ObjectStatus> NPC_controlers = new List<ObjectStatus>(); //includes NPC's controllers
    public List<ObjectStatus> playerControlers = new List<ObjectStatus>();//includes network Player Controlers exluding local player
    public List<ObjectStatus> NaturalStructures = new List<ObjectStatus>();//includes plannets astroids dialect stations ect...anything that has a constant relation 
    public List<string> FactionNames = new List<string>();
    public List<Faction> factions = new List<Faction>();
    public ObjectStatus _God;
    public ObjectStatus player;
    public bool clear = false;
    public GameMode gameMode = GameMode.NULL;
    private void Start()
    {
        
        worldtracker = transform.GetComponent<WorldTracker>();
    }


    private void Update()
    {
        if (worldtracker.player != null)
        {
            if(_God == null &&worldtracker._God != null)
            {
                _God = worldtracker._God.GetComponent<ObjectStatus>();
            }
            if (player == null)
            {
                player = worldtracker.player.GetComponent<ObjectStatus>();

            }
        }
        if(_God != null && player != null)
        {
            if (clear == true)
            {
                if (clear == true)
                {
                    clear = !clear;
                }
                UpdateRefrences();
                SetFactions();
                FactionNames.Clear();
                foreach (Faction current in factions)
                {
                    FactionNames.Add(current.name);
                }
            }
            
        }

    }

    public void UpdateRefrences()
    {
        player = worldtracker.player.GetComponent<ObjectStatus>();
        ALL_controlers.Clear();
        playerControlers.Clear();
        NPC_controlers.Clear();
        NaturalStructures.Clear();

        if (worldtracker.OtherPlayer != null)
        {
            ALL_controlers.Add(worldtracker.OtherPlayer.GetComponent<ObjectStatus>());//adds other player to all_controllers

            playerControlers.Add(worldtracker.OtherPlayer.GetComponent<ObjectStatus>());//adds other player to player_controllers
        }
        foreach (GameObject obj in worldtracker.world)//searches for other objects in worldTracker(this excludes players)
        {
            if (obj.GetComponent<ObjectStatus>() != null && !ALL_controlers.Contains(obj.GetComponent<ObjectStatus>()))
            {
                ALL_controlers.Add(obj.GetComponent<ObjectStatus>());//adds this world object to all_controllers
                if (worldtracker.player != obj)
                {
                    if (obj.tag == EngineTags.Player.ToString())
                    {
                        playerControlers.Add(obj.GetComponent<ObjectStatus>());//adds player to player_controllers
                    }
                    else if (obj.tag == EngineTags.AI.ToString())
                    {
                        NPC_controlers.Add(obj.GetComponent<ObjectStatus>());//adds AI to NPC_controllers
                    }
                    else
                    {
                        NaturalStructures.Add(obj.GetComponent<ObjectStatus>());//adds everythign else to natrualStructures                       
                    }
                }
                
            }
        }
    }
    public void SetFactions()
    {
        if(factions.Count > 0)
        {
            factions.Clear();
            foreach(ObjectStatus go in ALL_controlers)
            {
                go.faction = "";
                go.RelationStatus = Relation.Nutural;
            }
        }
        switch (gameMode)//define factions here
        {
            case GameMode.TDM://two teams leaders are dispersed randomly
                SET_FactionTDM_Config();                
                break;
            case GameMode.FFA://all leaders are set to there own factions and are enamy to player
                Debug.Log("FFA Started");
                SET_FactionFFA_Config();
                break;
            case GameMode.Conquest://similar to tdm fighting over objectives like CTF
                break;
            case GameMode.FreeMode://all factions are nutural until acted hostilay upon (might change if there are hostile factions added to the game)
                break;
            case GameMode.Peaceful://all players are one faction    
                break;
            case GameMode.Survival://1faction against NPC factions the NPC factions will have predefined relations
                SET_Survival_Config();
                break;
            case GameMode.NULL:
                break;
            default:
                break;
        }
    }
    public void SET_FactionTDM_Config()
    {
        Faction Alpha = new Faction(player, new List<ObjectStatus>(), "Team Alpha");
        Faction Bravo = null;
        bool Alt = false;
        if (playerControlers.Count > 0)
        {
            Bravo = new Faction(playerControlers[0], new List<ObjectStatus>(), "Team Bravo");

        }
        if (Bravo != null)
        {
            for (int i = 1; i < playerControlers.Count; i++)
            {
                switch (Alt)
                {
                    case true:
                        Alt = !Alt;
                        Alpha.AddSubordinate(playerControlers[i]);
                        break;
                    case false:
                        Alt = !Alt;
                        Bravo.AddSubordinate(playerControlers[i]);
                        break;
                    default:
                        break;
                }
            }
        }
        Alpha.SetRelation(Relation.Freindly);
        Bravo.SetRelation(Relation.Hostile);
        factions.Add(Alpha);
        factions.Add(Bravo);
        //need to send to clients from Host Game
    }

    public void SET_FactionFFA_Config()
    {
        Debug.Log("FFA RAN");
        Faction yourteam = new Faction(player, new List<ObjectStatus>(), "YourTeam");
        yourteam.SetRelation(Relation.Freindly);
        factions.Add(yourteam);
        //for other players
        int teamCounter = 1;
        for (int index = 0; index < playerControlers.Count; index++)//foreach player controller make team 
        {
            Debug.LogWarning(playerControlers[index].name);
            Faction team = new Faction(playerControlers[index], new List<ObjectStatus>(), "Team " + teamCounter);
            team.SetRelation(Relation.Hostile);
            factions.Add(team);
            teamCounter++;
        }
    }
    public void SET_Survival_Config()
    {
        Faction Natrual = new Faction(_God, new List<ObjectStatus>(), "Natrual");
        Debug.Log("Adding Natrual Faction");
        factions.Add(Natrual);
        Faction Alpha = new Faction(player, new List<ObjectStatus>(), "Team Alpha");
        foreach(ObjectStatus user in playerControlers)
        {
            if (!Alpha.subordinates.Contains(user))
            {
                Alpha.subordinates.Add(user);
            }
        }
        factions.Add(Alpha);
    }
}
