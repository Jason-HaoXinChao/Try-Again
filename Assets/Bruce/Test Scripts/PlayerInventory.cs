using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private List<GameObject> itemList;
    [SerializeField] private List<GameObject> itemSpriteHUD;
    [SerializeField] private List<Sprite> itemSprites;

    public string itemInHand;   // physical items
    public List<string> collectedItems;   // non-physical items

    private GameObject player;

    void Start()
    {
        itemInHand = String.Empty;
        player = GameObject.Find("BlockPlayer");
    }

    void Update()
    {
        if(!String.IsNullOrEmpty(itemInHand))
        {
            if(Input.GetButtonDown("Drop"))
            {
                for (int i = 0; i < itemList.Count; i++) 
                {
                    if(itemInHand == itemList[i].name)
                    {
                        Instantiate(
                            itemList[i], 
                            player.GetComponent<Transform>().position + new Vector3(0f, 4f, 0f), 
                            Quaternion.identity
                            );
                        itemInHand = String.Empty;
                        break;
                    }
                } 
            }
        }
    }

    // void Awake()
    // {
    //     DontDestroyOnLoad(this.gameObject);
    // }

    public void collectItem(string itemName)
    {
        // Debug.Log("collecting " + itemName);
        this.collectedItems.Add(itemName);
        // Debug.Log("added " + itemName);
        updateHUD();
    }

    public void consumeItem(string itemName)
    {
        // Debug.Log("using " + itemName);
        this.collectedItems.Remove(itemName);
        // Debug.Log("removed " + itemName);
        updateHUD();
    }

    private void updateHUD()
    {
        int index = 0;
        foreach (string item in this.collectedItems) {
            // Debug.Log(item);
            foreach (Sprite itemSprite in this.itemSprites) {
                // Debug.Log(itemSprite.name);
                if (itemSprite.name == item) {
                    this.itemSpriteHUD[index].SetActive(true);
                    this.itemSpriteHUD[index].GetComponent<Image>().sprite = itemSprite;
                    index++;
                }
            }
        }

        while (index < this.itemSpriteHUD.Count) {
            this.itemSpriteHUD[index].SetActive(false);
            index++;
        }
    }
}
