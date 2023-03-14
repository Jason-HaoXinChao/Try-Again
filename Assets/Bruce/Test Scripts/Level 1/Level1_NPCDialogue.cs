using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1_NPCDialogue : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject tooltip;
    
    [Header("Ink JSON")]
    [SerializeField] private TextAsset beforeJSON;
    [SerializeField] private TextAsset afterJSON;

    public bool inDetectRange { get; private set; }
    public bool firstContact { get; private set; }
    public GameObject player;

    void Start()
    {
        tooltip.SetActive(false);
        inDetectRange = false;
        firstContact = true;
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
            player.GetComponent<PlayerController>().FaceNPC();

            if(firstContact)
            {
                GlobalDialogueSystem.GetInstance().EnterDialogueMode(beforeJSON);
                firstContact = false;
            }
            else
            {
                GlobalDialogueSystem.GetInstance().EnterDialogueMode(afterJSON);
            }
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
