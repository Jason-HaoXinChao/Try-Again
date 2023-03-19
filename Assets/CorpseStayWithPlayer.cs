using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseStayWithPlayer : MonoBehaviour
{
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("BlockPlayer");   
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = player.transform.position;
        position.y = position.y + 1.0f;
        
        this.transform.position = position;
        Quaternion playerRotation = player.gameObject.transform.rotation;
        if (playerRotation.y == 0) {
            this.transform.rotation = Quaternion.Euler(0, 90, -90);
        } else {
            this.transform.rotation = Quaternion.Euler(0, -90, -90);
        }
    }
}
