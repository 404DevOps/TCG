using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector3 startPos;
    Vector3 endPos;
    bool drag = false;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        endPos = startPos;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(transform.position != endPos)
            transform.position = Vector3.Lerp(transform.position, endPos, 10);
        
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("BeginDrag");
        startPos = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        
        drag = true;
        endPos = eventData.position;
        Debug.Log("Draggin to " + endPos);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        drag = false;
        endPos = startPos;
    }

}
