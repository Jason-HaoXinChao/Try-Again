using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorScript : MonoBehaviour
{

    private bool inOpenRange;
    public GameObject tooltip;
    public string keycardName;
    public AK.Wwise.Event doorSounds;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        inOpenRange = false;
        tooltip.SetActive(false);
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
                doorSounds.Post(gameObject);
            }
        }
    }

    void OnTriggerEnter (Collider other)
    {   
        if (other.tag == "Player")
        {
            GameObject playerObj = other.gameObject;
            if (playerObj.activeSelf && !playerObj.GetComponent<PlayerMovementBruce>().playerInvincible) {
                player = playerObj;
                tooltip.SetActive(true);
                inOpenRange = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log(other);
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
