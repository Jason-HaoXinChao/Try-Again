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
    private Vector3 pressurePlateOrgin;

    void Start()
    {
        fireModel.SetActive(true);
        fireHitbox.SetActive(true);
        smokeModel.SetActive(false);

        pressurePlateOrgin = pressurePlateSystem.GetComponent<Transform>().position;
    }

    void OnTriggerStay (Collider other)
    {
        if(!triggerLockEnter)
        {
            fireModel.SetActive(false);
            fireHitbox.SetActive(false);
            smokeModel.SetActive(true);
            pressurePlateSystem.GetComponent<Transform>().position = pressurePlateOrgin - new Vector3(0,0.25f,0);

            StartCoroutine(SetTriggerLockEnter());
        }
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
        if(!triggerLockExit)
        {
            fireModel.SetActive(true);
            fireHitbox.SetActive(true);
            smokeModel.SetActive(false);
            pressurePlateSystem.GetComponent<Transform>().position = pressurePlateOrgin;
            
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
}
