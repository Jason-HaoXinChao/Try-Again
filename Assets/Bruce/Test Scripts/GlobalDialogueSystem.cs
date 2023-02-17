using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;

public class GlobalDialogueSystem : MonoBehaviour
{
    static GlobalDialogueSystem instance;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject characterProfile;
    [SerializeField] private GameObject characterIcon;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("[WARNING] More than one instance of GlobalDialogueSystem in the scene");
        }
        instance = this;
    }

    public static GlobalDialogueSystem GetInstance()
    {
        return instance;
    }

    void Start()
    {
        dialogueIsPlaying = false;
        characterProfile.SetActive(false);
        dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if(!dialogueIsPlaying){return;}

        if(Input.GetButtonUp("Confirm"))
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        characterProfile.SetActive(true);
        dialoguePanel.SetActive(true);
    }

    void ContinueStory()
    {
        if(currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
            List<string> tags = currentStory.currentTags;

            characterIcon.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Dialogue UI/Speaker Icon/{tags[0]}");
        }
        else
        {
            ExitDialogueMode();
        }
    }

    void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        characterProfile.SetActive(false);
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }
}
