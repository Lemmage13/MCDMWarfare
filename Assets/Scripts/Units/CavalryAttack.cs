using System.Collections.Generic;
using UnityEngine;

public class CavalryAttack : MonoBehaviour, IAttack
{
    BaseUnit unit;
    private void Awake()
    {
        if (GetComponent<IAttack>() != null)
        {
            Destroy(this);
        }
        unit = GetComponent<BaseUnit>();
    }
    public void AttackPlate()
    {
        List<BaseUnit> exposedUnits = FindExposed();
        foreach (BaseUnit unit in exposedUnits)
        {
            unit.Occupying.Attackable.SetActive(true);
        }
    }
    List<BaseUnit> FindExposed()
    {
        List<BaseUnit> exposed = new List<BaseUnit>();
        List<BaseUnit> rearUnits = new List<BaseUnit>();
        List<BaseUnit> vanguardUnits = new List<BaseUnit>();
        foreach (Space space in BattlefieldManager.instance.Spaces)
        {
            if (space.occupiedBy != null)
            {
                if (space.Rank == Rank.Vanguard && space.occupiedBy.Side != unit.Side)
                {
                    vanguardUnits.Add(space.occupiedBy);
                }
                if (space.Rank == Rank.Rear && space.occupiedBy.Side != unit.Side)
                {
                    rearUnits.Add(space.occupiedBy);
                }
            }
        }
        foreach (BaseUnit vanguardU in vanguardUnits)
        {
            if (OpenPathToEdge(vanguardU.Occupying)) { exposed.Add(vanguardU); }
        }
        foreach (BaseUnit rearU in rearUnits)
        {
            exposed.Add(rearU);
        }
        if (vanguardUnits.Count == 0 || rearUnits.Count == 0)
        {
            foreach (Space space in BattlefieldManager.instance.Spaces)
            {
                if (space.occupiedBy != null)
                {
                    if (space.occupiedBy.Side != unit.Side && (space.Rank == Rank.Centre || space.Rank == Rank.Reserve))
                    {
                        if (OpenPathToEdge(space)) { exposed.Add(space.occupiedBy); }
                    }
                }
            }
        }
        return exposed;
    }
    bool OpenPathToEdge(Space space)
    {
        int ypos;
        int xpos;
        //north
        xpos = space.x;
        ypos = space.y;
        while (true)
        {
            ypos++;
            Space north = FindSpaceInPosition(xpos, ypos);
            if (north != null)
            {
                if (north.occupiedBy != null)
                {
                    break;
                }
            }
            else { return true; }
        }
        //East
        xpos = space.x;
        ypos = space.y;
        while (true)
        {
            xpos++;
            Space east = FindSpaceInPosition(xpos, ypos);
            if (east != null)
            {
                if (east.occupiedBy != null)
                {
                    break;
                }
            }
            else { return true; }
        }
        //South
        xpos = space.x;
        ypos = space.y;
        while (true)
        {
            ypos--;
            Space south = FindSpaceInPosition(xpos, ypos);
            if (south != null)
            {
                if (south.occupiedBy != null)
                {
                    break;
                }
            }
            else { return true; }
        }
        //West
        xpos = space.x;
        ypos = space.y;
        while (true)
        {
            xpos--;
            Space west = FindSpaceInPosition(xpos, ypos);
            if (west != null)
            {
                if (west.occupiedBy != null)
                {
                    break;
                }
            }
            else { return true; }
        }
        return false;
    }
    Space FindSpaceInPosition(int x, int y)
    {
        foreach (Space space in BattlefieldManager.instance.Spaces)
        {
            if (space.x == x && space.y == y)
            {
                return space;
            }
        }
        return null;
    }
    public void ClearAttackPlate()
    {
        foreach (Space space in BattlefieldManager.instance.Spaces)
        {
            space.Attackable.SetActive(false);
        }
    }
}
