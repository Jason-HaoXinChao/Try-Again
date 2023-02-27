using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WetFloorTrap : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private GameObject deadPlayer;
    private bool delay;

    void Start()
    {
        player = GameObject.Find("BlockPlayer");
    }
    
    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject.name == "PlayerHitbox") // checks for player's hitbox
        {
            if(!delay && !player.GetComponent<PlayerMovementBruce>().playerInvincible)
            {
                player.GetComponent<PlayerMovementBruce>().WetFloor();
                StartCoroutine(TrapDelay());
            }
        }
    }

    IEnumerator TrapDelay()
    {
        delay = true;
        yield return new WaitForSeconds(3);
        delay = false;
    }


    public void SpawnDeadBody()
    {
        Vector3 position = player.transform.position;
        Quaternion rotation = player.transform.rotation;

        if (rotation.y != 0) {
            rotation.z = 180f;
            rotation.x = 180f;
        }
        

        rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        player.gameObject.SetActive(false);
        Instantiate(deadPlayer, position, rotation);

        StartCoroutine(RespawnTimer());
    }

    IEnumerator RespawnTimer()
    {
        yield return new WaitForSeconds(0.1f);
        player.GetComponent<PlayerMovementBruce>().RespawnCall();
    }
}
