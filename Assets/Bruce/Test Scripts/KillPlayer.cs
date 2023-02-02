using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private GameObject deadPlayer;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void OnTriggerEnter (Collider other)
    {
        if(other.transform == player.GetComponent<Transform>())
        {
            Instantiate(deadPlayer, other.GetComponent<Transform>().position, other.GetComponent<Transform>().rotation);
            other.gameObject.SetActive(false);

            StartCoroutine(RespawnTimer());
        }
    }

    IEnumerator RespawnTimer()
    {
        yield return new WaitForSeconds(1);
        player.GetComponent<PlayerMovementBruce>().RespawnCall();
    }
}
