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
    
    public int AP;
    public int bAP;
    public int MP;

    //Stats
    int ATK;    //Attack
    int DEF;    //Defence
    int POW;    //Power
    int TOU;    //Toughness
    int MOR;    //Morale
    int COM;    //Command
    int NoA;    //number of attacks
    int DMG;    //Damage

    int size;   //Size
    int MVMT;   //number of spaces of movement per action

    private void Awake()
    {
        
    }
    //Activating unit using turn button will start unit turn - atk/mv plates generated w/ Actions()
    //unit will wait for instruction
    public IEnumerator ActivateUnit()
    {
        UnitManager.instance.Active = this;
        AP = 1;
        MP = 1;
        bAP = 1;
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
    //spawn will move a unit that does not occupy a space onto the battlefield
    public IEnumerator Spawn()
    {
        MP = 1;
        BattlefieldManager.instance.SpawnPlate(true, side);
        while (MP > 0) { yield return null; }
        BattlefieldManager.instance.clearMovable();
    }
    //Actions() checks which action points the unit has and generates a movement/attack/action plate for each
    private void Actions()
    {
        if (AP > 0)
        { 
            BattlefieldManager.instance.MovePlate(occupying);
        }
        else if (MP > 0)
        {
            BattlefieldManager.instance.MovePlate(occupying);
        }
        if (bAP > 0)
        {
            //do stuff for bonus action (if available)
        }
    }
    private bool CheckSpentPoints()
    {
        if (AP == 0) { return true; }
        else if (MP == 0) { return true; }
        else if (bAP == 0) { return true; }
        else { return false; }
    }
    //move action moves unit into unoccupied space and decrements movement points or action points
    //possible improvement would convert action point to movement points for robustness
    //when dealing with units with multiple moves
    public void Move(Space space)
    {
        if (space.Movable.activeSelf){
            if (occupying != null) { occupying.occupiedBy = null; }
            this.transform.position = space.transform.position;
            space.occupiedBy = this;
            occupying = space;
            BattlefieldManager.instance.clearMovable();
            if (MP > 0) { MP--; }
            else if (AP > 0) { AP--; }
        }
    }
}
