using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightCorpse : MonoBehaviour
{
    [SerializeField] GameObject original;
    [SerializeField] GameObject highlight;

    // Start is called before the first frame update
    void Start()
    {
        original.SetActive(true);
        highlight.SetActive(false);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHighlight(bool on) {
        highlight.SetActive(on);
        original.SetActive(!on);
    }
}
