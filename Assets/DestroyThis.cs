using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyThis : MonoBehaviour
{
    void OnTriggerEnter(Collider c)
    {
        Destroy(c.gameObject);
    }
}
