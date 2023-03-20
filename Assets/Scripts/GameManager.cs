using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    System.Random dice = new System.Random();

    public GameState State;
    public BattleState BattleState;
    public int PCnum = 1;
    public int DMCnum = 1;

    public int[] turnorder = { 1, -1 }; //temporary for testing - will eventually be changeable
    public int activePlayer;
    public int Round;
    public bool roundEnd;
    public Team Victor = Team.none;
    private void Start()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
    }
    public void StartBattle()
    {
        UpdateGameState(GameState.Battlefield);
    }
    public void UpdateGameState(GameState newState)
    {
        State = newState;
        switch (newState)
        {
            case GameState.ArmyBuilder:
                HandleArmyBuilder();
                break;
            case GameState.Battlefield:
                HandleBattlefield();
                break;
            default:
                break;
        }
    }
    void HandleArmyBuilder()
    {
        SceneManager.LoadScene("ArmyBuilder");
    }
    void HandleBattlefield()
    {
        SceneManager.LoadScene("Battlefield");
        UpdateBattleState(BattleState.SpawnTurn);
    }
    public void UpdateBattleState(BattleState newState)
    {
        BattleState = newState;
        switch (newState)
        {
            case BattleState.SpawnTurn:
                HandleSpawn();
                break;
            case BattleState.RoundTurns:
                StartCoroutine(HandleTurns());
                break;
            case BattleState.RoundEnd:
                RoundEnd();
                break;
            case BattleState.GameEnd:
                break;
            default:
                break;
        }
    }
    public void HandleSpawn()
    {
        Round = 0;
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
            UpdateBattleState(BattleState.RoundEnd);
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
            UpdateBattleState(BattleState.RoundTurns);
        }
    }
    public void BattleEnd(Team victor)
    {
        Victor = victor;
        UpdateBattleState(BattleState.GameEnd);
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
    ArmyBuilder,
    Battlefield
}
public enum BattleState
{
    SpawnTurn,
    TurnStart,
    RoundTurns,
    RoundEnd,
    GameEnd
}
public enum UnitType
{

    Infantry,
    Aerial,
    Artillery,
    Cavalry
}
public enum Ancestry
{
    Human,
    Dwarf,
    Elf
}