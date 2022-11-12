using System.Collections;
using UnityEngine;
using UnityEngine.WSA;

public class Unit : MonoBehaviour
{
    public bool Ready = false;
    public bool Alive = true;
    public bool Diminished = false;

    public Space occupying;

    //properties
    public int player;
    public bool side; //1 is player, 0 is DM -- useful while player does not have its own class
    
    //consumable stats
    public int AP;
    public int bAP;
    public int MP;
    public int Casualties;

    //Stats
    public int ATK = 3;    //Attack
    public int DEF = 12;    //Defence
    public int POW = 2;    //Power
    public int TOU = 12;    //Toughness
    public int MOR = 1;    //Morale
    public int COM = 2;    //Command
    public int NoA = 1;    //number of attacks
    public int DMG = 1;    //Damage

    public int size = 6;   //Size
    public int MVMT = 1;   //number of spaces of movement per action
    public int tier = 1;

    private void Awake()
    {
        Casualties = size;
    }
    //Activating unit using turn button will start unit turn - atk/mv plates generated w/ Actions()
    //unit will wait for instruction
    public IEnumerator ActivateUnit()
    {
        BattlefieldManager.instance.clearPlates();
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
    //spawn will make a spawn plate to move a unit onto the battlefield
    public IEnumerator Spawn()
    {
        MP = 1;
        BattlefieldManager.instance.SpawnPlate(true, side);
        while (MP > 0) { yield return null; }
        BattlefieldManager.instance.clearPlates();
    }
    //Actions() checks which action points the unit has and generates a movement/attack/action plate for each
    private void Actions()
    {
        if (AP > 0)
        { 
            BattlefieldManager.instance.MovePlate(occupying);
            BattlefieldManager.instance.AttackPlate(occupying);
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
    //possible improvement would _convert_ action point to movement points for robustness
    //when dealing with units with multiple moves
    public void Move(Space space)
    {
        if (space.Movable.activeSelf){
            if (occupying != null) { occupying.occupiedBy = null; }
            Vector3 spacePos = space.transform.position;
            Vector3 target = new Vector3(spacePos[0], spacePos[1], -3);
            this.transform.position = target;
            space.occupiedBy = this;
            occupying = space;
            BattlefieldManager.instance.clearPlates();
            if (MP > 0) { MP--; }
            else if (AP > 0) { AP--; }
        }
    }
    //attack unit - if hit - deal damage, if power test success - deal more damage
    public void Attack(Unit target)
    {
        if(rollTest(ATK, target.DEF, null))
        {
            Debug.Log("hit");
            target.Casualties--;
            if (rollTest(POW, target.TOU, null))
            {
                Debug.Log("damage");
                target.Casualties = target.Casualties - DMG;
            }
            target.CheckCasualties();
        }

        BattlefieldManager.instance.clearPlates();
        UIManager.instance.AttackButton.SetActive(false);
        AP--;
        UnitManager.instance.unitSelect(this);
    }
    public void CheckCasualties()
    {
        if (Casualties < 1) { Perish(); }
    }
    public void Perish()
    {
        occupying.occupiedBy = null;
        Casualties = 0;
        Alive = false;
        this.transform.position = new Vector3(100, 100, 100);
        Debug.Log("Unit died " + name);
    }
    //make a test versus a target number - roll d20, add modifier, determine success
    public bool rollTest(int bonus, int target, bool? adv)
    {
        if (GameManager.Instance.dieRoll(20, adv) + bonus >= target) { return true; }
        else { return false; }
    }
}
