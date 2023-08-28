using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faction
{
    public string name;
    Relation relation = Relation.Nutural;
    public ObjectStatus leader;
    public List<ObjectStatus> subordinates;
    public Faction(ObjectStatus leader, List<ObjectStatus> subordinates, string name)
    {
        this.leader = leader;
        this.subordinates = subordinates;
        this.name = name;
        leader.faction = name;
    }
    public void RenameFaction(string FactionName)
    {
        name = FactionName;
        if(leader != null)
        {
            leader.faction = FactionName;
        }if(subordinates.Count > 0)
        {
            foreach (ObjectStatus obj in subordinates)
            {
                obj.faction = FactionName;
            }
        }
    }
    public void SetRelation(Relation relation)
    {
        this.relation = relation;
        leader.RelationStatus = relation;
        if (subordinates.Count > 0)
        {
            foreach (ObjectStatus member in subordinates)
            {
                member.RelationStatus = relation;
            }
        }
    }
    public void AddSubordinate(ObjectStatus Obj)
    {
        subordinates.Add(Obj);
        Obj.faction = name;
    }
    public void RemoveSubordinate(ObjectStatus obj)
    {
        subordinates.Remove(obj);
    }
    public void SetLeader(ObjectStatus leader)
    {
        this.leader = leader;
        leader.faction = name;
    }
    public void CheckForNulls()
    {
        
        for (int current = 0; current < subordinates.Count; current++)
        {
            if (subordinates[current] == null)
            {
                subordinates.Remove(subordinates[current]);
                current--;
            }
        }
        if(leader == null && subordinates.Count >0)
        {
            SetLeader(subordinates[0]);
            subordinates.Remove(subordinates[0]);
        }
    }
}
