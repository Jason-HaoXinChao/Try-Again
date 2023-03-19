using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectBodyPickUp : MonoBehaviour
{
    private List<GameObject> bodyInRange;
    private bool bodyInHand;
    private GameObject bodyHighlighted;
    [SerializeField] private GameObject deadPlayer;
    [SerializeField] private GameObject carriableCorpse;
    private GameObject corpseCarried;
    private bool pickupLock;
    private GameObject player;
    public AK.Wwise.Event dropSound;

    // Start is called before the first frame update
    void Start()
    {
        bodyInRange = new List<GameObject>();
        bodyInHand = false;
        bodyHighlighted = null;
        player = GameObject.Find("BlockPlayer");
        player.GetComponent<PlayerController>().PickUpCorpse(false);
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
            player.GetComponent<PlayerController>().PickUpCorpse(true);
            Vector3 position = player.gameObject.transform.position;
            Quaternion playerRotation = player.gameObject.transform.rotation;
            Quaternion rotation;
            if (playerRotation.y == 0) {
                rotation = Quaternion.Euler(0, 90, -90);
            } else {
                rotation = Quaternion.Euler(0, -90, -90);
            }
            this.corpseCarried = Instantiate(carriableCorpse, position, rotation) as GameObject;
            StartCoroutine(SetPickupLock());
        }
    }

    void BodyDrop()
    {
        if (!pickupLock) 
        {
            if(bodyInHand == true && Input.GetButtonDown("CorpseInteract"))
            {
                DumpCorpse();
            }
        }
        
    }

    private void DumpCorpse()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 position = this.corpseCarried.gameObject.transform.position;
        position.y += 0.5f;
        Quaternion rotation = this.corpseCarried.gameObject.transform.rotation;
        Destroy(this.corpseCarried, 0.1f);
        this.corpseCarried = null;
        GameObject ragdoll = Instantiate(deadPlayer, position, rotation) as GameObject;
        Rigidbody spine = ragdoll.transform.Find("spine").gameObject.GetComponent<Rigidbody>();
        if (player.transform.rotation.y == 0) {
            spine.AddForce(-60f, 0f, 0f, ForceMode.VelocityChange);
        } else {
            spine.AddForce(60f, 0f, 0f, ForceMode.VelocityChange);
        }
        dropSound.Post(gameObject);
        bodyInHand = false;
        player.GetComponent<PlayerController>().PickUpCorpse(false);
    }

    public void Reset()
    {
        if (bodyInHand) {
            DumpCorpse();
            bodyInHand = false;
        }
        bodyInRange.Clear();
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
