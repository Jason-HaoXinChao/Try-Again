using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateCap : MonoBehaviour
{
    public int frameRate = 60;

    void Start() {
        StartCoroutine(changeFramerate());
    }

    
    IEnumerator changeFramerate() {
        yield return new WaitForSeconds(1);
        Application.targetFrameRate = frameRate;
    }

}
