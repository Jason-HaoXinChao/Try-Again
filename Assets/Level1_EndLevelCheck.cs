using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1_EndLevelCheck : MonoBehaviour
{
    [SerializeField] GameObject questLog;
    int currProgress;
    bool once;
    private GameObject gameManager;

    void Start()
    {
        once = true;
        gameManager = GameObject.Find("GameManager");
    }

    void Update()
    {
        currProgress = questLog.GetComponent<UnityEngine.UI.Text>().text[17] - '0';
        if(once && currProgress == 3 && !GlobalDialogueSystem.GetInstance().dialogueIsPlaying)
        {
            once = false;
            StartCoroutine(EndLevel());
        }
    }

    IEnumerator EndLevel()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Level Complete");
        gameManager.GetComponent<GameManager>().currentLevel++;
    }
}
