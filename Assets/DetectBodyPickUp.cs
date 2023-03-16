using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectBodyPickUp : MonoBehaviour
{
    private List<GameObject> bodyInRange;
    private bool bodyInHand;
    private GameObject bodyHighlighted;
    [SerializeField] private GameObject deadPlayer;
    private bool pickupLock;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        bodyInRange = new List<GameObject>();
        bodyInHand = false;
        bodyHighlighted = null;
        player = GameObject.Find("BlockPlayer");
    }

    // Update is called once per frame
    void Update()
    {
        BodyPickUp();
        BodyDrop();
        if (bodyInHand == false && bodyInRange.Count > 0 && !player.GetComponent<PlayerController>().playerInvincible) {
            // highlight closest corpse
            GameObject closestBody = FindClosestCorpse();
            if (bodyHighlighted != null & closestBody != bodyHighlighted) {
                SwitchHighlight(bodyHighlighted, false);
            }
            SwitchHighlight(closestBody, true);
            bodyHighlighted = closestBody;
        }
    }

    private void OnTriggerEnter(Collider other) {
        GameObject body = other.gameObject;
        if (body.tag == "MoveableCorpse") {
            // Debug.Log(body);
            if (!bodyInRange.Contains(body)) {
                bodyInRange.Add(body);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject body = other.gameObject;
        if (bodyInRange.Contains(body)) {
            bodyInRange.Remove(body);
            SwitchHighlight(body, false);
            if (bodyHighlighted == body) {
                bodyHighlighted = null;
            }
        }
    }

    private GameObject FindClosestCorpse()
    {
        GameObject closestBody = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject body in bodyInRange)
        {
            Vector3 delta = body.transform.position - position;
            float currDistance = delta.sqrMagnitude;
            if (currDistance <= distance) {
                closestBody = body;
                distance = currDistance;
            }
        }
        return closestBody;
    }

    private void SwitchHighlight(GameObject corpse, bool status)
    {
        corpse.GetComponent<HighlightCorpse>().SetHighlight(status);
    }

    void BodyPickUp()
    {
        if(bodyHighlighted != null && bodyInHand == false && Input.GetButtonDown("CorpseInteract"))
        {
            bodyInRange.Remove(bodyHighlighted);
            Destroy(bodyHighlighted, 0.1f);
            bodyHighlighted = null;
            bodyInHand = true;
            StartCoroutine(SetPickupLock());
        }
    }

    void BodyDrop()
    {
        if (!pickupLock) 
        {
            if(bodyInHand == true && Input.GetButtonDown("CorpseInteract"))
            {
                GameObject player = GameObject.FindWithTag("Player");
                Vector3 position = player.gameObject.transform.position;
                Quaternion rotation = player.gameObject.transform.rotation;
                if (rotation.y == 0) {
                    position.x -= 2.0f;
                } else {
                    position.x += 2.0f;
                }
                Instantiate(deadPlayer, position, rotation);
                bodyInHand = false;
            }
        }
        
    }

    public void Reset()
    {
        bodyInRange.Clear();
        bodyInHand = false;
        if (bodyHighlighted != null) {
            SwitchHighlight(bodyHighlighted, false);
        }
        bodyHighlighted = null;
    }

    IEnumerator SetPickupLock()
    {
        pickupLock = true;
        yield return new WaitForSeconds(0.2f);
        pickupLock = false;
    }
}
