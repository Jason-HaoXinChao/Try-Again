using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuRagdollSpawn : MonoBehaviour
{
    [SerializeField] private GameObject ragdoll;
    [SerializeField] private int timer;
    void Start()
    {
        SpawnDoll();
    }

    void SpawnDoll()
    {
        GameObject temp = Instantiate(ragdoll, this.transform);
        StartCoroutine(WaitDoll());
    }

    IEnumerator WaitDoll()
    {
        yield return new WaitForSeconds(timer);
        SpawnDoll();
    }
}
