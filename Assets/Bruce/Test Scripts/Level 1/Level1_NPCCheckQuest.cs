using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1_NPCCheckQuest : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject tooltip;
    
    [Header("Ink JSON")]
    [SerializeField] private TextAsset successJSON;
    [SerializeField] private TextAsset failJSON;
    [SerializeField] private TextAsset afterIdleJSON;

    public bool inDetectRange { get; private set; }
    public bool questComplete { get; private set; }
    public GameObject player;
    GameObject playerInventory;
    [SerializeField] GameObject questLog;

    void Start()
    {
        tooltip.SetActive(false);
        inDetectRange = false;
        questComplete = false;
        player = null;
        playerInventory = GameObject.Find("Inventory");
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

            if(playerInventory.GetComponent<PlayerInventory>().collectedItems.Contains("coffee") && !questComplete)
            {
                GlobalDialogueSystem.GetInstance().EnterDialogueMode(successJSON);
                playerInventory.GetComponent<PlayerInventory>().consumeItem("coffee");

                int currProgress = questLog.GetComponent<UnityEngine.UI.Text>().text[17] - '0';
                questLog.GetComponent<UnityEngine.UI.Text>().text = $"Deliver Coffee : {currProgress + 1}/3";

                questComplete = true;
            }
            else if(questComplete)
            {
                GlobalDialogueSystem.GetInstance().EnterDialogueMode(afterIdleJSON);
            }
            else
            {
                GlobalDialogueSystem.GetInstance().EnterDialogueMode(failJSON);
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

    void DisableInteract() {
        player = null;
        tooltip.SetActive(false);
        inDetectRange = false;
    }
}
