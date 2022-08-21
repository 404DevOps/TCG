using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject messagePrefab;
    public GameObject canvas;

    public PlayerStats myPlayer;
    public PlayerStats enemyPlayer;
    // Start is called before the first frame update
    void Start()
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
        var newMessage = Instantiate(messagePrefab, canvas.transform);
        var hoverText = newMessage.GetComponent<HoverText>();
        hoverText.text = message;
        hoverText.color = color;
    }
}
