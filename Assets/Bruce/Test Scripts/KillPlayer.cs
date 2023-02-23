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

                Vector3 position = other.gameObject.transform.position;
                Quaternion rotation = other.gameObject.transform.rotation;
                
                other.gameObject.SetActive(false);
                Instantiate(deadPlayer, position, rotation);
                
                

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
