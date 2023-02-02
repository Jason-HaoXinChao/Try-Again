using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTrapTrigger : MonoBehaviour
{
    public GameObject boxTrap;

    void OnTriggerEnter (Collider other)
    {
        if(other.transform == GameObject.Find("Player").GetComponent<Transform>())
        {
            boxTrap.SetActive(true);
        }
    }
}
