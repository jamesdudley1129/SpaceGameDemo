using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class IconProperties : MonoBehaviour, IPointerClickHandler
{
    public ObjectStatus refrence = null;
    public GameObject game_manager = null;
    public Settings settings;
    public GameObject SelectionMenuPrefab;
    public GameObject SelectionMenuInstance;
    public bool Debugvalue = false;
    public void SetValues(ObjectStatus obj ,GameObject gameMgr)
    {
        refrence = obj;
        game_manager = gameMgr;
        settings = game_manager.GetComponent<Settings>();
    }
    private void Update()
    {


    }
    
    public void OnPointerClick(PointerEventData Click)
    {
        if (Click.button == PointerEventData.InputButton.Left)
        {
            Activate();
        }
        else if (Click.button == PointerEventData.InputButton.Right)
        {
            if (SelectionMenuInstance == null)
            {
                SelectionMenuInstance = Instantiate(SelectionMenuPrefab, transform);
            }
        }
    }

    public void Activate()
    {
        game_manager.GetComponent<WorldTracker>().player.GetComponent<ShipControls>().activeTarget = refrence;
    }
}
