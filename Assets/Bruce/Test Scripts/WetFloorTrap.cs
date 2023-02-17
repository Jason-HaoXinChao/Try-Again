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
        Debug.Log(other);
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
        Instantiate(deadPlayer, player.GetComponent<Transform>().position, player.GetComponent<Transform>().rotation);
        player.gameObject.SetActive(false);

        StartCoroutine(RespawnTimer());
    }

    IEnumerator RespawnTimer()
    {
        yield return new WaitForSeconds(0.1f);
        player.GetComponent<PlayerMovementBruce>().RespawnCall();
    }
}
