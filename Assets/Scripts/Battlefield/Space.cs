using System;
using System.Collections.Generic;
using UnityEngine;

public class Space : MonoBehaviour
{
    public GameObject Highlight;
    public GameObject Movable;
    public GameObject Attackable;
    public GameObject Spawnable;
    public bool Front;

    public int x;
    public int y;

    public List<Space> Adjacent;

    public BaseUnit occupiedBy;
    private void Awake()
    {
        Highlight.SetActive(false);
        Movable.SetActive(false);
        Attackable.SetActive(false);
        Spawnable.SetActive(false);
    }
    private void OnMouseEnter()
    {
        Highlight.SetActive(true);
    }
    private void OnMouseExit()
    {
        Highlight.SetActive(false);
    }
    private void OnMouseDown()
    {
        if (occupiedBy != null)
        {
            UnitManager.instance.UnitSelect(occupiedBy);
        }
        else if (Movable.activeSelf)
        {
            UnitManager.instance.Active.MakeMove(this);
        }
        else if (Spawnable.activeSelf)
        {
            UnitManager.instance.Active.Spawn(this);
        }
    }
    public void ListAdjacent()
    {
        Adjacent = new List<Space>();
        foreach (Space space in BattlefieldManager.instance.Spaces)
        {
            if (Distance(this, space) == 1) { Adjacent.Add(space); }
        }
    }
    bool IsAdjacent(Space space)
    {
        foreach (Space i in Adjacent)
        {
            if (i == space) { return true; }
        }
        return false;
    }
    public int Distance(Space A, Space B)
    {
        int dist = Math.Abs(A.x - B.x) + Math.Abs(A.y - B.y);
        return dist;
    }
}
