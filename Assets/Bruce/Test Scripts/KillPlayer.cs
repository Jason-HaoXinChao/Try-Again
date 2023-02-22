using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private GameObject deadPlayer;

    void Start()
    {
        player = GameObject.Find("BlockPlayer");
    }

    void OnTriggerEnter (Collider other)
    {
        if(other.transform == player.GetComponent<Transform>())
        {
            if (!player.GetComponent<PlayerMovementBruce>().playerInvincible) {
                GameObject corpse = Instantiate(deadPlayer, other.transform.position, other.transform.rotation) as GameObject;
                
                other.gameObject.SetActive(false);

                StartCoroutine(RespawnTimer());
            } else {
                player.GetComponent<PlayerMovementBruce>().RemoveHorizontalInertia();
            }

        }
    }

    IEnumerator RespawnTimer()
    {
        GameObject audioManager = GameObject.FindWithTag("AudioManager");
        audioManager.GetComponent<PlayerAudioManager>().impale();
        yield return new WaitForSeconds(0.5f);
        audioManager.GetComponent<PlayerAudioManager>().respawn();
        yield return new WaitForSeconds(0.8f);
        player.GetComponent<PlayerMovementBruce>().RespawnCall();
    }
}
