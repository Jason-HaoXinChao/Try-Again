using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class BlinkingText : MonoBehaviour
{   
    TMP_Text text;
    public float blinkFadeInTime = 0.5f;
    public float blinkStayTime = 0.8f;
    public float blinkFadeOutTime = 0.7f;
    private float time = 0f;
    private Color color;
    private bool selected;
    private EventSystem eventSystem;

    // Start is called before the first frame update
    void Start()
    {
        text = this.gameObject.GetComponent<TMP_Text>();
        color = text.color;
        selected = false;
        eventSystem = EventSystem.current;
    }

    // Update is called once per frame
    void Update()
    {   
        if (selected || (eventSystem != null && (eventSystem.currentSelectedGameObject == this.gameObject || eventSystem.currentSelectedGameObject == this.transform.parent.gameObject))) {
            time += Time.deltaTime;

            if (time < blinkFadeInTime) {
                var tempColor = text.color;
                tempColor.a = time / blinkFadeInTime;
                text.color = tempColor;
            } else if (time < blinkFadeInTime + blinkStayTime) {
                var tempColor = text.color;
                tempColor.a = 1;
                text.color = tempColor;
            } else if (time < blinkFadeInTime + blinkStayTime + blinkFadeOutTime) {
                var tempColor = text.color;
                tempColor.a = 1 - (time - (blinkFadeInTime + blinkStayTime))/ blinkFadeOutTime;
                text.color = tempColor;
            } else {
                time = 0f;
            }
        } else {
            var tempColor = text.color;
            tempColor.a = 1;
            text.color = tempColor;
            time = 0;
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (eventData.selectedObject == this.gameObject || eventSystem.currentSelectedGameObject == this.transform.parent.gameObject) {
            selected = true;
        } else {
            selected = false;
        }
    }
}
