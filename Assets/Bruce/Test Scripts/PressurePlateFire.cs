using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateFire : MonoBehaviour
{
    [SerializeField] private GameObject fireModel;
    [SerializeField] private GameObject fireHitbox;
    [SerializeField] private GameObject smokeModel;
    [SerializeField] private GameObject pressurePlateSystem;

    private bool triggerLockEnter, triggerLockExit;
    private Vector3 pressurePlateOrgin, pressurePlateSinked;
    private List<int> objectsInRange;
    private bool fireOn;
    public AK.Wwise.Event extinguish;

    void Start()
    {
        objectsInRange = new List<int>();
        updateFire(true);
        pressurePlateOrgin = pressurePlateSystem.GetComponent<Transform>().position;
        pressurePlateSinked = pressurePlateOrgin - new Vector3(0,0.25f,0);
    }

    void Update()
    {
        if (!fireOn) {
            pressurePlateSystem.GetComponent<Transform>().position = Vector3.MoveTowards(pressurePlateSystem.GetComponent<Transform>().position, pressurePlateSinked, 0.01f);
        } else {
            pressurePlateSystem.GetComponent<Transform>().position = Vector3.MoveTowards(pressurePlateSystem.GetComponent<Transform>().position, pressurePlateOrgin, 0.01f);
        }
    }

    void OnTriggerStay (Collider other)
    {
        if (!triggerLockEnter)
        {
            int count = objectsInRange.Count;
            int id = other.gameObject.GetInstanceID();
            if (!objectsInRange.Contains(id)) {
                objectsInRange.Add(id);
                updateFire(false);
            }

            StartCoroutine(SetTriggerLockEnter());
        }
    }

    // turn the fire on or off
    private void updateFire(bool on) {
        fireModel.SetActive(on);
        fireHitbox.SetActive(on);
        smokeModel.SetActive(!on);
        fireOn = on;
    }

    // void OnTriggerEnter (Collider other)
    // {
    //     if(!triggerLockEnter)
    //     {
    //         fireModel.SetActive(false);
    //         fireHitbox.SetActive(false);
    //         smokeModel.SetActive(true);
    //         pressurePlateSystem.GetComponent<Transform>().position = pressurePlateOrgin - new Vector3(0,0.25f,0);

    //         StartCoroutine(SetTriggerLockEnter());
    //     }
    // }

    void OnTriggerExit (Collider other)
    {
        if (!triggerLockExit)
        {
            int id = other.gameObject.GetInstanceID();
            int index = objectsInRange.IndexOf(id);
            if (index >= 0) {
                objectsInRange.RemoveAt(index);
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
        yield return new WaitForSeconds(0.23f);
        triggerLockExit = false;
    }
}
