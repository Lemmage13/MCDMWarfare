using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitList
{
    [SerializeField] static List<int[]> playerUnits;
    [SerializeField] static List<int[]> DMUnits;

    static public void SetPlayerUnits(List<int[]> unitList)
    {
        playerUnits = new List<int[]>(unitList);
    }
    static public void SetDMUnits(List<int[]> unitList)
    {
        DMUnits = new List<int[]>(unitList);
    }
}
