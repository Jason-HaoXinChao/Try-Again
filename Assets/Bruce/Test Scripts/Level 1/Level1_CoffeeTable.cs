using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1_CoffeeTable : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject tooltip;

    private bool inDetectRange;
    public string itemName;
    public GameObject player;

    [Header("Quest Giver")]
    [SerializeField] private GameObject npc;
    private bool inNPCRange;

    [Header("Quest Log")]
    [SerializeField] private GameObject questStatus;

    void Start()
    {
        tooltip.SetActive(false);
        inDetectRange = false;
        inNPCRange = false;
        player = null;
    }

    void Update()
    {
        if(!questStatus.activeSelf)
        {
            tooltip.SetActive(false);
        }
        InNPCRange();
        PlayerInteract();
        if (player != null) {
            if (!player.activeSelf) {
                DisableInteract();
            }
        }
    }

    void InNPCRange()
    {
        if(npc.GetComponent<Level1_NPCDialogue>().inDetectRange)
        {
            inNPCRange = true;
            if(inDetectRange || !questStatus.activeSelf)
            {
                tooltip.SetActive(false);
            }
        }
        else
        {
            inNPCRange = false;
            if(inDetectRange && questStatus.activeSelf)
            {
                tooltip.SetActive(true);
            }
        }
    }

    void PlayerInteract()
    {
        if(inDetectRange && !GlobalDialogueSystem.GetInstance().dialogueIsPlaying && Input.GetButtonDown("Confirm") && !inNPCRange)
        {
            player.GetComponent<PlayerController>().FaceNPC();

            // Pick up coffee
            GameObject playerInventory = GameObject.Find("Inventory");
            if(playerInventory.GetComponent<PlayerInventory>().collectedItems.Count == 0)
            {
                playerInventory.GetComponent<PlayerInventory>().collectItem(itemName);
            }
            
        }
    }

    void OnTriggerEnter (Collider other)
    {
        if(other.tag == "Player")
        {   
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
            DisableInteract();
        }
    }

    void DisableInteract() 
    {
        player = null;
        tooltip.SetActive(false);
        inDetectRange = false;
    }
}
