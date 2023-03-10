using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItemScript : MonoBehaviour
{
    
    private bool inPickUpRange;
    public GameObject tooltip;
    public string itemName;
    public GameObject player;
    public AK.Wwise.Event pickUpCard;
    

    // Start is called before the first frame update
    void Start()
    {
        inPickUpRange = false;
        player = null;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPickUp();
        if (player != null) {
            if (!player.activeSelf) {
                DisablePickup();
            }
        }
    }

    void PlayerPickUp()
    {
        if(inPickUpRange && Input.GetButtonDown("Confirm"))
        {
            // Debug.Log("Confirm Key Pressed");
            GameObject playerInventory = GameObject.Find("Inventory");
            playerInventory.GetComponent<PlayerInventory>().collectItem(itemName);

            transform.parent.gameObject.SetActive(false);
            pickUpCard.Post(gameObject);
        }
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.tag == "Player" && !GameObject.Find("BlockPlayer").GetComponent<PlayerController>().playerInvincible)
        {  
            tooltip.SetActive(true);
            inPickUpRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            this.DisablePickup();
        }
    }

    private void DisablePickup() {
        player = null;
        tooltip.SetActive(false);
        inPickUpRange = false;
    }
}
