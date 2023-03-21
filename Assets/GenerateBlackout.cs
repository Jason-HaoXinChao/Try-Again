using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBlackout : MonoBehaviour
{
    [SerializeField] private float timeBetweenBlackout;
    [SerializeField] private float blackoutSize;
    [SerializeField] private GameObject blackBar;
    [SerializeField] private GameObject flickeringblackBar;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private bool start;
    private Vector3 currentPosition;
    private GameObject nextBlackBar;
    private bool queued;
    private bool flickering;
    [SerializeField] private GameObject blackoutMenu;

    // Start is called before the first frame update
    void Start()
    {
        start = false;
        queued = false;
        flickering = false;
        currentPosition = startPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (start) {
            if (!queued) {
                StartCoroutine(Queue());
            } else {
                if (!flickering) {
                    StartCoroutine(Flicker());
                }
            }
        }
    }

    IEnumerator Queue() {
        queued = true;
        nextBlackBar = Instantiate(flickeringblackBar, currentPosition, Quaternion.identity) as GameObject;
        nextBlackBar.transform.localScale = new Vector3(999f, blackoutSize, 10f);
        yield return new WaitForSeconds(timeBetweenBlackout);
        Destroy(nextBlackBar);
        GameObject solidBar = Instantiate(blackBar, currentPosition + Vector3.zero, Quaternion.identity) as GameObject;
        solidBar.transform.localScale = new Vector3(999f, blackoutSize, 10f);
        currentPosition.y -= blackoutSize;
        queued = false;
    }

    IEnumerator Flicker() {
        flickering = true;
        Debug.Log("flicker");
        Color tmp = nextBlackBar.GetComponent<Renderer>().material.color;
        tmp.a = 0f;
        nextBlackBar.GetComponent<Renderer>().material.color = tmp;
        yield return new WaitForSeconds(0.5f);
        tmp.a = 1f;
        nextBlackBar.GetComponent<Renderer>().material.color = tmp;
        yield return new WaitForSeconds(0.5f);
        flickering = false;
    }

    public void StartBlackout(){
        start = true;
    }

    public void OpenBlackoutMenu()
    {
        start = false;
        blackoutMenu.SetActive(true);
    }
}
