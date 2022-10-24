using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager instance;

    public Unit unit_;
    public List<Unit> Units;
    private void Awake()
    {
        instance = this; 
        GameManager.GameStateChanged += GameManager_GameStateChanged;
    }
    private void OnDestroy()
    {
        GameManager.GameStateChanged -= GameManager_GameStateChanged;
    }
    private void GameManager_GameStateChanged(GameState state)
    {
        
    }
    public Unit GenerateUnit(int player)
    {
        var unit = Instantiate(unit_, new Vector3(-100, -100, -100), Quaternion.identity);
        unit.player = player;
        Units.Add(unit);
        return unit;
    }
    public List<Unit> GetPlayerUnits(int player)
    {
        List<Unit> pUnits = new List<Unit>();
        for (int i = 0; i < Units.Count; i++)
        {
            var unit = Units[i];
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

