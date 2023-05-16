using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitRep : MonoBehaviour
{
    [SerializeField] TMP_Dropdown Ancestry;
    [SerializeField] TMP_Dropdown Type;

    public int[] GetAncTyp()
    {
        return new int[] { Ancestry.value, Type.value };
    }
}
