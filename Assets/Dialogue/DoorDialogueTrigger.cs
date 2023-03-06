using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDialogueTrigger : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool inDetectRange;

    void Awake()
    {
        inDetectRange = false;
    }

    void Update()
    {
        PlayerInteract();
    }

    void PlayerInteract()
    {   
        if(inDetectRange && !GlobalDialogueSystem.GetInstance().dialogueIsPlaying && Input.GetButtonDown("Confirm"))
        {   
            GameObject playerInventory = GameObject.Find("Inventory");
            string keycardName = this.GetComponent<OpenDoorScript>().keycardName;

            if (!playerInventory.GetComponent<PlayerInventory>().collectedItems.Contains(keycardName)) {
                this.GetComponent<OpenDoorScript>().tooltip.SetActive(false);
                // Debug.Log("Confirm Key Pressed");
                GlobalDialogueSystem.GetInstance().EnterDialogueMode(inkJSON);
            }
        }
    }

    void OnTriggerEnter (Collider other)
    {
        if(other.tag == "Player" && !other.gameObject.GetComponent<PlayerController>().playerInvincible)
        {
            //Debug.Log("Enter Hitbox");
            inDetectRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            //Debug.Log("Leave Hitbox");
            inDetectRange = false;
        }
    }

    void OnTriggerStay(Collider other) {
        if(other.tag == "Player" && !other.gameObject.GetComponent<PlayerController>().playerInvincible)
        {
            if (!GlobalDialogueSystem.GetInstance().dialogueIsPlaying && !this.GetComponent<OpenDoorScript>().tooltip.activeSelf) {
                this.GetComponent<OpenDoorScript>().tooltip.SetActive(true);
            }
        }
    }
}
