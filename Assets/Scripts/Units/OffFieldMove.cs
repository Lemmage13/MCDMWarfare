using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffFieldMove : MonoBehaviour, IMove
{
    BaseUnit unit;
    private void Awake()
    {
        if (GetComponent<IMove>() != null)
        {
            Destroy(this);
        }
        unit = GetComponent<BaseUnit>();
    }
    public void SpawnPlate()
    {
        if (unit.Side)
        {
            foreach(Space space in BattlefieldManager.instance.OffFieldSpacesPlayer)
            {
                if (space.occupiedBy == null)
                {
                    space.Spawnable.SetActive(true);
                }
            }
        }
        else
        {
            foreach(Space space in BattlefieldManager.instance.OffFieldSpacesDM)
            {
                if (space.occupiedBy == null)
                {
                    space.Spawnable.SetActive(true);
                }
            }
        }
    }
    public void ClearSpawnPlate()
    {
        foreach(Space space in BattlefieldManager.instance.OffFieldSpacesPlayer)
        {
            space.Spawnable.SetActive(false);
        }
        foreach(Space space in BattlefieldManager.instance.OffFieldSpacesDM)
        {
            space.Spawnable.SetActive(false);
        }
    }
    public void MovePlate()
    {
        //nothing is done as cavalry need not move
    }
    public void ClearMovePlate()
    {
        //cavalry do not move
    }
}
