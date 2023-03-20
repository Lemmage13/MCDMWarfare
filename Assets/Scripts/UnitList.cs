using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitList
{
    static List<int[]> playerUnits;
    static List<int[]> DMUnits;

    static public void SetPlayerUnits(List<int[]> unitList)
    {
        playerUnits = new List<int[]>(unitList);
    }
    static public void SetDMUnits(List<int[]> unitList)
    {
        DMUnits = new List<int[]>(unitList);
    }
    static public List<int[]> GetPlayerUnits()
    {
        return playerUnits;
    }
    static public List<int[]> GetDMUnits()
    {
        return DMUnits;
    }
}
