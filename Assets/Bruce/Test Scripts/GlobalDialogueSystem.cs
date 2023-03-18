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
    private bool firstLine;
    public AK.Wwise.Event colleagueVoice;
    public AK.Wwise.Event colleagueVoiceEnd;
    public AK.Wwise.Event playerVoice;
    public AK.Wwise.Event playerVoiceEnd;

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
        firstLine = false;
    }

    void Update()
    {
        if(!dialogueIsPlaying){return;}

        if(Input.GetButtonUp("Confirm"))
        {
            if(firstLine){firstLine = false;}
            else{EndPreviousDialogueVoice();}
            
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        characterProfile.SetActive(true);
        dialoguePanel.SetActive(true);
        firstLine = true;
    }

    void ContinueStory()
    {
        if(currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
            List<string> tags = currentStory.currentTags;

            DialogueRNGVoice();

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

    void DialogueRNGVoice()
    {
        float dialogueLength = dialogueText.text.Length * 0.2f; //Change the number to modify length of speech
        Debug.Log(dialogueLength);

        string dialogueSpeaker = currentStory.currentTags[0].Split('_')[0];
        Debug.Log(dialogueSpeaker);
        if (dialogueSpeaker == "Player")
        {
            playerVoice.Post(gameObject);
        }
        else if (dialogueSpeaker == "Colleague")
        {
            colleagueVoice.Post(gameObject);
        }
        // else if (dialogueSpeaker == "CHARACTERNAMEHERE")
        // {

        // }
        else
        {
            Debug.LogError("Dialogue System Error: Incorrect Speaker Identifier");
        }
    }

    void EndPreviousDialogueVoice()
    {
        playerVoiceEnd.Post(gameObject);
        colleagueVoiceEnd.Post(gameObject);
    }
}