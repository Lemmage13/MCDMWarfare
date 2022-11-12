using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject UnitTurnButton;
    public GameObject AttackButton;
    public GameObject AttackText;
    public GameObject screenGreyout;
    private void Awake()
    {
        instance = this; 
        AttackButton.SetActive(false);
        AttackText.SetActive(false);
        screenGreyout.SetActive(false);
    }
    private void Start()
    {
        UnitManager.instance.UnitSelected += SelectUpdate;
        GameManager.GameStateChanged += GameStateUpdated;
    }
    public void SelectUpdate(Unit selected)
    {
        if (UnitManager.instance.Active != null)
        {
            Unit Active = UnitManager.instance.Active;
            if (selected.occupying.Attackable.activeSelf) 
            { AttackButton.SetActive(true); AttackText.SetActive(true); }
            else 
            { AttackButton.SetActive(false); AttackText.SetActive(false); }
        }
    }
    public void GameStateUpdated(GameState state)
    {
        switch (state)
        {
            case GameState.GameEnd:
                gameEnd();
                break;
            default:
                break;
        }
    }
    public void gameEnd()
    {
        screenGreyout.SetActive(true);
    }
}
