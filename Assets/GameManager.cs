using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    System.Random dice = new System.Random();

    public GameState State;
    public int PCnum = 1;
    public int DMCnum = 1;
    public int unitnum = 2;

    public int[] turnorder = { 1, -1 }; //temporary for testing - will eventually be changeable
    public int activePlayer;
    public int turn;
    public bool winner;

    public static event Action<GameState> GameStateChanged;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        UpdateGameState(GameState.SpawnTurn);
    }
    public void UpdateGameState(GameState newState)
    {
        State = newState;
        switch (newState)
        {
            case GameState.SpawnTurn:
                StartCoroutine(HandleSpawn());
                break;
            case GameState.GameTurns:
                StartCoroutine(HandleTurns());
                break;
            default:
                break;
        }
        GameStateChanged(newState);
    }
    public IEnumerator HandleSpawn()
    {
        foreach (int player in turnorder)
        {
            turn = player;
            for (int i = 0; i < unitnum; i++)
            {
                Unit unit = UnitManager.instance.GenerateUnit(player);
                unit.name = "player " + player.ToString() + " unit " + i.ToString();
                StartCoroutine(unit.Spawn());
                UnitManager.instance.Active = unit;
                while (unit.occupying == null) { yield return null; }
                UnitManager.instance.Active = null;
            }
        }
        UpdateGameState(GameState.GameTurns);
    }
    public IEnumerator HandleTurns()
    {
        winner = false;
        activePlayer = 0;
        StartCoroutine(PlayerTurn(turnorder[activePlayer]));
        while (!winner)
        {
            yield return null;
        }
    }
    public IEnumerator PlayerTurn(int player)
    {
        Debug.Log($"player turn " + player.ToString());
        List<Unit> playerUnits = UnitManager.instance.GetPlayerUnits(player);
        foreach (Unit unit in playerUnits) { unit.Ready = true; }
        while (turnorder[activePlayer] == player)
        {
            yield return null;
        }
    }
    public void EndTurn(int player)
    {
        UnitManager.instance.Active = null;
        List<Unit> playerUnits = UnitManager.instance.GetPlayerUnits(player);
        for (int i= 0; i < playerUnits.Count; i++)
        {
            playerUnits[i].Ready = false;
        }
        BattlefieldManager.instance.clearMovable();
        activePlayer = (activePlayer + 1) % (PCnum + DMCnum);
        StartCoroutine(PlayerTurn(turnorder[activePlayer]));
    }
    public int dieRoll(int dx, Advantage adv)
    {
        switch (adv)
        {
            case Advantage.Standard:
                return dice.Next(1, dx + 1);
            case Advantage.Advantage:
                return Math.Max(dice.Next(1, dx + 1), dice.Next(1, dx + 1));
            case Advantage.Disadvantage:
                return Math.Min(dice.Next(1, dx + 1), dice.Next(1, dx + 1));
            default:
                Debug.Log("dice error - null advantage");
                return dice.Next(1, dx + 1);
        }
    }
}
public enum Advantage
{
    Standard,
    Advantage,
    Disadvantage
}
public enum GameState
{
    SpawnTurn,
    GameTurns
}