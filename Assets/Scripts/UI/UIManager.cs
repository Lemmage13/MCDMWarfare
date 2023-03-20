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
    }
    public void SelectUpdate(BaseUnit selected)
    {
        if (UnitManager.instance.Active != null)
        {
            BaseUnit Active = UnitManager.instance.Active;
            if (selected.Occupying.Attackable.activeSelf)
            { AttackButton.SetActive(true); AttackText.SetActive(true); }
            else
            { AttackButton.SetActive(false); AttackText.SetActive(false); }
        }
    }
    public void gameEnd()
    {
        screenGreyout.SetActive(true);
    }
}
