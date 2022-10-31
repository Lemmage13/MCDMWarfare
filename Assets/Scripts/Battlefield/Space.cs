using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Space : MonoBehaviour
{
    public GameObject Highlight;
    public GameObject Movable;
    public GameObject Attackable;

    public Unit occupiedBy;
    private void Awake()
    {
        Highlight.SetActive(false);
        Movable.SetActive(false);
    }
    private void OnMouseEnter()
    {
        Highlight.SetActive(true);
    }
    private void OnMouseExit()
    {
        Highlight.SetActive(false);
    }
    private void OnMouseDown()
    {
        if (occupiedBy != null)
        {
            UnitManager.instance.Selected = occupiedBy;
        }
        else if (Movable.activeSelf)
        {
            UnitManager.instance.Active.Move(this);
        }
    }
}
