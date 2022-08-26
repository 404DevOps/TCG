using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    //Singleton
    public static GameManager Instance;

    //Ressource References
    public GameObject messagePrefab;
    public GameObject cardPrefab;
    public GameObject canvas;
    public List<Card> allCards;

    //Global Decks
    public MarketDeck marketDeck;
    public FireGemDeck fireGemDeck;

    //UI Stuff
    public Button EndTurnButton;
    public Button DamageButton;

    //Server Variables
    public static readonly List<Player> players = new List<Player>();
    public readonly SyncList<uint> playerIds = new SyncList<uint>();


    [SyncVar]
    public ServerState serverState;

    [SyncVar(hook = nameof(OnTurnChanged))]
    public uint currentTurnPlayer = new();

    [SyncVar]
    public bool isGameRunning;

    bool firstTurn = true;

    #region FieldManagement
    [Server]
    void InitializeGameBoard() 
    {
        if (!isGameRunning)
        {
            serverState = ServerState.Initialize;
            marketDeck.InitializeDeck();
            marketDeck.InitializeMarketField();
            fireGemDeck.InitializeDeck();

            foreach (var p in players)
            {
                p.deckCards.AddRange(GetStarterDeck());
                p.deckCards.ShuffleDeck();
            }

            ServerNextState("StartGame");
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdPlayCardOnField(Player player, int fieldIndex, int handIndex, string cardId)
    {
        //generate GUID on server to be the same on all clients
        var guid = GUID.Generate().ToString();
        var cardInfo = new FieldCard(cardId, guid);

        player.handCards.RemoveAt(handIndex);
        player.fieldCards.Insert(fieldIndex, cardInfo);

        var card = Instance.allCards.FirstOrDefault(c => c.Id == cardId);
        card.ApplyInstantEffects(player);

        if (!card.isPermanent)
            StartCoroutine(WaitForDiscard(cardInfo, player));
    }

    [Command(requiresAuthority = false)]
    public void CmdBuyMarketCard(Player player, string cardId, int index)
    {
        //get card on server to prevent cheating with costs compared to if its a parameter
        var card = Instance.allCards.Where(c => c.Id == cardId).FirstOrDefault();

        //Is it even possible to buy it?
        if (player.GoldPool >= card.cost)
        {
            player.AddToGoldPool(-card.cost);
            //put in player discard pile
            player.discardCards.Add(card.Id);

            //tell client where to remove the card
            Instance.marketDeck.RemoveMarketCardRpc(index);

            //only tell where to place it, server will choose a new card anyways.
            Instance.marketDeck.AddCardToMarketField(transform.GetSiblingIndex());

        }
        else
        {
            if (player.isLocalPlayer)
                Instance.RpcMessage("Not enough Gold.", Color.red);
        }
    }

    [Server]
    IEnumerator WaitForDiscard(FieldCard cardInfo, Player player)
    {
        yield return new WaitForSeconds(1);

        var card = player.fieldCards.FirstOrDefault(c => c.fieldId == cardInfo.fieldId);

        player.fieldCards.RemoveAll(c => c.fieldId == cardInfo.fieldId);
        player.discardCards.Add(cardInfo.cardId);
    }

    #endregion

    #region DealCards

    [Server]
    void DealCards(Player player)
    {
        //Player to go First only gets 3 Cards on the first turn
        if (firstTurn && player.netId == currentTurnPlayer)
        {
            for (int i = 0; i < 3; i++)
            {
                var card = player.deckCards.DealCardFromDeck();
                player.handCards.Add(card);
            }
            firstTurn = false;
        }
        else
        {
            //Every other Time DealCards will give 5 Cards
            for (int i = 0; i < 5; i++)
            {
                var card = player.deckCards.DealCardFromDeck();
                //null means deck is empty, therefore reshuffle
                if(card == null)
                {
                    Debug.Log("Reshuffle Deck");
                    player.deckCards.ShufflePileIntoDeck(player.discardCards);
                    card = player.deckCards.DealCardFromDeck();
                }
                player.handCards.Add(card);
            }
        }
    }

    [Server]
    List<string> GetStarterDeck()
    {
        var cards = new List<string>();
        var listCards = new List<string>();

        var starters = Instance.allCards.Where(c => c.cardType == CardType.Starter).Select(c => c.Id);
        var goldCardId = Instance.allCards.FirstOrDefault(c => c.name == "Gold").Id;

        listCards.AddRange(starters);
        while (listCards.Count < 7)
            listCards.Add(goldCardId);

        foreach (var c in listCards)
            cards.Add(c);

        return cards;
    }

    #endregion

    #region TurnManagement

    [Command(requiresAuthority = false)]
    public void CmdEndTurn()
    {
        Debug.Log("End Turn pressed");
        var p = players.FirstOrDefault(p => p.netId == currentTurnPlayer);

        //discard all cards into pile
        foreach (var card in p.handCards)
        {
            p.discardCards.Add(card);
        }

        //reset everything 
        p.DamagePool.Value = 0;
        p.GoldPool.Value = 0;
        p.handCards.Clear();

        //deal 5 new cards
        DealCards(p);

        //next player's turn
        currentTurnPlayer = players.FirstOrDefault(p => p.netId != currentTurnPlayer).netId;
    }

    [Client]
    public void OnTurnChanged(uint oldValue, uint newValue)
    {
        if (EndTurnButton == null)
            EndTurnButton = GameObject.Find("EndTurn").GetComponent<Button>(); ;
        Debug.Log("Turn Changed.");
        foreach (var p in players)
            p.isMyTurn = p.netId == newValue;

 
        if (NetworkClient.localPlayer.netId == newValue)
        {
            EndTurnButton.interactable = true;
            Debug.Log("Players Turn: NetID" + newValue);
        }
        else 
        {
            EndTurnButton.interactable = false;
        }
    }

    #endregion

    #region Utility

    [Client]
    public void ShowMessage(string message, Color color)
    {
        var newMessage = Instantiate(messagePrefab, canvas.transform);
        var hoverText = newMessage.GetComponent<HoverText>();
        hoverText.text = message;
        hoverText.color = color;
    }

    [ClientRpc]
    public void RpcMessage(string message, Color color)
    {
        ShowMessage(message, color);
    }

    #endregion

    #region GameStateManagement

    [Server]
    public void AddPlayer(Player player)
    {
        playerIds.Add(player.netId);
        players.Add(player);

        player.HealthPool.Value = 50;
        player.GoldPool.Value = 0;
        player.DamagePool.Value = 0;

        if (playerIds.Count == 2)
            ServerNextState("InitializeGameBoard");
    }

    [Server]
    public void RemovePlayer(Player player)
    {
        playerIds.Remove(player.netId);
        players.Remove(player);
        
    }

    [ClientRpc]
    public void RpcDestroyPlayerObject(Player player)
    {
        Destroy(player.gameObject);
    }

    [Server]
    void ServerNextState(string funcName)
    {
        Debug.Log("Server next state:" + funcName);
        Invoke(funcName, 1.0f);
    }

    [Server]
    void StartGame()
    {
        isGameRunning = true;
        serverState = ServerState.GameInProgress;

        //determine starting player
        var rand = Random.Range(0, players.Count);
        currentTurnPlayer = players[rand].netId;

        foreach (var p in players)
        {
            p.isMyTurn = p.netId == currentTurnPlayer;
            DealCards(p);
        }
    }

    [Server]
    public void GameOver()
    {
        RpcMessage("Game Over!!!", Color.red);
        serverState = ServerState.Ended;
    }

    public enum ServerState
    {
        Connect = 0,
        Initialize = 1,
        GameInProgress = 2,
        Ended = 3,
        Reconnect = 4
    }
    #endregion

    #region Unity Functions
    public void Awake()
    {
        Instance = this;
        //playerIds.Callback += OnPlayerIdChanged;
    }

    private void OnPlayerIdChanged(SyncList<uint>.Operation op, int itemIndex, uint oldItem, uint newItem)
    {
        var player = FindObjectsOfType<Player>().FirstOrDefault(p => p.netId == newItem);
        switch (op)
        {
            case SyncList<uint>.Operation.OP_ADD:
                players.Add(player);
                break;
            case SyncList<uint>.Operation.OP_CLEAR:
                players.Clear();
                break;
            case SyncList<uint>.Operation.OP_INSERT:
                    players.Insert(itemIndex, player);
                break;
            case SyncList<uint>.Operation.OP_REMOVEAT:
                    players.RemoveAt(itemIndex);
                break;
            case SyncList<uint>.Operation.OP_SET:
                players[itemIndex] = player;
                break;
            default:
                break;
        }
    }

    public override void OnStartServer()
    {
        serverState = ServerState.Connect;
    }

    //loads all card ressources on client & server
    public void Start()
    {
        allCards = Resources.LoadAll<Card>("Cards").ToList();
    }
    #endregion
}
