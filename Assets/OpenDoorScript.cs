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
            GameObject playerInventory = GameObject.Find("Inventory");
            if (playerInventory.GetComponent<PlayerInventory>().collectedItems.Contains(keycardName)) {
                playerInventory.GetComponent<PlayerInventory>().consumeItem(keycardName);
                transform.parent.gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerEnter (Collider other)
    {   
        if (other.tag == "Player" && !GameObject.Find("BlockPlayer").GetComponent<PlayerMovementBruce>().playerInvincible)
        {
            tooltip.SetActive(true);
            inOpenRange = true;

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            DisableOpen();
        }
    }

    private void DisableOpen() {
        player = null;
        tooltip.SetActive(false);
        inOpenRange = false;
    }
}
