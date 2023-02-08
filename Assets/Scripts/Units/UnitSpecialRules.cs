using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BaseUnit))]
public class UnitSpecialRules : MonoBehaviour
{
    public delegate void MadeTest();
    public MadeTest MORT;
    public MadeTest COMT;
    BaseUnit unit;
    private void Awake()
    {
        unit = GetComponent<BaseUnit>();
        switch (unit.Ancestry)
        {
            case Ancestry.Human:
                MORT += Adaptable;
                COMT += Adaptable;
                break;
            case Ancestry.Elf:
                break;
            case Ancestry.Dwarf:
                break;
        }
    }
    void Adaptable()
    {
        Debug.Log("Adaptable!");
        unit.Adv = true;
    }
}
