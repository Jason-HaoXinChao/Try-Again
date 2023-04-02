using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStamp : MonoBehaviour
{
    [SerializeField] private List<GameObject> stamps;
    [SerializeField] private float upSpeed;   // distance move up per frame
    [SerializeField] private float downSpeed;   // distance move down per frame
    [SerializeField] private float height;   // distance per position
    private List<int> positions;
    private List<Vector3> originalPosition;
    private List<bool> waiting;

    // Start is called before the first frame update
    void Start()
    {
        positions = new List<int>();
        originalPosition = new List<Vector3>();
        waiting = new List<bool>();
        foreach (GameObject stamp in stamps)
        {
            positions.Add(-1);
            originalPosition.Add(stamp.transform.position + Vector3.zero);
            waiting.Add(false);
        }
        StartCoroutine(Initialize());
    }

    // Update is called once per frame
    void Update()
    {
        int index = 0;
        while (index<stamps.Count)
        {
            if (positions[index] >= 0 && !waiting[index]) 
            {
                float targetY = originalPosition[index].y;
                targetY += positions[index] * height;
                Vector3 targetLocation = new Vector3(originalPosition[index].x, targetY, originalPosition[index].z);
                float speed = 0;
                if (positions[index] == 0) {
                    speed = downSpeed;
                } else {
                    speed = upSpeed;
                }
                stamps[index].transform.position = Vector3.MoveTowards(stamps[index].transform.position, targetLocation, speed);
                if (stamps[index].transform.position.y == targetY) {
                    if (positions[index] < 3) {
                        // move up 1 level
                        positions[index]++;
                    } else {
                        // drop
                        positions[index] = 0;
                    }
                    StartCoroutine(Wait(index));
                }
            }
            index++;
        }
    }

    // makes sure the stamps start moving some seconds apart
    IEnumerator Initialize()
    {
        int initialCounter = 0;
        while (initialCounter<stamps.Count) {
            yield return new WaitForSeconds(2.0f);
            positions[initialCounter]++;
            initialCounter++;
        }
    }

    // make stamp wait before moving to next spot
    IEnumerator Wait(int index)
    {
        waiting[index] = true;
        yield return new WaitForSeconds(1.0f);
        waiting[index] = false;
    }
}
