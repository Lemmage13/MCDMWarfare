using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTurnButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        Unit unit = UnitManager.instance.Selected;
        if (unit != null)
        {
            if (unit.Ready && unit != UnitManager.instance.Active)
            {
                StartCoroutine(unit.ActivateUnit());
            }
            else if (unit == UnitManager.instance.Active)
            {
                BattlefieldManager.instance.clearMovable();
                unit.Ready = false;
                UnitManager.instance.Active = null;
            }
        }
    }
}
