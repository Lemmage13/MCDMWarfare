using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AddUnit : MonoBehaviour
{
    public UnitRep unitPrefab;
    public GameObject PlayerSide;
    public GameObject DMSide;
    public GameObject PlayerPanel;
    public GameObject DMPanel;

    //unity cannot serialise nested objects to show in inspector
    private List<int[]> playerUnits = new List<int[]>();
    private List<int[]> dmUnits = new List<int[]>();

    //index to number units
    int playerUnitIndex = 0;
    int dmUnitIndex = 0;
    //add unit functions called by corresponding buttons in ui
    public void AddPlayerUnit()
    {
        UnitRep newUnit = Instantiate(unitPrefab);
        newUnit.transform.SetParent(PlayerSide.transform, false);
        newUnit.name = "Player Unit " + playerUnitIndex.ToString();
        playerUnitIndex++;
        Debug.Log("player unit added");
    }
    public void AddDMUnit()
    {
        UnitRep newUnit = Instantiate(unitPrefab);
        newUnit.transform.SetParent(DMSide.transform, false);
        newUnit.name = "DM Unit " + dmUnitIndex.ToString();
        dmUnitIndex++;
        Debug.Log("DM unit added");
    }
    public void CommitUnits()
    {
        GetUnitAncTyp();
        UnitList.SetPlayerUnits(playerUnits);
        UnitList.SetDMUnits(dmUnits);
    }
    void GetUnitAncTyp()
    {
        for(int i = 1; i < PlayerPanel.transform.childCount; i++)
        {
            UnitRep unit = PlayerPanel.transform.GetChild(i).gameObject.GetComponent<UnitRep>();
            playerUnits.Add(unit.GetAncTyp());
        }
        for (int i = 1; i < DMPanel.transform.childCount; i++)
        {
            UnitRep unit = DMPanel.transform.GetChild(i).GetComponent<UnitRep>();
            dmUnits.Add(unit.GetAncTyp());
        }
    }
}
