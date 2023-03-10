using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnActivation : MonoBehaviour
{
    [SerializeField] public GameObject spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            if (!other.gameObject.GetComponent<PlayerController>().playerInvincible) {
                other.gameObject.GetComponent<PlayerController>().SetRespawnPoint(spawnPoint.transform);
            }
        }
    }

}
