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
            getImpaled.Post(gameObject);
            Instantiate(deadPlayer, other.GetComponent<Transform>().position, other.GetComponent<Transform>().rotation);
            other.gameObject.SetActive(false);

            StartCoroutine(RespawnTimer());
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
