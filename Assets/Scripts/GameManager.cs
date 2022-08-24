using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public readonly SyncList<Player> players = new SyncList<Player>();
    [SyncVar]
    public ServerState serverState;

    [SyncVar(hook = nameof(OnTurnChanged))]
    public Player currentTurnPlayer = null;

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
        player.fieldCards.Insert(fieldIndex, cardId);
        player.handCards.RemoveAt(handIndex);
        var card = Instance.allCards.FirstOrDefault(c => c.Id == cardId);
        card.ApplyInstantEffects();
    }

    #endregion

    #region DealCards

    [Server]
    void DealCards(Player player)
    {
        //Player to go First only gets 3 Cards on the first turn
        if (firstTurn && player == currentTurnPlayer)
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
        //next player's turn
        currentTurnPlayer = Instance.players.FirstOrDefault(p => p.netId != currentTurnPlayer.netId);
    }

    [Client]
    public void OnTurnChanged(Player oldValue, Player newValue)
    {
        if (EndTurnButton == null)
            EndTurnButton = GameObject.Find("EndTurn").GetComponent<Button>(); ;
        Debug.Log("Turn Changed.");
        //foreach (var p in players)
        //    p.DetermineTurn();

        //Only Updates the Local player.
        NetworkClient.localPlayer.gameObject.GetComponent<Player>().DetermineTurn();

        if (newValue.isLocalPlayer)
        {
            EndTurnButton.interactable = true;
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
        if (isLocalPlayer)
        { 
            var newMessage = Instantiate(messagePrefab, canvas.transform);
            var hoverText = newMessage.GetComponent<HoverText>();
            hoverText.text = message;
            hoverText.color = color;
        }
    }

    #endregion

    #region GameStateManagement

    [Server]
    public void AddPlayer(Player player)
    {
        if (players.Count <= 2)
            players.Add(player);
        else
        {
            Debug.LogError("Third Player tried to Join.");
        }

        if (players.Count == 2)
            ServerNextState("InitializeGameBoard");
    }

    [Server]
    public void RemovePlayer(Player player)
    {
        players.Remove(player);
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
        currentTurnPlayer = players[rand];

        foreach (var p in players)
        {
            DealCards(p);
        }
    }

    public void GameOver()
    {
        ShowMessage("Game Over!!!", Color.red);
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
