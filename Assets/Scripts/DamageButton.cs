using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DamageButton : MonoBehaviour
{
    Button button;
    PlayerStats player;
    public bool isActive;
    public GameObject redArrow;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        player = FindObjectsOfType<PlayerStats>().Where(p => p.owner == Owner.Player).FirstOrDefault();
    }

    public void SetActive()
    {
        isActive = true;
        var arrow = gameObject.AddComponent<DrawArrow>();
        arrow.actionType = ActionType.Damage;
        arrow.arrowHead = redArrow;
        button.interactable = true;
        arrow.TargetSelected += DealDamage;
    }

    public void SetInactive()
    {
        isActive = false;
        Destroy(GetComponent<DrawArrow>());
        Destroy(GetComponent<LineRenderer>()?.material);
        Destroy(GetComponent<LineRenderer>());
        button.interactable = false;
    }

    void DealDamage(object sender, DrawArrow.TargetSelectedEventArgs e)
    {
        Debug.Log("DealDamage Event handled in DamageButton.cs");
        EffectExecutor.Instance.DealDamageToPlayer();
    }
}
