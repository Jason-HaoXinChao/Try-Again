using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private List<GameObject> itemList;
    [SerializeField] private List<GameObject> itemSpriteHUD;

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

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void collectItem(string itemName)
    {
        this.collectedItems.Add(itemName);
    }
}
