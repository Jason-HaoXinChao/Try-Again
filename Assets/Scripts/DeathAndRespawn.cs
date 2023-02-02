using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathAndRespawn : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    private Renderer renderer;
    public bool isDead = false;
    public GameObject player;
    public GameObject corpse;
    public AudioSource impaleSound;
    public AudioSource spawnSound;
    public AudioSource[] deathSounds;

    private void Awake()
    {   
        renderer = GetComponent<Renderer>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Spike" && !isDead)
        {
            isDead = true;
            StartCoroutine(DieAndRespawn());
            Debug.Log("touched spike");
        }
    }

    private IEnumerator DieAndRespawn()
    {
        renderer.enabled = false;
        Instantiate(corpse, player.transform.position, Quaternion.identity);
        impaleSound.Play();
        yield return new WaitForSeconds(0.5f);

        deathSounds[Random.Range(0, 4)].Play();

        yield return new WaitForSeconds(2.5f);

        spawnSound.Play();

        transform.position = respawnPoint.position;
        transform.rotation = Quaternion.identity;

        renderer.enabled = true;
        isDead = false;
    }

    public void SetRespawnPoint(Transform newLocation)
    {
        this.respawnPoint = newLocation;
    }
}
