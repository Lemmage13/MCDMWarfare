using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTurnButton : MonoBehaviour
{
    public GameObject StartTurn;
    public GameObject EndTurn;
    public GameObject greyout;

    private void Awake()
    {
        StartTurn.SetActive(false);
        EndTurn.SetActive(false);
        greyout.SetActive(true);
    }
    private void Start()
    {
        UnitManager.instance.UnitSelected += unitSelUpdate;
    }
    private void OnMouseDown()
    {
        BaseUnit unit = UnitManager.instance.Selected;
        if (unit != null)
        {
            if (unit.Ready && unit != UnitManager.instance.Active) //activate newly selected unit
            {
                StartTurn.SetActive(false);
                EndTurn.SetActive(true);
                BattlefieldManager.instance.clearPlates();
                unit.ActivateUnit();
            }
            else if (unit == UnitManager.instance.Active) //deactivate already active unit
            {
                StartTurn.SetActive(false);
                EndTurn.SetActive(false);
                greyout.SetActive(true);
                unit.Ready = false;
                UnitManager.instance.Active = null;
                BattlefieldManager.instance.clearPlates();
            }
        }
    }
#nullable enable
    void unitSelUpdate(BaseUnit? unit)
    {
        if (unit != null)
        {
            if (UnitManager.instance.Active == unit)
            {
                greyout.SetActive(false);
                StartTurn.SetActive(false);
                EndTurn.SetActive(true);
            }
            else if (unit.Ready)
            {
                greyout.SetActive(false);
                EndTurn.SetActive(false);
                StartTurn.SetActive(true);
            }
            else
            {
                StartTurn.SetActive(false);
                EndTurn.SetActive(false);
                greyout.SetActive(true);
            }
        }
        else
        {
            StartTurn.SetActive(false);
            EndTurn.SetActive(false);
            greyout.SetActive(true);
        }
    }
#nullable disable
}
