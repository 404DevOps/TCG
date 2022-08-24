using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : NetworkBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector3 startPos;
    Vector3 endPos;

    Card card;

    bool dropped = false;
    public Player player;

    Transform dropTarget;
    // Start is called before the first frame update
    void Start()
    {
        card = GetComponent<DisplayCard>().card;
        startPos = transform.position;
    }

    void Update() 
    {
        if (player == null)
            player = NetworkClient.localPlayer.gameObject.GetComponent<Player>();
            //player = GameManager.Instance.players.FirstOrDefault(p => p.isLocalPlayer);
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        startPos = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dropped)
            return;
        if (!player.isMyTurn)
            return;

        endPos = GetWorldPoint(eventData.position);
        transform.position = Vector3.Lerp(transform.position, endPos, 10);
    }

    Vector2 GetWorldPoint(Vector2 position)
    {
        return Camera.main.ScreenToWorldPoint(position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        if (dropTarget != null)
        {
            var index = GetNewChildIndex();
            if (index == -1)
                Debug.LogError("FieldCard Index could not be determined");

            var handIndex = transform.GetSiblingIndex();
            GameManager.Instance.CmdPlayCardOnField(player, index, handIndex, card.Id);

            dropped = true;
        }
        else
        {
            Debug.Log("No Drop Target present.");
            endPos = startPos;
            transform.position = startPos;
        }
    }

    private int GetNewChildIndex()
    {
        if (dropTarget.childCount < 1)
            return 0;
        //see if its to the left of the first.
        if (transform.position.x < dropTarget.GetChild(0).transform.position.x)
            return 0;
        //see if its to the right of the last.
        if (transform.position.x > dropTarget.GetChild(dropTarget.childCount - 1).transform.position.x)
            return dropTarget.childCount - 1;

        for (int i = 0; i < dropTarget.childCount - 1; i++)
        {
            var currChild = dropTarget.GetChild(i);
            //if child is to the left
            if (currChild.transform.position.x < transform.position.x)
            {
                //get next child to see if its on the right, if yes, this is the new position.
                var nextChild = dropTarget.GetChild(i + 1);
                if (nextChild.transform.position.x > transform.position.x)
                {
                    return i + 1;
                }
            }
        }

        return -1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("TriggerEnter");
        var field = collision.gameObject.GetComponent<Field>();
        if (field != null && field.owner == Owner.Player)
        {
            //Drop on Field
            if (field.transform.childCount < 7)
            {
                dropTarget = field.transform;
                Debug.Log("Field " + field.name + " set as Target");
            }
            else
            {
                dropTarget = null;
                GameManager.Instance.ShowMessage("Field already full.", Color.red);
            }
        }
        else
        {
            Debug.Log("No Drop Target set.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Drop Target reset.");
        dropTarget = null;
    }


}
