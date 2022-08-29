using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConnectButton : MonoBehaviour
{
    public TMP_InputField inputName;

    private void Start()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            inputName.text = PlayerPrefs.GetString("PlayerName");
        }
    }

    public void OnConnectClick()
    {

        if (SetPlayerName())
        {
            NetworkClient.OnConnectedEvent += OnConncted;
            Debug.Log("Trying to Connect to Localhost.");
            NetworkClient.Connect("localhost");
        }
    }

    public void OnHostClick()
    {
       if(SetPlayerName())
            NetworkClient.ConnectLocalServer();
    }

    bool SetPlayerName()
    {
        if (!string.IsNullOrEmpty(inputName.text))
        {
            PlayerPrefs.SetString("PlayerName", inputName.text);
            return true;
        }

        Debug.Log("PlayerName could not be set.");
        return false;
    }

    private void OnConncted()
    {
        Debug.Log("Connected!");
    }

    private void OnConnectedToServer()
    {
        Debug.Log("Waiting for Opponent");
    }

}
