using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompPlayer : MonoBehaviour
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
            if (!player.GetComponent<PlayerController>().playerInvincible) {

                Vector3 position = other.gameObject.transform.position;
                Quaternion rotation = other.gameObject.transform.rotation;
                other.gameObject.SetActive(false);
                Instantiate(deadPlayer, position, rotation);
            
                StartCoroutine(RespawnTimer());
            } else {
                player.GetComponent<PlayerController>().RemoveHorizontalInertia();
            }

        }
    }

    IEnumerator RespawnTimer()
    {
        player.GetComponent<PlayerController>().DropRagdollBody();
        yield return new WaitForSeconds(1.0f);
        player.GetComponent<PlayerController>().RespawnCall();
    }
}
