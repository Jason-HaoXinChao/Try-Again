using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemScript : MonoBehaviour
{   
    [SerializeField] private int itemCount;
    public GameObject item;
    private bool canPickUp;
    private GameObject pickedUpItem;
    public TextMeshProUGUI itemCountText;
    public TextMeshProUGUI pickUpItemText;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {   PickUpItem();
        DropItem();
        UpdateItemCountUI();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Item")
        {
            Debug.Log("can pick up");
            canPickUp = true;
            pickedUpItem = other.gameObject.transform.parent.gameObject;
            pickUpItemText.enabled = true;
            pickUpItemText.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {   
        if (other.transform.tag == "Item") {
            canPickUp = false;
            pickUpItemText.enabled = false;
            pickUpItemText.gameObject.SetActive(false);
        }
    }

    void PickUpItem() {
        if (pickedUpItem != null && canPickUp && Input.GetButtonDown("Fire1")) {
            Destroy(pickedUpItem);
            itemCount += 1;

            pickUpItemText.enabled = false;
            pickUpItemText.gameObject.SetActive(false);
        }
    }

    void DropItem() {
        if (itemCount > 0 && Input.GetButtonDown("Fire2")) {
            itemCount -= 1;
            Debug.Log("drop item");
            Vector3 offset = new Vector3(5f, 0f, 0f); // change to where player is facing

            Instantiate(item, this.gameObject.transform.position + offset, Quaternion.identity);
        }
    }

    void UpdateItemCountUI() {
        itemCountText.text = "Item Count: " + itemCount;
    }
}
