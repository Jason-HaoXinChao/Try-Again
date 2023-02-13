using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WetFloor : MonoBehaviour
{
    private GameObject player;
    private GameObject slidingPlayer;
    [SerializeField] private GameObject deadPlayer;

    private bool sliding = false;
    private float speed = 0.05f;
    private int segmentCount = 0;
    private Vector3 direction = new Vector3(100, 0, 0);
    private Quaternion bodyRotation;

    void Start()
    {
        player = GameObject.Find("BlockPlayer");
        bodyRotation = deadPlayer.GetComponent<Transform>().rotation;
    }

    void Update()
    {   
        if(sliding)
        {   
            slidingPlayer.transform.position = Vector3.Lerp(slidingPlayer.transform.position, slidingPlayer.transform.position + direction, speed * Time.deltaTime);
            player.transform.position = Vector3.Lerp(slidingPlayer.transform.position,slidingPlayer.transform.position + direction, speed * Time.deltaTime);
        } else {
            if (Input.GetAxisRaw("Horizontal") == -1) {
                direction *= -1;
                bodyRotation = Quaternion.Inverse(deadPlayer.GetComponent<Transform>().rotation);
            } else if (Input.GetAxisRaw("Horizontal") == 1) {
                direction *= -1;
                bodyRotation = deadPlayer.GetComponent<Transform>().rotation;
            }
        }
    }

    void OnTriggerEnter (Collider other)
    {
        if(other.transform == player.GetComponent<Transform>())
        {
            slidingPlayer = Instantiate(deadPlayer, other.GetComponent<Transform>().position, bodyRotation);

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
