using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseUnit))]
public class InfantryAttack : MonoBehaviour, IAttack
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
            foreach (Space adj in unit.Occupying.Adjacent) 
            { 
                if (adj.occupiedBy != null && adj.occupiedBy.Side != unit.Side)
                {
                    adj.Attackable.SetActive(true);
                }
            }
        }
    }
    public void ClearAttackPlate()
    {
        foreach(Space adj in unit.Occupying.Adjacent) { adj.Attackable.SetActive(false); }
    }
}
