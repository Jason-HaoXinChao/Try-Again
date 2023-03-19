using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateFire : MonoBehaviour
{
    [SerializeField] private GameObject fireModel;
    [SerializeField] private GameObject fireHitbox;
    [SerializeField] private GameObject smokeModel;
    [SerializeField] private GameObject pressurePlateSystem;
    [SerializeField] private GameObject sprinklerModel;

    private bool triggerLockEnter, triggerLockExit;
    private Vector3 pressurePlateOrgin, pressurePlateSinked;
    private List<GameObject> objectsInRange;
    private bool fireOn;
    private GameObject player;
    public AK.Wwise.Event ButtonOn;
    public AK.Wwise.Event ButtonOff;

    void Start()
    {
        objectsInRange = new List<GameObject>();
        updateFire(true);
        pressurePlateOrgin = pressurePlateSystem.GetComponent<Transform>().position;
        pressurePlateSinked = pressurePlateOrgin - new Vector3(0,0.25f,0);
        player = GameObject.Find("BlockPlayer");
    }

    void Update()
    {
        if (objectsInRange.Count > 0) {
            // update objects in hitbox
            List<GameObject> updated = new List<GameObject>();
            foreach (GameObject item in objectsInRange)
            {
                if (item != null) {
                    if (item.activeSelf) {
                        updated.Add(item);
                    }
                }
            }
            objectsInRange = updated;
        }
        if (!fireOn) {
            pressurePlateSystem.GetComponent<Transform>().position = Vector3.MoveTowards(pressurePlateSystem.GetComponent<Transform>().position, pressurePlateSinked, 0.005f);
            if (objectsInRange.Count == 0) {
                if (pressurePlateSystem.GetComponent<Transform>().position == pressurePlateSinked) {
                    updateFire(true);
                }
            }
        } else {
            pressurePlateSystem.GetComponent<Transform>().position = Vector3.MoveTowards(pressurePlateSystem.GetComponent<Transform>().position, pressurePlateOrgin, 0.005f);
        }
        
    }

    void OnTriggerStay (Collider other)
    {
        if (!triggerLockEnter)
        {
            GameObject instance = other.gameObject;
            if (!objectsInRange.Contains(instance) && other.gameObject.name != "PickUpBodyHitbox") {
                objectsInRange.Add(instance);
                updateFire(false);
                // if the object entering hitbox is player's sliding body then it should be removed once player respawn.
                if (player.GetComponent<PlayerController>().playerInvincible && instance.name == "PlayerHitbox") {
                    StartCoroutine(RemoveObjectInHitbox(instance));
                }
            }

            StartCoroutine(SetTriggerLockEnter());
        }
    }

    // turn the fire on or off
    private void updateFire(bool on) {
        fireModel.SetActive(on);
        fireHitbox.SetActive(on);
        smokeModel.SetActive(!on);
        sprinklerModel.SetActive(!on);
        fireOn = on;
    }

    void OnTriggerExit (Collider other)
    {
        if (!triggerLockExit)
        {
            GameObject instance = other.gameObject;
            if (objectsInRange.Contains(instance)) {
                objectsInRange.Remove(instance);
                if (objectsInRange.Count == 0) {
                    updateFire(true);
                }
            }
            
            StartCoroutine(SetTriggerLockExit());
        }
    }


    IEnumerator SetTriggerLockEnter()
    {
        triggerLockEnter = true;
        yield return new WaitForSeconds(0.3f);
        triggerLockEnter = false;
    }

    IEnumerator SetTriggerLockExit()
    {
        triggerLockExit = true;
        yield return new WaitForSeconds(0.3f);
        triggerLockExit = false;
    }

    // remove the target object from object in list
    IEnumerator RemoveObjectInHitbox(GameObject target)
    {
        yield return new WaitForSeconds(1f);
        objectsInRange.Remove(target);
    }
}
