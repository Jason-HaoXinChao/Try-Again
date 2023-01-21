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

    private void Awake()
    {   
        renderer = GetComponent<Renderer>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Spike")
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

        yield return new WaitForSeconds(0.01f);

        transform.position = respawnPoint.position;
        transform.rotation = Quaternion.identity;

        renderer.enabled = true;
        isDead = false;
    }
}
