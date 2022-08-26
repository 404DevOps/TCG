using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnButton : NetworkBehaviour
{
    public void OnClick()
    {
        GameManager.Instance.CmdEndTurn();
    }
}
