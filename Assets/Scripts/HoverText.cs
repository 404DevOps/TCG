using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HoverText : MonoBehaviour
{
    public float duration = 3;

    float startTime;
    float endTime;
    float speed = 0.003f;

    float waitDuration = 0.0f;

    public string text = "None";
    public Color color = Color.red;

    TextMeshProUGUI tmp;
    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.color = color;

        startTime = Time.deltaTime;
        endTime = Time.deltaTime + duration;
    }

    // Update is called once per frame
    void Update()
    {
        if (startTime > endTime)
        {
            Destroy(gameObject);
        }
        else {
            startTime += Time.deltaTime;
            if (startTime > waitDuration)
            {
                gameObject.transform.Translate(Vector3.up * speed);
                tmp.alpha -= 0.001f;  
            }
        }
    }
}
