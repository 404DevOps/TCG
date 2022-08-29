using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Player : NetworkBehaviour
{
    public Owner owner;
    public DisplayPlayerStats displayStats;

    public DiscardPile discardPile;
    public Hand hand;
    public Field field;

    public readonly SyncList<string> deckCards = new SyncList<string>();
    public readonly SyncList<string> handCards = new SyncList<string>();
    public readonly SyncList<FieldCard> fieldCards = new SyncList<FieldCard>();
    public readonly SyncList<string> discardCards = new SyncList<string>();

    public readonly SyncVar<string> PlayerName = new SyncVar<string>("");

    public readonly SyncVar<int> DamagePool = new SyncVar<int>(0);
    public readonly SyncVar<int> HealthPool = new SyncVar<int>(0);
    public readonly SyncVar<int> GoldPool = new SyncVar<int>(0);

    [SyncVar(hook = nameof(OnTurnChanged))]
    public bool isMyTurn;

    public string _playername;

    //initialize references locally
    [Client]
    public void Start()
    {
        handCards.Callback += OnHandCardChanged;
        fieldCards.Callback += OnFieldCardChanged;
        discardCards.Callback += OnDiscardCardChanged;

        GoldPool.Callback += OnGoldPoolChanged;
        HealthPool.Callback += OnHealthPoolChanged;
        DamagePool.Callback += OnDamagePoolChanged;

        //PlayerName.Callback += OnPlayerNameChanged;

        if (isLocalPlayer)
        {
            //_playername = PlayerPrefs.GetString("PlayerName");
            _playername = "Player";
            owner = Owner.Player;           
            displayStats = GameObject.Find("PlayerStats").GetComponent<DisplayPlayerStats>();
            discardPile = GameObject.Find("PlayerDiscardPile").GetComponent<DiscardPile>();
            hand = GameObject.Find("PlayerHand").GetComponent<Hand>();
            field = GameObject.Find("PlayerField").GetComponent<Field>();
        }
        else
        {
            _playername = "Enemy";
            owner = Owner.Enemy;
            displayStats = GameObject.Find("EnemyStats").GetComponent<DisplayPlayerStats>();
            discardPile = GameObject.Find("EnemyDiscardPile").GetComponent<DiscardPile>();
            hand = GameObject.Find("EnemyHand").GetComponent<Hand>();
            field = GameObject.Find("EnemyField").GetComponent<Field>();
        }

        displayStats.SetPlayerName(_playername);
        displayStats.UpdateTexts(GoldPool, DamagePool, HealthPool);
    }



    #region Callbacks

    [Client]
    void OnDiscardCardChanged(SyncList<string>.Operation op, int itemIndex, string oldItem, string newItem)
    {
        switch (op)
        {
            case SyncList<string>.Operation.OP_ADD:
                discardPile.AddCardToPile(newItem);
                break;
            case SyncList<string>.Operation.OP_CLEAR:
                discardPile.EmptyPile();
                break;
        }
    }
    [Client]
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
    [Client]
    void OnFieldCardChanged(SyncList<FieldCard>.Operation op, int index, FieldCard oldItem, FieldCard newItem)
    {
        switch (op)
        {
            case SyncList<FieldCard>.Operation.OP_ADD:
                field.AddFieldCard(index, newItem);
                break;
            case SyncList<FieldCard>.Operation.OP_INSERT:
                field.AddFieldCard(index, newItem);
                break;
            case SyncList<FieldCard>.Operation.OP_REMOVEAT:
                field.RemoveCard(oldItem.fieldId);
                break;
            case SyncList<FieldCard>.Operation.OP_CLEAR:
                field.RemoveCard(null); //removes all
                break;
            case SyncList<FieldCard>.Operation.OP_SET:
                field.ReplaceCard(index, newItem);
                break;
        }
    }

    [Client]
    void OnGoldPoolChanged(int oldValue, int newValue)
    {
        displayStats.UpdateTexts(newValue, null, null);
    }
    [Client]
    void OnDamagePoolChanged(int oldValue, int newValue)
    {
        displayStats.UpdateTexts(null, newValue, null);
    }
    [Client]
    void OnHealthPoolChanged(int oldValue, int newValue)
    {
        displayStats.UpdateTexts(null, null, newValue);
    }
    [Client]
    void OnPlayerNameChanged(string oldValue, string newValue)
    {
        displayStats.SetPlayerName(newValue);
    }

    [Client]
    void OnTurnChanged(bool oldValue, bool newValue)
    {
        if (isLocalPlayer)
        {
            if (newValue)
            {
                GameManager.Instance.ShowMessage("It's your Turn!", Color.red);
            }
            else
            {
                GameManager.Instance.ShowMessage("It's your Enemies Turn!", Color.red);
            }

        }

    }

    #endregion

    #region ServerFunctions

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

    #endregion


    #region Handle Playerstats

    [Server]
    public void AddToGoldPool(int amount)
    {
        GoldPool.Value += amount;
    }

    [Server]
    public void AddToDamagePool(int amount)
    {
        DamagePool.Value += amount;
    }

    [Server]
    public void AddToHealthPool(int amount)
    {
        HealthPool.Value += amount;
    }
    #endregion
}
