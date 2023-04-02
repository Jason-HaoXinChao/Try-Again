using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] float DestroyTime;
    [SerializeField] Vector3 offset;
    [SerializeField] Vector3 randomizeIntensity;
    void Start()
    {
        Destroy(this.gameObject, DestroyTime);
        transform.localPosition += offset;
        transform.localPosition += new Vector3(
            Random.Range(-randomizeIntensity.x, randomizeIntensity.x),
            Random.Range(-randomizeIntensity.y, randomizeIntensity.y),
            0);
    }
}
