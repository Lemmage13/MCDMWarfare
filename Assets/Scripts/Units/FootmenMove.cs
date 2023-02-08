using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class FootmenMove : MonoBehaviour, IMove
{
    BaseUnit unit;
    List<Space> movable;
    private void Awake()
    {
        if (GetComponent<IMove>() != null)
        {
            Destroy(this);
        }
        unit = GetComponent<BaseUnit>();
    }
    public void MovePlate()
    {
        movable = FindMovableSpaces();
        if (unit.MP > 0)
        {
            foreach (Space space in movable)
            {
                space.Movable.SetActive(true);
            }
        }
    }
    public void SpawnPlate()
    {
        foreach (Space space in BattlefieldManager.instance.Spaces)
        {
            if (space.Front != unit.Side && space.occupiedBy == null)
            {
                space.Spawnable.SetActive(true);
            }
        }
    }
    public void ClearSpawnPlate()
    {
        foreach (Space space in BattlefieldManager.instance.Spaces)
        {
            if (space.Spawnable.activeSelf)
            {
                space.Spawnable.SetActive(false);
            }
        }
    }
    List<Space> FindMovableSpaces()
    {
        movable = new List<Space> { unit.Occupying };
        for (int mov = 0; mov < unit.MP; mov++)
        {
            foreach (Space mv in movable)
            {
                List<Space> unoccupiedAdjacent = AdjUnoccupied(mv);
                if (mov == 0) { movable = unoccupiedAdjacent; }
                else
                {
                    foreach (Space space in unoccupiedAdjacent)
                    {
                        if (!IsSpaceInList(space, movable))
                        {
                            movable.Add(space);
                        }
                    }
                }
            }
        }
        return movable;
    }
    bool IsSpaceInList(Space space, List<Space> list)
    {
        foreach(Space i in list) { if (space == i) return true; }
        return false;
    }
    List<Space> AdjUnoccupied(Space space)
    {
        List<Space> unoccupied = new List<Space>();
        foreach (Space adj in space.Adjacent)
        {
            if (adj.occupiedBy == null) { unoccupied.Add(adj); }
        }
        return unoccupied;
    }
    public void ClearMovePlate()
    {
        foreach (Space space in movable)
        {
            space.Movable.SetActive(false); 
        }
    }
}