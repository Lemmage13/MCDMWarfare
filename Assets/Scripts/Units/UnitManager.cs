using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    public BaseUnit GenerateUnit(int player, Ancestry ancestry, UnitType type)
    {
        BaseUnit unit = Instantiate(unitPrefab, new Vector3(100, 100, 100), Quaternion.identity);
        unit.Initialise(ancestry, type, player);
        AllUnits.Add(unit);
        return unit;
    }
    public async void Spawning()
    {
        Debug.Log("Spawning started");
        List<int[]> playerUnits = UnitList.GetPlayerUnits();
        List<int[]> DMUnits = UnitList.GetDMUnits();
        int i = 0;
        Camera camera = Camera.main;
        foreach (int[] pUnit in playerUnits)
        {
            i++;
            BaseUnit unit = UnitManager.instance.GenerateUnit(1, (Ancestry)pUnit[0], (UnitType)pUnit[1]);
            unit.name = "player " + 1.ToString() + " unit " + i.ToString();
            camera.GetComponentInChildren<UItxtIndicator>().UpdateText("Player spawning: " + unit.Ancestry.ToString() + " " + unit.Type.ToString());
            unit.ActivateSpawning();
            while (unit.Occupying == null) { await Task.Yield(); } //neater way to use aync here
            Debug.Log("unit " + (i).ToString() + " of " + pUnit.Length.ToString() + " spawned");
        }
        i = 0;
        foreach (int[] dUnit in DMUnits)
        {
            i++;
            BaseUnit unit = UnitManager.instance.GenerateUnit(-1, (Ancestry)dUnit[0], (UnitType)dUnit[1]);
            unit.name = "player " + (-1).ToString() + " unit " + i.ToString();
            camera.GetComponentInChildren<UItxtIndicator>().UpdateText("DM spawning: " + unit.Ancestry.ToString() + " " + unit.Type.ToString());
            unit.ActivateSpawning();
            while (unit.Occupying == null) { await Task.Yield(); } //neater way to use aync here
            Debug.Log("unit " + (i).ToString() + " of " + dUnit.Length.ToString() + " spawned");
        }
        Debug.Log("Spawning finished");
        GameManager.Instance.UpdateBattleState(BattleState.RoundTurns);
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

