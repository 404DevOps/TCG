using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawArrow : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public GameObject target;

    public Vector3 startPosition;
    public Vector3 currentPosition;

    public ActionType actionType; 

    public bool drawing;

    private LineRenderer lineRenderer;
    public int speed = 2;
    public GameObject arrowHead;

    public event EventHandler<TargetSelectedEventArgs> TargetSelected;

    // Start is called before the first frame update
    void Start()
    {
        
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;
        lineRenderer.enabled = false;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.material = Resources.Load<Material>("Materials/ArrowMaterial");
        //lineRenderer.material.color = Color.red;
    }

    void Update()
    {
        lineRenderer.material.SetTextureOffset("_MainTex", Vector2.left * speed * Time.time);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (drawing)
        {
            currentPosition = GetWorldPoint(Input.mousePosition);

            var dist = Vector3.Distance(startPosition, currentPosition);
            var positions = dist / 1.2f;

            lineRenderer.enabled = true;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, startPosition);
            lineRenderer.SetPosition(lineRenderer.positionCount-1, currentPosition);
            lineRenderer.textureMode = LineTextureMode.Tile;
            arrowHead.SetActive(true);
        } 
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Register Drag");
        startPosition = GetWorldPoint(Input.mousePosition);
        drawing = true;
    }
    Vector2 GetWorldPoint(Vector2 point)
    {
        return Camera.main.ScreenToWorldPoint(point);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        arrowHead.SetActive(false);
        drawing = false;
        lineRenderer.enabled = false;
        lineRenderer.positionCount = 0;

        if (IsMouseOverValidTarget(eventData, out target))
        {
            OnTargetSelected();
        }
        else 
        {
            GameManager.Instance.ShowMessage("No valid Target selected.", Color.red);
        }
    }

    bool IsMouseOverValidTarget(PointerEventData eventData, out GameObject target)
    {
        //check if over valid target, if yes, set it and invoke ontargetselected event.
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        foreach (var element in raycastResults)
        {
            //enemy stats can only be targeted if damage is done, not when stun is performed.
            if (actionType == ActionType.Damage)
            {
                Debug.Log(element.gameObject.name);
                if (element.gameObject.GetComponent<PlayerStats>()?.owner == Owner.Enemy)
                {
                    target = element.gameObject;
                    return true;
                }
            }
           
            if (element.gameObject.GetComponent<DisplayCreature>()?.card?.owner == Owner.Enemy)
            {
                target = element.gameObject;
                return true;
            }
        }
        target = null;
        return false;
    }

    void OnTargetSelected()
    {
        Debug.Log("Target selected & event raised. Target = " + target.name);

        EventHandler<TargetSelectedEventArgs> handle = TargetSelected;// (this, new TargetSelectedEventArgs(target));
        handle?.Invoke(this, new TargetSelectedEventArgs(target));
    }

    public class TargetSelectedEventArgs : EventArgs
    {
        public TargetSelectedEventArgs(GameObject _target) 
        {
            Target = _target;
        }
        public GameObject Target { get; set; }
    }
}
