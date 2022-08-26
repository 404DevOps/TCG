using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DamageButton : MonoBehaviour
{
    Button button;
    Player player;
    Player enemy;

    EffectExecutor fxExecutor;

    public bool isActive;
    public GameObject redArrow;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();

    }

    private void Update()
    {
        if (fxExecutor == null)
            fxExecutor = FindObjectOfType<EffectExecutor>();

        if (player == null)
        {
            if (NetworkClient.localPlayer != null)
            {
                player = NetworkClient.localPlayer.gameObject.GetComponent<Player>();
            }

        }
        else
        {
            var e = FindObjectsOfType<Player>().Where(p => p.netId != player.netId).ToList();
            if (e.Any())
                enemy = e.FirstOrDefault();

            if (player.isMyTurn && player.DamagePool > 0)
                SetActive();
            else
                SetInactive();

        }

    }

    public void SetActive()
    {
        if (isActive == false)
        {
            isActive = true;
            var arrow = gameObject.AddComponent<DrawArrow>();
            arrow.actionType = ActionType.Damage;
            arrow.arrowHead = redArrow;
            button.interactable = true;
            arrow.TargetSelected += DealDamage;
        }
    }

    public void SetInactive()
    {
        if (isActive == true)
        {
            isActive = false;
            Destroy(GetComponent<DrawArrow>());
            Destroy(GetComponent<LineRenderer>()?.material);
            Destroy(GetComponent<LineRenderer>());
            button.interactable = false;
        }
    }

    void DealDamage(object sender, DrawArrow.TargetSelectedEventArgs e)
    {
        Debug.Log("DealDamage Event handled in DamageButton.cs");
        DisplayCard card = e.Target.GetComponent<DisplayCard>();
        if (card != null)
            fxExecutor.DealDamage(player, enemy, card.cardInfo.fieldId);
        else
            fxExecutor.DealDamageToPlayer(player,enemy);
    }
}
