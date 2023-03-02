using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaporatePlayer : MonoBehaviour
{
    private GameObject player;
    public AK.Wwise.Event evaporateplayer;

    void Start()
    {
        player = GameObject.Find("BlockPlayer");
    }
    
    void OnTriggerEnter (Collider other)
    {
        if(other.transform == player.GetComponent<Transform>() && !player.GetComponent<PlayerMovementBruce>().playerInvincible)
        {
            other.gameObject.SetActive(false);
            
            StartCoroutine(RespawnTimer());
            evaporateplayer.Post(gameObject);
        }
    }

    IEnumerator RespawnTimer()
    {
        yield return new WaitForSeconds(1.3f);
        player.GetComponent<PlayerMovementBruce>().RespawnCall();
    }
}
