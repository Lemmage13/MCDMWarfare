using System.Collections;
using UnityEngine;
using UnityEngine.WSA;

public class Unit : MonoBehaviour
{
    public bool Ready = false;

    public Space occupying;

    //properties
    public int player;
    public bool side; //1 is player, 0 is DM
    
    public bool AP;      // ACTION POINTS MAY BE BOOLEAN OR INT - WILL NEED TO CHECK LATER
    public bool bAP;
    public bool MP;

    //Stats (to be added)

    private void Awake()
    {
        
    }
    //Activating unit using turn button will start unit turn - atk/mv plates generated automatically
    public IEnumerator ActivateUnit()
    {
        UnitManager.instance.Active = this;
        AP = true;
        MP = true;
        bAP = true;
        while (UnitManager.instance.Active == this)
        {
            Actions();
            bool acted = false;
            while (!acted)
            {
                yield return null;
                acted = CheckSpentPoints();
            }
            yield return null;
        }
        Ready = false;
    }
    public IEnumerator Spawn()
    {
        MP = true;
        BattlefieldManager.instance.SpawnPlate(true, side);
        while (MP) { yield return null; }
        BattlefieldManager.instance.clearMovable();
    }
    private void Actions()
    {
        if (AP)
        {
            BattlefieldManager.instance.MovePlate(occupying);
        }
        else if (MP)
        {
            BattlefieldManager.instance.MovePlate(occupying);
        }
        if (bAP)
        {
            //do stuff for bonus action (if available)
        }
    }
    private bool CheckSpentPoints()
    {
        if (!AP) { return true; }
        else if (!MP) { return true; }
        else if (!bAP) { return true; }
        else { return false; }
    }
    public void Move(Space space)
    {
        if (space.Movable.activeSelf){
            if (occupying != null) { occupying.occupiedBy = null; }
            this.transform.position = space.transform.position;
            space.occupiedBy = this;
            occupying = space;
            BattlefieldManager.instance.clearMovable();
            if (MP) { MP = false; }
            else if (AP) { AP = false; }
        }
    }
}
