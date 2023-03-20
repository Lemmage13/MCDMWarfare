using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AddUnit : MonoBehaviour
{
    //unity cannot serialise nested objects to show in inspector
    public List<int[]> playerUnits = new List<int[]>();
    public List<int[]> dmUnits = new List<int[]>();

    //add unit functions called by corresponding buttons in ui
    public void AddPlayerUnit() //only adds human infantry for now
    {
        playerUnits.Add(new int[] { 0, 0 });
        Debug.Log("player unit added");
    }
    public void AddDMUnit() //ditto above
    {
        dmUnits.Add(new int[] { 0, 0 });
        Debug.Log("DM unit added");
    }

    public void CommitUnits()
    {
        UnitList.SetPlayerUnits(playerUnits);
        UnitList.SetDMUnits(dmUnits);
    }
}
