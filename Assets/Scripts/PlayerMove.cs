using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    private float moveForce = 10f;
    private float jumpForce = 5000f;
    private float moveX;
    private Rigidbody playerBody;
    private bool isGrounded;
    private string GroundTag = "Ground";
    private string WallTag = "Wall";
    private bool facingRight;
    

    private void Awake() {
        playerBody = GetComponent<Rigidbody>();    
    }

    void Start()
    {
        isGrounded = true;
        facingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        playerMoveKeyboard();
        PlayerJump();
    }

    void playerMoveKeyboard() {
        moveX = Input.GetAxisRaw("Horizontal");
        playerBody.velocity = new Vector3(moveX * moveForce, playerBody.velocity.y, 0f);

        if (moveX > 0 && !facingRight) Flip();
        else if (moveX < 0 && facingRight) Flip();
    }

    void PlayerJump() {
        if (Input.GetButtonDown("Jump") && (isGrounded)) {
            isGrounded = false;
            playerBody.AddForce(0f, jumpForce, 0f);
            Debug.Log("Jumped");
            
        }
    }

    void HandleGroundCollision(Collision collision) {
        if (collision.gameObject.CompareTag(GroundTag)) {
            isGrounded = true;
            Debug.Log("Grounded");
        }
    }

    void HandleWallJumpCollision(Collision collision) {
        if (collision.gameObject.CompareTag(WallTag)) {
            isGrounded = true;
        }
    }


    private void OnCollisionEnter(Collision collision) {
        HandleGroundCollision(collision);
        HandleWallJumpCollision(collision);

    }

    void Flip() {
        facingRight = !facingRight;

        // Need to handle character flip
        // Vector3 charScale = transform.localScale;
        // charScale.x *= -1;
        // transform.localScale = charScale;
        

    }

}
