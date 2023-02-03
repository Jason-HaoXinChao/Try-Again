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
        if(other.transform == player.GetComponent<Transform>())
        {
            other.gameObject.SetActive(false);
            
            StartCoroutine(RespawnTimer());
        }
    }

    IEnumerator RespawnTimer()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject audioManager = GameObject.FindWithTag("AudioManager");
        audioManager.GetComponent<PlayerAudioManager>().respawn();
        yield return new WaitForSeconds(0.8f);
        player.GetComponent<PlayerMovementBruce>().RespawnCall();
    }
}
