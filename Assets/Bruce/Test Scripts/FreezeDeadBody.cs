using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeDeadBody : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(FreezeBody());
    }

    IEnumerator FreezeBody()
    {
        yield return new WaitForSeconds(3);
        this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
}
