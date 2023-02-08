using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    System.Random dice = new System.Random();

    public GameState State;
    public int PCnum = 1;
    public int DMCnum = 1;
    public int unitnum = 3; //temporary for spawning set units for testing

    public int[] turnorder = { 1, -1 }; //temporary for testing - will eventually be changeable
    public int activePlayer;
    public int Round;
    public bool roundEnd;
    public Team Victor = Team.none;

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
            case GameState.RoundTurns:
                StartCoroutine(HandleTurns());
                break;
            case GameState.RoundEnd:
                RoundEnd();
                break;
            case GameState.GameEnd:
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
            for (int i = 0; i < unitnum; i++)
            {
                BaseUnit unit = UnitManager.instance.GenerateUnit(player, UnitType.Infantry, Ancestry.Human);
                unit.name = "player " + player.ToString() + " unit " + i.ToString();
                unit.ActivateSpawning();
                while (unit.Occupying == null) { yield return null; }
                Debug.Log("unit " + (i + 1).ToString() + " of " + unitnum.ToString() + " spawned");
            }
        }
        Round = 0;
        UpdateGameState(GameState.RoundTurns);
    }
    public IEnumerator HandleTurns()
    {
        roundEnd = false;
        activePlayer = 0;
        StartCoroutine(PlayerRound(turnorder[activePlayer]));
        while (!roundEnd)
        {
            yield return null;
        }
    }
    public IEnumerator PlayerRound(int player)
    {
        Debug.Log($"player turn " + player.ToString());
        List<BaseUnit> playerUnits = UnitManager.instance.GetPlayerUnits(player);
        foreach (BaseUnit unit in playerUnits) { unit.Ready = true; }
        if (Victor == Team.none)
        {
            while (turnorder[activePlayer] == player)
            {
                yield return null;
            }
        }
    }
    public void EndTurn(int player)
    {
        UnitManager.instance.Active = null;
        UnitManager.instance.UnitSelect(null);
        List<BaseUnit> playerUnits = UnitManager.instance.GetPlayerUnits(player);
        for (int i = 0; i < playerUnits.Count; i++)
        {
            playerUnits[i].Ready = false;
        }
        BattlefieldManager.instance.clearPlates();
        activePlayer += 1;
        if (activePlayer == turnorder.Length)
        {
            UpdateGameState(GameState.RoundEnd);
        }
        else { StartCoroutine(PlayerRound(turnorder[activePlayer])); }
    }
    public void RoundEnd()
    {
        int PCUTier = 0;
        int DMUTier = 0;
        for (int i = 0; i < UnitManager.instance.AllUnits.Count; i++)
        {
            BaseUnit unit = UnitManager.instance.AllUnits[i];
            if (unit.Alive)
            {
                if (unit.Player > 0) { PCUTier += unit.GetTier(); }
                else if (UnitManager.instance.AllUnits[i].Player < 0) { DMUTier += unit.GetTier(); }
            }
        }
        Debug.Log("compare: " + PCUTier.ToString() + " " + DMUTier.ToString());
        if (PCUTier > 2 * DMUTier)
        {
            BattleEnd(Team.Players);
        }
        else if (DMUTier > 2 * PCUTier)
        {
            BattleEnd(Team.DM);
        }
        else
        {
            Debug.Log("prepare to start next round");
            activePlayer = 0;
            Round++;
            UpdateGameState(GameState.RoundTurns);
        }
    }
    public void BattleEnd(Team victor)
    {
        Victor = victor;
        UpdateGameState(GameState.GameEnd);
    }
    public int dieRoll(int dx, bool? adv)
    {
        int roll;
        switch (adv)
        {
            case true:
                Debug.Log("Advantage!");
                roll = Math.Max(dice.Next(1, dx + 1), dice.Next(1, dx + 1));
                Debug.Log("Rolled: " + roll.ToString());
                return roll;
            case false:
                Debug.Log("Disadvantage!");
                roll = Math.Min(dice.Next(1, dx + 1), dice.Next(1, dx + 1));
                Debug.Log("Rolled: " + roll.ToString());
                return roll;
            case null:
                roll = dice.Next(1, dx + 1);
                Debug.Log("Rolled: " + roll.ToString());
                return roll;
        }
    }
}
public enum Team
{
    Players,
    DM,
    none
}
public enum GameState
{
    SpawnTurn,
    TurnStart,
    RoundTurns,
    RoundEnd,
    GameEnd
}
public enum UnitType
{
    Aerial,
    Artillery,
    Cavalry,
    Infantry
}
public enum Ancestry
{
    Dwarf,
    Elf,
    Human
}