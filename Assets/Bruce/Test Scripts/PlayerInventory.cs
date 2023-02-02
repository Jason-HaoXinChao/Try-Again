using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> itemList;

    public string itemInHand;

    private GameObject player;

    void Start()
    {
        itemInHand = String.Empty;
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if(!String.IsNullOrEmpty(itemInHand))
        {
            if(Input.GetButtonDown("Fire2"))
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
}
