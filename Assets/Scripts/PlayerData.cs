using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : NetworkBehaviour
{
    public Owner Owner;
    public NetworkIdentity Identity;
    public DisplayPlayerStats displayStats;

    public string PlayerName { get; set; }
    public int DamagePool { get; set; }
    public int HealthPool { get; set; }
    public int GoldPool { get; set; }

    public override void OnStartClient()
    {
        Debug.Log("Player connected: " + netId);

        DamagePool = 0;
        HealthPool = 50;
        GoldPool = 0;

        if (isLocalPlayer)
        {
            PlayerName = "Local Player";
            Owner = Owner.Player;
            displayStats = GameObject.Find("PlayerStats").GetComponent<DisplayPlayerStats>();
        }
        else
        {
            PlayerName = "Enemy";
            Owner = Owner.Enemy;
            displayStats = GameObject.Find("EnemyStats").GetComponent<DisplayPlayerStats>();
        }

        displayStats.SetPlayerName(PlayerName);
        GameManager.Instance.players.Add(this);
        displayStats.UpdateTexts(GoldPool, DamagePool, HealthPool);
    }

    [Server]
    public void AddToGoldPool(int amount)
    {
        GoldPool += amount;
        UpdateStats();
    }

    [Server]
    public void AddToDamagePool(int amount)
    {
        DamagePool += amount;
    }

    [Server]
    public void AddToHealthPool(int amount)
    {
        HealthPool += amount;
    }

    [ClientRpc]
    public void UpdateStats()
    {
        displayStats.UpdateTexts(GoldPool, DamagePool, HealthPool);
    }
}
