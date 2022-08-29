using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AutoHostClient : MonoBehaviour
{
    public NetworkManager networkManager;
    public TMP_InputField inputName;

    public string networkAdress;

    private void Start()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            inputName.text = PlayerPrefs.GetString("PlayerName");
        }

        //if (Application.isBatchMode)
        //{
        //    networkManager.networkAddress = networkAdress;
        //    networkManager.StartHost();

        //    Debug.Log($"Host \"{networkAdress}\" started.");
        //}
    }

    public void OnConnectClick()
    {

        if (SetPlayerName())
        {
            Debug.Log("Trying to Connect to Localhost.");
            networkManager.networkAddress = networkAdress;
            networkManager.StartClient();
        }
    }

    public void OnHostClick()
    {
        if (SetPlayerName())
        {
            networkManager.networkAddress = networkAdress;
            networkManager.StartHost();

            Debug.Log($"Host \"{networkAdress}\" started.");
        }
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
}
