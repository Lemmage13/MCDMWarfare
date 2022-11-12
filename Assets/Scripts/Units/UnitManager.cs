using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager instance;

    public Unit unit_;
    public List<Unit> AllUnits;

    public Unit Selected;
    public event Action<Unit> UnitSelected;

    public Unit Active;
    private void Awake()
    {
        instance = this;
    }
    public void unitSelect(Unit unit)
    {
        Selected = unit;
        UnitSelected(unit);
    }
    public Unit GenerateUnit(int player)
    {
        var unit = Instantiate(unit_, new Vector3(-100, -100, -100), Quaternion.identity);
        unit.player = player;
        if (player > 0) { unit.side = true; }
        else { unit.side = false; }
        AllUnits.Add(unit);
        unit.transform.parent = this.transform;
        return unit;
    }
    public List<Unit> GetPlayerUnits(int player)
    {
        List<Unit> pUnits = new List<Unit>();
        for (int i = 0; i < AllUnits.Count; i++)
        {
            var unit = AllUnits[i];
            if (unit.player == player)
            {
                pUnits.Add(unit);
            }
        }
        return pUnits;
    }
    public bool CompareTeam(Unit unit, Unit target)
    {
        //return true if teams are the same
        if (unit.side == target.side) { return true; }
        else { return false; }
    }
}

