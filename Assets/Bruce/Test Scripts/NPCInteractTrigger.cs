using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject tooltip;
    
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool inDetectRange;
    public GameObject player;

    void Start()
    {
        tooltip.SetActive(false);
        inDetectRange = false;
        player = null;
    }

    void Update()
    {
        PlayerInteract();
        if (player != null) {
            if (!player.activeSelf) {
                DisableInteract();
            }
        }
    }

    void PlayerInteract()
    {
        if(inDetectRange && !GlobalDialogueSystem.GetInstance().dialogueIsPlaying && Input.GetButtonDown("Confirm"))
        {
            //Debug.Log("Confirm Key Pressed");
            GlobalDialogueSystem.GetInstance().EnterDialogueMode(inkJSON);
        }
    }

    void OnTriggerEnter (Collider other)
    {
        if(other.tag == "Player")
        {   
            //Debug.Log("Enter Hitbox");
            player = other.gameObject;
            if (!player.GetComponent<PlayerController>().playerInvincible) {
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
