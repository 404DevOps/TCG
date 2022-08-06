using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler//, IPointerEnterHandler, IPointerExitHandler
{
    Vector3 startPos;
    Vector3 endPos;

    bool dropped = false;

    Transform dropTarget;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (dropped)
            return;
        Debug.Log("BeginDrag");
      
            startPos = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dropped)
            return;
        endPos = eventData.position;
        transform.position = Vector3.Lerp(transform.position, endPos, 10);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dropped)
            return;
       
        if (dropTarget != null)
        {
            transform.SetParent(dropTarget);
            
            //only calculate position if theres already a child there
            if (dropTarget.childCount > 0)
            {
                SetChildPosition();
            }

            dropped = true;
        }
        else 
        {
            Debug.Log("No Drop Target present.");
            endPos = startPos;
            transform.position = startPos;
        }
    }

    private void SetChildPosition()
    {
        //see if its to the left of the first.
        if (transform.position.x < dropTarget.GetChild(0).transform.position.x)
            transform.SetAsFirstSibling();
        //see if its to the right of the last.
        if (transform.position.x > dropTarget.GetChild(dropTarget.childCount - 1).transform.position.x)
            transform.SetAsLastSibling();

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
                    //this is the new position
                    Debug.Log("New Child Index = " + i + 1);
                    transform.SetSiblingIndex(i + 1);
                    break;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("TriggerEnter");
        var dropZone = collision.gameObject.GetComponent<DropZone>();
        if (dropZone != null && dropZone.owner == Owner.Player)
        {
            Debug.Log("DropZone is Player");
            if (dropZone.transform.childCount < 7)
            {
                dropTarget = dropZone.transform;
                Debug.Log("Field set as Target");
            }
            else 
            {
                dropTarget = null;
                Debug.Log("Field already full.");
            }
                
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        dropTarget = null;
    }
}
