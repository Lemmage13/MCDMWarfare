using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryAttack : MonoBehaviour, IAttack
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
        if (unit.CanAttack())
        {
            foreach(BaseUnit bunit in UnitManager.instance.AllUnits)
            {
                if (bunit.Side != unit.Side)
                {
                    bunit.Occupying.Attackable.SetActive(true);
                }
            }
        }
    }
    public void ClearAttackPlate()
    {
        foreach(BaseUnit bunit in UnitManager.instance.AllUnits)
        {
            bunit.Occupying.Attackable.SetActive(false);
        }
    }
}
