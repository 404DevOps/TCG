using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public GameObject btnDealDamage;
    // Start is called before the first frame update
    void OnEnable()
    {
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        transform.position = new Vector3(pos.x,pos.y, 1);

        transform.LookAt(btnDealDamage.transform, Vector3.forward);
        
    }
}
