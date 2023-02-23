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
        // Debug.Log(other);
        if(other.name == "leg") // checks if the player's feet is on the floor
        {
            if(!delay)
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
