using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1_Quest : MonoBehaviour
{
    [SerializeField] private GameObject questStatus;
    private bool oneTime = true;

    void Update()
    {
        if(oneTime && !this.gameObject.GetComponent<Level1_NPCDialogue>().firstContact && !GlobalDialogueSystem.GetInstance().dialogueIsPlaying)
        {
            questStatus.SetActive(true);
            oneTime = false;
        }
    }
}
