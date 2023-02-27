using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    private GameObject player;
    public AK.Wwise.Event getImpaled;
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
                // Debug.Log("object in hitbox" + other.gameObject.name);
                // Debug.Log("player" + position);
                // Debug.Log("local" + other.transform.position);
                getImpaled.Post(gameObject);
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
        yield return new WaitForSeconds(1.3f);
        player.GetComponent<PlayerMovementBruce>().RespawnCall();
    }
}
