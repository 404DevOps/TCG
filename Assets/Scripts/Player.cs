using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player : NetworkBehaviour
{
    public Owner Owner;
    public NetworkIdentity Identity;
    public DisplayPlayerStats displayStats;

    public Deck deck;
    public DiscardPile discardPile;
    public Hand hand;
    public Field field;

    public readonly SyncList<string> deckCards = new SyncList<string>();
    public readonly SyncList<string> handCards = new SyncList<string>();
    public readonly SyncList<string> fieldCards = new SyncList<string>();

    public string PlayerName { get; set; }

    public int DamagePool;
    public int HealthPool;
    public int GoldPool;

    public bool isMyTurn = false;

    public void Start()
    {
        DamagePool = 0;
        HealthPool = 50;
        GoldPool = 0;

        Owner = Owner.Player;

        handCards.Callback += OnHandCardChanged;
        fieldCards.Callback += OnFieldCardChanged;

        if (isLocalPlayer)
        {
            PlayerName = "Player";
            displayStats = GameObject.Find("PlayerStats").GetComponent<DisplayPlayerStats>();
            discardPile = GameObject.Find("PlayerDiscardPile").GetComponent<DiscardPile>();
            deck = GameObject.Find("PlayerDeck").GetComponent<Deck>();
            hand = GameObject.Find("PlayerHand").GetComponent<Hand>();
            field = GameObject.Find("PlayerField").GetComponent<Field>();
        }
        else
        {
            PlayerName = "Enemy";
            displayStats = GameObject.Find("EnemyStats").GetComponent<DisplayPlayerStats>();
            discardPile = GameObject.Find("EnemyDiscardPile").GetComponent<DiscardPile>();
            deck = GameObject.Find("EnemyDeck").GetComponent<Deck>();
            hand = GameObject.Find("EnemyHand").GetComponent<Hand>();
            field = GameObject.Find("EnemyField").GetComponent<Field>();
        }

        displayStats.SetPlayerName(PlayerName);
        displayStats.UpdateTexts(GoldPool, DamagePool, HealthPool);
    }

    #region Callbacks

    void OnHandCardChanged(SyncList<string>.Operation op, int index, string oldItem, string newItem)
    {
        switch (op)
        {
            case SyncList<string>.Operation.OP_ADD:
                hand.AddHandCard(newItem);
                break;
            case SyncList<string>.Operation.OP_REMOVEAT:
                hand.RemoveCard(index);
                break;
            case SyncList<string>.Operation.OP_CLEAR:
                hand.RemoveCard(-1); //removes all
                break;
        }
    }

    void OnFieldCardChanged(SyncList<string>.Operation op, int index, string oldItem, string newItem)
    {
        switch (op)
        {
            case SyncList<string>.Operation.OP_ADD:
                field.AddFieldCard(index, newItem);
                break;
            case SyncList<string>.Operation.OP_INSERT:
                field.AddFieldCard(index, newItem);
                break;
            case SyncList<string>.Operation.OP_REMOVEAT:
                field.RemoveCard(index);
                break;
            case SyncList<string>.Operation.OP_CLEAR:
                field.RemoveCard(-1); //removes all
                break;
        }
    }

    internal void DetermineTurn()
    {
        if (GameManager.Instance.currentTurnPlayer == this)
        {
            isMyTurn = true;
            GameManager.Instance.ShowMessage("It's your Turn!", Color.red);
        }
        else
        {
            isMyTurn = false;
            GameManager.Instance.ShowMessage("It's your Enemies Turn!", Color.red);
        }
    }

    #endregion

    //called when player is spawned on server
    [Server]
    public override void OnStartServer()
    {
        GameManager.Instance.AddPlayer(this);
    }

    [Server]
    //called when player is removed from server
    public override void OnStopServer()
    {
        GameManager.Instance.RemovePlayer(this);
    }

    #region Handle Playerstats

    [Server]
    public void AddToGoldPool(int amount)
    {
        GoldPool += amount;
        UpdateStatsRpc();
    }

    [Server]
    public void AddToDamagePool(int amount)
    {
        DamagePool += amount;
        UpdateStatsRpc();
    }

    [Server]
    public void AddToHealthPool(int amount)
    {
        HealthPool += amount;
        UpdateStatsRpc();
    }

    [ClientRpc]
    public void UpdateStatsRpc()
    {
        displayStats.UpdateTexts(GoldPool, DamagePool, HealthPool);
    }

    #endregion

}
