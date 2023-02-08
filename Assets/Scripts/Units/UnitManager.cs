using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager instance;

    public BaseUnit unitPrefab;
    public List<BaseUnit> AllUnits;
    public List<BaseUnit> PlayerUnits;
    public List<BaseUnit> DMUnits;

    public BaseUnit Selected;
    public event Action<BaseUnit> UnitSelected;

    public BaseUnit Active;
    private void Awake()
    {
        instance = this;
    }
    public void UnitSelect(BaseUnit unit)
    {
        Selected = unit;
        UnitSelected(unit);

    }
    public void Unselect()
    {
        Selected = null;
        UnitSelected(null);
    }
    public BaseUnit GenerateUnit(int player, UnitType type, Ancestry ancestry)
    {
        BaseUnit unit = Instantiate(unitPrefab, new Vector3(100, 100, 100), Quaternion.identity);
        unit.Initialise(type, ancestry, player);
        AllUnits.Add(unit);
        return unit;
    }
    public List<BaseUnit> GetPlayerUnits(int player)
    {
        List<BaseUnit> pUnits = new List<BaseUnit>();
        for (int i = 0; i < AllUnits.Count; i++)
        {
            var unit = AllUnits[i];
            if (unit.Player == player)
            {
                pUnits.Add(unit);
            }
        }
        return pUnits;
    }
    public bool CompareTeam(BaseUnit unit, BaseUnit target)
    {
        //return true if teams are the same
        if (unit.Side == target.Side) { return true; }
        else { return false; }
    }
}

