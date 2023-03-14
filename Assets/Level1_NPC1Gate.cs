using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1_NPC1Gate : MonoBehaviour
{
    [SerializeField] GameObject gate;
    private bool oneTime = true;

    void Update()
    {
        if(oneTime && this.gameObject.GetComponent<Level1_NPCCheckQuest>().questComplete && !GlobalDialogueSystem.GetInstance().dialogueIsPlaying)
        {
            gate.SetActive(false);
            oneTime = false;
        }
    }
}
