using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;
    public GameObject messagePrefab;
    public GameObject canvas;

    public DisplayPlayerStats myPlayer;
    public DisplayPlayerStats enemyPlayer;


    //Server Variables
    public List<PlayerData> players;

    //[SyncVar(hook = UpdatePlayerHand())]
    //public List<Card> playerHandCards;
    //public SyncList<Card> playerDeckCards;

    //public SyncList<Card> enemyHandCards;
    //public SyncList<Card> enemyDeckCards;

    //public SyncList<Card> marketCards;

    //public SyncVar<int> fireGemCount;

    public GameObject cardPrefab;
    GameObject EnemyField, PlayerField, PlayerHand, EnemyHand;
    // Start is called before the first frame update

    [Server]
    public void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void GameOver()
    {
        ShowMessage("Game Over!!!", Color.red);
    }

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
}
