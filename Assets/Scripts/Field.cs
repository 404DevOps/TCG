using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Field : NetworkBehaviour
{
    public Owner owner;
    public GameObject cardPlaceholder;
    public GameObject cardBack; //to play flip cards at some point

    [Client]
    public void AddFieldCard(int index, FieldCard cardInfo)
    {
        var newCard = Instantiate(cardPlaceholder, transform);
        newCard.transform.SetSiblingIndex(index);

        var placeHolder = newCard.GetComponent<CardPlaceholder>();
        placeHolder.card = GameManager.Instance.allCards.Where(c => c.Id == cardInfo.cardId).FirstOrDefault();
        placeHolder.instantiatedIn = owner == Owner.Player ? InstantiatedField.PlayerField : InstantiatedField.EnemyField;
        placeHolder.DisplayCard(cardInfo);
    }

    [Client]
    public void RemoveCard(string fieldId = null)
    {
        //if fieldId is set, destroy specific
        if (!string.IsNullOrEmpty(fieldId))
        {
            Destroy(GetChildWithFieldId(fieldId));
        }
        //if not, destroy all children of the field
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i).gameObject;
                Destroy(child.gameObject);
            }
        }
    }

    [Client]
    public void ReplaceCard(int index, FieldCard cardInfo)
    {
        Destroy(transform.GetChild(index).gameObject);

        var newCard = Instantiate(cardPlaceholder, transform);
        var placeHolder = newCard.GetComponent<CardPlaceholder>();

        placeHolder.card = GameManager.Instance.allCards.Where(c => c.Id == cardInfo.cardId).FirstOrDefault();
        placeHolder.instantiatedIn = owner == Owner.Player ? InstantiatedField.PlayerField : InstantiatedField.EnemyField;
        placeHolder.DisplayCard(cardInfo);

        newCard.transform.SetSiblingIndex(index);
    }

    public GameObject GetChildWithFieldId(string fieldId)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).gameObject.GetComponent<DisplayCard>();
            if (child.cardInfo.fieldId == fieldId)
                return child.gameObject;
        }

        Debug.Log("Field ID was not found");
        return null;
    }
}
