using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaporatePlayer : MonoBehaviour
{
    private GameObject player;

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
        }
    }

    IEnumerator RespawnTimer()
    {
        GameObject audioManager = GameObject.FindWithTag("AudioManager");
        audioManager.GetComponent<PlayerAudioManager>().die();
        yield return new WaitForSeconds(0.5f);
        audioManager.GetComponent<PlayerAudioManager>().respawn();
        yield return new WaitForSeconds(0.8f);
        player.GetComponent<PlayerMovementBruce>().RespawnCall();
    }
}
