using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;
using System;

public class GlobalDialogueSystem : MonoBehaviour
{
    static GlobalDialogueSystem instance;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject bgUI;
    [SerializeField] private GameObject characterProfile;
    [SerializeField] private GameObject characterIcon;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI speakerName;

    [Header("Player Inventory")]
    [SerializeField] private GameObject playerInv;

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }
    private bool firstLine;

    [Header("Audio")]
    public AK.Wwise.Event colleagueVoice;
    public AK.Wwise.Event colleagueVoiceEnd;
    public AK.Wwise.Event playerVoice;
    public AK.Wwise.Event playerVoiceEnd;

    private bool stillPlaying;
    private int currLine;

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
        bgUI.SetActive(false);
        characterProfile.SetActive(false);
        dialoguePanel.SetActive(false);
        firstLine = false;
        currLine = 0;
    }

    void Update()
    {
        if(!dialogueIsPlaying){return;}

        if(Input.GetButtonUp("Confirm") || Input.GetButtonUp("Jump"))
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

        playerInv.SetActive(false);

        bgUI.SetActive(true);
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
            currLine++;

            DialogueRNGVoice();

            if (tags[0].Split('_')[0] == "Player")
                speakerName.text = "Me";
            else
                speakerName.text = tags[0].Split('_')[0];
            characterIcon.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Dialogue UI/Speaker Icon/{tags[0]}");
        }
        else
        {
            ExitDialogueMode();
            GameObject blackout = GameObject.Find("Blackout");
            if (blackout) {
                blackout.GetComponent<GenerateBlackout>().StartBlackout();
            }
        }
    }

    void ExitDialogueMode()
    {
        dialogueText.text = "";
        speakerName.text = "";
        characterIcon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Dialogue UI/Speaker Icon/EmptySquare");
        
        dialogueIsPlaying = false;

        playerInv.SetActive(true);

        bgUI.SetActive(false);
        characterProfile.SetActive(false);
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    void DialogueRNGVoice()
    {
        //Change the number to modify length of speech
        float dialogueLength = dialogueText.text.Length * (0.06f + 
            (Math.Max(((dialogueText.text.Length - 40)/10), 0)/ 100));
        //Debug.Log(dialogueLength);

        string dialogueSpeaker = currentStory.currentTags[0].Split('_')[0];
        //Debug.Log(dialogueSpeaker);
        
        stillPlaying = true;

        if (dialogueSpeaker == "Player")
        {
            playerVoice.Post(gameObject);
        }
        else if (dialogueSpeaker == "Colleague")
        {
            colleagueVoice.Post(gameObject);
        }
        else if (dialogueSpeaker == "Janitor")
        {
            colleagueVoice.Post(gameObject);
        }
        else
        {
            Debug.LogError("Dialogue System Error: Incorrect Speaker Identifier");
            stillPlaying = false;
        }

        StartCoroutine(TimedStopDialogueVoice(currLine, dialogueLength));
    }

    IEnumerator TimedStopDialogueVoice(int line, float dur)
    {
        yield return new WaitForSeconds(dur);
        if (line == currLine && stillPlaying)
        {
            EndPreviousDialogueVoice();
        }
    }

    void EndPreviousDialogueVoice()
    {
        playerVoiceEnd.Post(gameObject);
        colleagueVoiceEnd.Post(gameObject);
        stillPlaying = false;
    }
}