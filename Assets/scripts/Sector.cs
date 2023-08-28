using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sector
{
    public string sectorName;
    //this script contains locational data for ships,gates,astroids,stations,and drops
    //this data is updated by there ai
    public List<GameObject> gates = new List<GameObject>();
    public List<GameObject> ships = new List<GameObject>();
    public List<GameObject> astroids = new List<GameObject>();
    public List<GameObject> stations = new List<GameObject>();
    public List<GameObject> drops = new List<GameObject>();

    public Sector(string sectorName,List<GameObject> gates, List<GameObject> ships, List<GameObject> astroids, List<GameObject> stations, List<GameObject> drops)
    {
        this.sectorName = sectorName;
        this.gates = gates;
        this.ships = ships;
        this.astroids = astroids;
        this.stations = stations;
        this.drops = drops;

    }
    public void PassiveUpdate(List<GameObject> gameObjects)
    {

    }


    /*IDK ABOUT THIS\/
    public void UpdateShips(GameObject ship)
    {
        //compare ship provided by its ai
    }
    public void UpdateGates()
    {
        //gates cant move or rotate but can be added by AI or player or story
        
    }

    public void RemoveShip()
    {
        //removes ship
    }
    public void AddShip(GameObject gate,GameObject ship)
    {
        //compares gate 
        //adds ship at gate
    }
    public void CargoDrop(GameObject ship)
    {

    }
    public void Battle(List<GameObject> ships)
    {
        //takes ships targets and simulates the battle based on ship specs
        //if freight is surrenderd(rnd gen) maybe end battle (rnd gen)
        //{
        // SurrenderFreight();
        //}
        //else 
        //{
        //RemovesShip when destroyed
        //drops freight when destroyed
        //}
    }
    public void SurrenderFreight()
    {

    }
    public void CargoCollect(GameObject ship, GameObject drop)
    {

    }

    public void StationCycle(GameObject station)
    {
        //compares for station
        //if station.supplys >= needed supplys
        //{
        //update station products
        //}
    }

    public void StationBuy(GameObject station,GameObject ship)
    {

    }
    */
}
