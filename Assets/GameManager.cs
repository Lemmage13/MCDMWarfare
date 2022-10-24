using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState State;
    public int PCnum = 1;
    public int NPCnum = -1;
    public int turn;

    public static event Action<GameState> GameStateChanged;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        UpdateGameState(GameState.SpawnPlayers);
    }
    public void UpdateGameState(GameState newState)
    {
        State = newState;
        switch (newState)
        {
            case GameState.SpawnPlayers:
                StartCoroutine(PlayerTurnSpawn(1));
                break;
            case GameState.SpawnDM:
                StartCoroutine(DMTurnSpawn(-1));
                break;
            case GameState.PlayerTurn:
                StartCoroutine(PlayerTurn());
                break;
            case GameState.DMTurn:
                UpdateGameState(GameState.PlayerTurn);
                break;
            default:
                break;
        }
        GameStateChanged(newState);
    }
    public IEnumerator PlayerTurnSpawn(int player)
    {
        Unit unit = UnitManager.instance.GenerateUnit(player);
        BattlefieldManager.instance.SpawnPlate(true, player);
        unit.Active = true;
        while (unit.Active) { yield return null; }
        BattlefieldManager.instance.clearMovable();
        UpdateGameState(GameState.SpawnDM);
    }
    public IEnumerator DMTurnSpawn(int player)
    {
        Unit unit = UnitManager.instance.GenerateUnit(-1);
        BattlefieldManager.instance.SpawnPlate(true, player);
        unit.Active = true;
        unit.Move(BattlefieldManager.instance.Spaces[27]);
        BattlefieldManager.instance.clearMovable();
        UpdateGameState(GameState.PlayerTurn);
        yield return null;
    }
    public IEnumerator PlayerTurn()
    {
        for (int i = 0; i < PCnum + 1; ++i)
        {
            List<Unit>PlayerUnits = UnitManager.instance.GetPlayerUnits(i);
            foreach (Unit unit in PlayerUnits)
            {
                unit.Active = true;
                BattlefieldManager.instance.MovePlate(unit.occupying);
                while (unit.Active) { yield return null; }
                BattlefieldManager.instance.clearMovable();
            }
        }
        UpdateGameState(GameState.DMTurn);
    }
    
}
public enum GameState
{
    SpawnPlayers,
    SpawnDM,
    PlayerTurn,
    DMTurn
}