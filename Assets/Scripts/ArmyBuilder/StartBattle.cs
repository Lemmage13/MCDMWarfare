using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBattle : MonoBehaviour
{
    public void BattleStarter()
    {
        GameManager.Instance.UpdateGameState(GameState.Battlefield);
    }
}
