using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerMovementBruce : MonoBehaviour
{
    private Vector3 moveVector;
    private Vector3 lastMove;

    [SerializeField] private float speed = 5;
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private float gravity = 40;

    private float verticalVelocity;
    private bool wallHopLock = false;
    public CharacterController controller;
    public GameObject score;
    private int deathCount;

    // Save and update the transform of new respawn points to this var
    public Transform respawnPoint;

    void Start()
    {
        deathCount = 0;
        // controller = gameObject.AddComponent<CharacterController>();
        //Testing Line, Remove Later
        // respawnPoint = GameObject.Find("Temp Respawn Point").GetComponent<Transform>();
    }

    void Update()
    {
        moveVector = Vector3.zero;
        moveVector.x = Input.GetAxisRaw("Horizontal");

        if(controller.isGrounded)
        {
            verticalVelocity = -1.1f;

            if(Input.GetButtonDown("Jump"))
            {
                verticalVelocity = jumpForce;
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
            moveVector = lastMove;

            if(Input.GetAxisRaw("Horizontal") != 0)
            {
                moveVector.x = Input.GetAxisRaw("Horizontal");
            }

            if(wallHopLock){moveVector = lastMove;}
        }

        moveVector.y = 0;
        moveVector.Normalize();
        moveVector *= speed;
        moveVector.y = verticalVelocity;

        moveVector.z = 0;
        controller.Move(moveVector * Time.deltaTime);
        lastMove = moveVector;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(!controller.isGrounded && hit.normal.y < 0.1f)
        {
            if(Input.GetButtonDown("Jump"))
            {
                Debug.DrawRay(hit.point, hit.normal, Color.red, 1.25f);
                verticalVelocity = jumpForce;
                moveVector = hit.normal * speed;
                if(!wallHopLock)
                {
                    StartCoroutine(SetWallHopLock());
                }
            }
        }
    }

    IEnumerator SetWallHopLock()
    {
        wallHopLock = true;
        yield return new WaitForSeconds(0.5f);
        wallHopLock = false;
    }

    public void RespawnCall()
    {
        this.gameObject.GetComponent<Transform>().position = respawnPoint.position + new Vector3(0,-3,0);
        this.gameObject.SetActive(true);
        deathCount++;
        score.GetComponent<Text>().text = "Employee Number UT069-0" + (deathCount + 1);
    }

    public void SetRespawnPoint(Transform newLocation)
    {
        this.respawnPoint = newLocation;
    }

    // Old Script =============================================================

    // private CharacterController controller;
    // private Vector3 playerVelocity;
    // private Vector3 move;
    // private bool groundedPlayer;
    // public float playerSpeed = 7.0f;
    // public float jumpHeight = 3.0f;
    // private float gravityValue = -39.81f;

    // // Save and update the transform of new respawn points to this var
    // private Transform respawnPoint;

    // private void Start()
    // {
    //     controller = gameObject.AddComponent<CharacterController>();
        
    //     //Testing Line, Remove Later
    //     respawnPoint = GameObject.Find("Temp Respawn Point").GetComponent<Transform>();
    // }

    // void Update()
    // {
    //     // print(controller.isGrounded);

    //     groundedPlayer = controller.isGrounded;
    //     if (groundedPlayer && playerVelocity.y < 0)
    //     {
    //         playerVelocity.y = -1.1f;
    //     }

    //     move = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
    //     controller.Move(move * Time.deltaTime * playerSpeed);

    //     // if (move != Vector3.zero)
    //     // {
    //     //     gameObject.transform.forward = -move;
    //     // }

    //     // Changes the height position of the player..
    //     if (Input.GetButtonDown("Jump") && groundedPlayer)
    //     {
    //         playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
    //     }

    //     playerVelocity.y += gravityValue * Time.deltaTime;
    //     controller.Move(playerVelocity * Time.deltaTime);
    // }

}
