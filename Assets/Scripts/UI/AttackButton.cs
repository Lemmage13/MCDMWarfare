using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        UnitManager.instance.Active.Attack(UnitManager.instance.Selected);
    }
}
