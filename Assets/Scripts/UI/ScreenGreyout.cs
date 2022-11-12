using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenGreyout : MonoBehaviour
{
    public TextMeshPro winText;
    private void OnEnable()
    {
        if (GameManager.Instance.Victor == Team.Players)
        {
            winText.text = "The party is victorious!";
        }
        else
        {
            winText.text = "The party has met defeat in the battle...";
        }
    }
}
