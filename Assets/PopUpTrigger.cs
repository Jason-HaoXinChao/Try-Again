using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpTrigger : MonoBehaviour
{   
    [SerializeField] private GameObject tooltip;
    private bool inDetectRange;
    public GameObject player;

    void Start()
    {
        tooltip.SetActive(false);
        inDetectRange = false;
        player = null;
    }
    
    void OnTriggerEnter (Collider other)
    {
        if(other.tag == "Player")
        {   
            //Debug.Log("Enter Hitbox");
            player = other.gameObject;
            if (!player.GetComponent<PlayerMovementBruce>().playerInvincible) {
                tooltip.SetActive(true);
                inDetectRange = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            //Debug.Log("Leave Hitbox");
            DisableInteract();
        }
    }

    void DisableInteract() {
        player = null;
        tooltip.SetActive(false);
        inDetectRange = false;
    }
}
