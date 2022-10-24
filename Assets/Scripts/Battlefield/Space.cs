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
        if (Movable.activeSelf)
        {
            int i = 0;
            while (!UnitManager.instance.Units[i].Active) { i++; }
            UnitManager.instance.Units[i].Move(this);
        }
    }
}
