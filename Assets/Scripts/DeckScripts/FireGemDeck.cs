using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireGemDeck : DeckBase
{
    public DiscardPile discardPile;
    public Player player;
    public GameObject cardPlaceholder;

    public Card fireGemCard;

    private void Update()
    {
        if (player == null)
        {
            if (NetworkClient.localPlayer != null)
                player = NetworkClient.localPlayer.gameObject.GetComponent<Player>();
        }
    }

    [Client]
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (!player.isMyTurn)
            return;

        CmdBuyFireGem(player);
    }

    [Command(requiresAuthority = false)]
    public void CmdBuyFireGem(Player player)
    {
        //send command to server to check if possible to buy && add
        if (player.GoldPool >= fireGemCard.cost)
        {
            player.AddToGoldPool(-fireGemCard.cost);
            player.discardCards.Add(fireGemCard.Id);
            cards.RemoveAt(0);

            if (cards.Count < 1)
                RpcDestroyFireGem();
        }
        else
        {   
           GameManager.Instance.TargetRpcMessage(player.connectionToClient, "Not enough Gold.", Color.red);
        }
    }

    [Server]
    public override void InitializeDeck()
    {
        string card = DrawNextCard();
        fireGemCard = GameManager.Instance.allCards.Where(c => c.cardType == CardType.FireGem).FirstOrDefault();

        for (int i = 0; i <= 16; i++)
        {
            cards.Add(fireGemCard.Id);
        }

        RpcShowFireGem();
    }

    [ClientRpc]
    void RpcShowFireGem()
    {
        var newCard = Instantiate(cardPlaceholder, transform);
        var placeHolder = newCard.GetComponent<CardPlaceholder>();

        placeHolder.card = GameManager.Instance.allCards.Where(c => c.cardType == CardType.FireGem).FirstOrDefault();
        placeHolder.instantiatedIn = owner == Owner.Player ? InstantiatedField.PlayerField : InstantiatedField.EnemyField;
        placeHolder.DisplayCard();
    }

    [ClientRpc]
    void RpcDestroyFireGem()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        } 
    }
}
