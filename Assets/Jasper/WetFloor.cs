using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WetFloor : MonoBehaviour
{
    private GameObject player;
    private GameObject slidingPlayer;
    [SerializeField] private GameObject deadPlayer;
    private bool sliding = false;
    float speed = 0.002f;
    private int segmentCount = 0;

    void Start()
    {
        player = GameObject.Find("BlockPlayer");
    }

    void Update()
    {
        if(sliding)
        {
            slidingPlayer.transform.position = Vector3.Lerp(slidingPlayer.transform.position, transform.forward * speed * Time.deltaTime, speed);
            player.transform.position = Vector3.Lerp(slidingPlayer.transform.position, transform.forward * speed * Time.deltaTime, speed);
        }
    }

    void OnTriggerEnter (Collider other)
    {
        if(other.transform == player.GetComponent<Transform>())
        {
            slidingPlayer = Instantiate(deadPlayer, other.GetComponent<Transform>().position, deadPlayer.GetComponent<Transform>().rotation);

            other.gameObject.SetActive(false);
            sliding = true;
        }
    }

    void OnTriggerExit (Collider other) 
    {   
        segmentCount += 1;

        if (segmentCount == 3) {
            sliding = false;
            segmentCount = 0;
            StartCoroutine(RespawnTimer());
        }
    }

    IEnumerator RespawnTimer()
    {
        GameObject audioManager = GameObject.FindWithTag("AudioManager");
        // audioManager.GetComponent<PlayerAudioManager>().impale();
        yield return new WaitForSeconds(0.25f);
        audioManager.GetComponent<PlayerAudioManager>().respawn();
        yield return new WaitForSeconds(0.8f);
        player.GetComponent<PlayerMovementBruce>().RespawnCall();
    }
}
