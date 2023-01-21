using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveForce = 10f;
    private float jumpForce = 10f;
    private float moveX;
    private Rigidbody2D playerBody;
    private bool isGrounded;
    private string GroundTag = "Ground";
    

    private void Awake() {
        playerBody = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerMoveKeyboard();
        PlayerJump();
        
    }

    private void FixedUpdate() {
        // PlayerJump();
    }

    void playerMoveKeyboard() {
        moveX = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(moveX, 0f, 0f) * Time.deltaTime * moveForce;
    }

    void PlayerJump() {
        if (Input.GetButtonDown("Jump") && isGrounded) {
            isGrounded = false;
            playerBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag(GroundTag)) {
            isGrounded = true;
            Debug.Log("Landed");
        }
    }
}
