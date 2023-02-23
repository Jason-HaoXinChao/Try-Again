using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    private GameObject pressKeyToInteract;
    
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool inDetectRange;

    void Awake()
    {
        pressKeyToInteract = this.gameObject.transform.parent.parent.GetChild(1).gameObject;
        pressKeyToInteract.SetActive(false);
        inDetectRange = false;
    }

    void Update()
    {
        HoverIconUpdate();
        PlayerInteract();
    }

    void HoverIconUpdate()
    {
        Vector3 offset = new Vector3 (0f, 1f, 0f);
        pressKeyToInteract.GetComponent<Transform>().position = 
            this.gameObject.transform.parent.position + offset;
    }

    void PlayerInteract()
    {
        if(inDetectRange && !GlobalDialogueSystem.GetInstance().dialogueIsPlaying && Input.GetButtonDown("Confirm"))
        {
            GlobalDialogueSystem.GetInstance().EnterDialogueMode(inkJSON);
        }
    }

    void OnTriggerEnter (Collider other)
    {
        if(other.tag == "Player")
        {   
            GameObject player = other.gameObject;
            if (!player.GetComponent<PlayerMovementBruce>().playerInvincible) {
                pressKeyToInteract.SetActive(true);
                inDetectRange = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            pressKeyToInteract.SetActive(false);
            inDetectRange = false;
        }
    }
}
