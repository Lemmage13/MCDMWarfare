using UnityEngine;
using UnityEngine.WSA;

public class Unit : MonoBehaviour
{
    public bool Active;

    public Space occupying;

    //properties
    public int player;
    public bool side; //1 is player, 0 is DM
    public int AP;      // ACTION POINTS MAY BE BOOLEAN
    public int bAP;
    public int MP;

    //Stats (to be added)

    private void Awake()
    {
        if (player > 0) { side = true; }
        else { side = false; }
    }
    //PLAN:  mouse down on space will activate turn action if this unit is active - generate mv/atk plates
    public void Move(Space space)
    {
        if (space.Movable.activeSelf){
            if (occupying != null) { occupying.occupiedBy = null; }
            this.transform.position = space.transform.position;
            space.occupiedBy = this;
            occupying = space;
            Active = false;
        }
    }
}
