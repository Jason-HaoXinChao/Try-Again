using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorScript : MonoBehaviour
{

    private bool inOpenRange;
    public GameObject tooltip;
    public string keycardName;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        inOpenRange = false;
        player = null;
    }

    // Update is called once per frame
    void Update()
    {
        OpenDoor();
        if (player != null) {
            if (!player.activeSelf) {
                DisableOpen();
            }
        }
    }

    void OpenDoor()
    {
        if(inOpenRange && Input.GetButtonDown("Confirm"))
        {
            // Debug.Log("Confirm Key Pressed");
            GameObject playerInventory = GameObject.Find("Inventory");
            // Debug.Log(keycardName);
            if (playerInventory.GetComponent<PlayerInventory>().collectedItems.Contains(keycardName)) {
                // Debug.Log("Consuming item");
                playerInventory.GetComponent<PlayerInventory>().consumeItem(keycardName);
                transform.parent.gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerEnter (Collider other)
    {   
        if(other.tag == "Player" && player.activeSelf && !player.GetComponent<PlayerMovementBruce>().wetfloorOverride)
        {
            // Debug.Log("Enter Hitbox");
            player = other.gameObject;
            tooltip.SetActive(true);
            GameObject playerInventory = GameObject.Find("Inventory");
            if (playerInventory.GetComponent<PlayerInventory>().collectedItems.Contains(keycardName)) {
                tooltip.GetComponent<TMPro.TextMeshProUGUI>().text = "Press F to use keycard";
            } else {
                tooltip.GetComponent<TMPro.TextMeshProUGUI>().text = "You need a keycard to open this door";
            }
            inOpenRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //Debug.Log("Leave Hitbox");
            DisableOpen();
        }
    }

    private void DisableOpen() {
        player = null;
        tooltip.SetActive(false);
        inOpenRange = false;
    }
}
