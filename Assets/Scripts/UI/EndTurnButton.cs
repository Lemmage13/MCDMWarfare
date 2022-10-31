using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        GameManager.Instance.EndTurn(GameManager.Instance.turnorder[GameManager.Instance.activePlayer]);
    }
}
