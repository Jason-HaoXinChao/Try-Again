using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInRange : MonoBehaviour
{ 
    [SerializeField]
    private string thisItem;
    
    private GameObject pressKeyToPickUp;
    private bool inPickUpRange;

    void Start()
    {
        pressKeyToPickUp = this.gameObject.transform.parent.parent.GetChild(1).gameObject;
        inPickUpRange = false;
    }

    void Update()
    {
        HoverIconUpdate();
        PlayerPickUp();
    }

    void HoverIconUpdate()
    {
        Vector3 offset = new Vector3 (0f, 2f, 0f);
        pressKeyToPickUp.GetComponent<Transform>().position = 
            this.gameObject.transform.parent.position + offset;
    }

    void PlayerPickUp()
    {
        if(inPickUpRange && Input.GetButtonDown("Fire1"))
        {
            GameObject playerInventory = GameObject.Find("Inventory");
            playerInventory.GetComponent<PlayerInventory>().itemInHand = thisItem;

            Destroy(this.gameObject.transform.parent.parent.gameObject);
        }
    }

    void OnTriggerEnter (Collider other)
    {
        if(other.transform.tag == "Player")
        {
            pressKeyToPickUp.SetActive(true);
            inPickUpRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            pressKeyToPickUp.SetActive(false);
            inPickUpRange = false;
        }
    }
}
