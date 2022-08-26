using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Buyable : MonoBehaviour, IPointerClickHandler
{
    Card card;
    Player player;

    void Update()
    {
        if (player == null)
        {
            if(NetworkClient.localPlayer != null)
                player = NetworkClient.localPlayer.gameObject.GetComponent<Player>();
        }     
    }

    public void Start()
    {
        card = GetComponent<DisplayCard>().card;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!player.isMyTurn)
            return;

        GameManager.Instance.CmdBuyMarketCard(player, card.Id, transform.GetSiblingIndex());
    }
}
