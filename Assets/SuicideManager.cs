using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SuicideManager : MonoBehaviour
{

    private float startTime;
    private bool buttonHeld;
    private float holdDuration = 2.0f;
    private GameObject player;
    [SerializeField] private GameObject deadPlayer;
    // private float opacity;
    private Image blackScreen;
    [SerializeField] TextMeshProUGUI timeDisplay;
    [SerializeField] GameObject timerCollection;

    public CameraShake cameraShake;


    // Start is called before the first frame update
    void Start()
    {
        startTime = float.NegativeInfinity;
        buttonHeld = false;
        player = GameObject.Find("BlockPlayer");
        // opacity = 0f;
        blackScreen = GameObject.Find("SuicideScreen/Canvas/Fade").GetComponent<Image>();
        timeDisplay.text = "0.00";
        timerCollection.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {   
        if (buttonHeld) {
            var tempColour = blackScreen.color;
            // opacity = Mathf.Lerp(0, 1, time.Time / (startTime + holdDuration));
            tempColour.a = Mathf.Lerp(0f, 0.75f, Time.time / (startTime + holdDuration));
            blackScreen.color = tempColour;

            cameraShake.Shake(3.4f);
            double timerT = 2 - Math.Round(Time.time - startTime, 2);
            if (timerT < 0.1) timerT = 0.00;
            timeDisplay.text = timerT.ToString();

            if (startTime + holdDuration <= Time.time) {
                // Debug.Log("dead");
                buttonHeld = false;

                tempColour.a = 0f;
                blackScreen.color = tempColour;

                timerCollection.SetActive(false);
                cameraShake.Shake(0f);

                if (!player.GetComponent<PlayerController>().playerInvincible) {
                    Vector3 position = player.transform.position;
                    Quaternion rotation = player.transform.rotation;
                    player.SetActive(false);
                    Instantiate(deadPlayer, position, rotation);
                    StartCoroutine(RespawnTimer());
                } else {
                    Debug.Log("stopped sliding");
                    player.GetComponent<PlayerController>().RemoveHorizontalInertia();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            startTime = Time.time;
            buttonHeld = true;
            timerCollection.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.R)) {
            startTime = float.NegativeInfinity;
            buttonHeld = false;

            var tempColour = blackScreen.color;
            tempColour.a = 0f;
            blackScreen.color = tempColour;
            timerCollection.SetActive(false);
            cameraShake.Shake(0f);
        }
    }

    IEnumerator RespawnTimer()
    {
        yield return new WaitForSeconds(1.3f);
        player.GetComponent<PlayerController>().RespawnCall();
    }
}
