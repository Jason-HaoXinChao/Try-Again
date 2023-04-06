using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolaroidsPan : MonoBehaviour
{
    [SerializeField] Vector3 targetPosition;
    [SerializeField] float speed;
    bool once;

    void Start()
    {
        once = true;
    }

    void Update()
    {
        this.GetComponent<RectTransform>().localPosition 
            = Vector3.Lerp(this.GetComponent<RectTransform>().localPosition, targetPosition, Time.deltaTime * speed);

        if (once && this.GetComponent<RectTransform>().localPosition.x > 900)
        {
            speed = speed * 2;
            once = false;
        }
    }
}
