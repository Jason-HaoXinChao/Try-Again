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
            other.transform.parent.gameObject.GetComponent<PlayerMovementBruce>().SetRespawnPoint(spawnPoint.transform);
            Debug.Log("checkpoint reached");
            // disable the checkpoint once it's been reached.
            this.gameObject.SetActive(false);
        } else {
            Debug.Log(other);
        }
        
    }

}
