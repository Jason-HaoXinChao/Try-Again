using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaporatePlayer : MonoBehaviour
{
    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
    }
    
    void OnTriggerEnter (Collider other)
    {
        if(other.transform == player.GetComponent<Transform>())
        {
            other.gameObject.SetActive(false);
            
            StartCoroutine(RespawnTimer());
        }
    }

    IEnumerator RespawnTimer()
    {
        yield return new WaitForSeconds(1);
        player.GetComponent<PlayerMovementBruce>().RespawnCall();
    }
}