using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItemScript : MonoBehaviour
{
    
    private bool inPickUpRange;
    public GameObject tooltip;
    public string itemName;

    // Start is called before the first frame update
    void Start()
    {
        inPickUpRange = false;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPickUp();
    }

    void PlayerPickUp()
    {
        if(inPickUpRange && Input.GetButtonDown("Confirm"))
        {
            //Debug.Log("Confirm Key Pressed");
            GameObject playerInventory = GameObject.Find("Inventory");
            playerInventory.GetComponent<PlayerInventory>().collectItem(itemName);

            transform.parent.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter (Collider other)
    {
        if(other.tag == "Player")
        {
            //Debug.Log("Enter Hitbox");
            tooltip.SetActive(true);
            inPickUpRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            //Debug.Log("Leave Hitbox");
            tooltip.SetActive(false);
            inPickUpRange = false;
        }
    }
}
