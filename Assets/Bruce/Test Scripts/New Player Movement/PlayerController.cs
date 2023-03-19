using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <Disclaimer>
///     Original Base Code by Tarodev
///     Ultimate 2D Platformer Controller in Unity: https://youtu.be/3sWTzMsmdx8
///     https://github.com/Matthew-J-Spencer/Ultimate-2D-Controller/blob/main/Scripts/PlayerController.cs
/// </Disclaimer>

/// <Contents>
///     Player Movement
///         - Coyote Time
///         - Queued Jump
///         - Wall Hop
///         - Wall Slide
///     
///     UI Score Update
///         - Death Counter
///
///     Dialogue System Override
///
///     Traps:
///         - Wet Floor Override (Sliding)
///
///     Animations
///         - Idle
///         - Walk
///         - Jump
///         - Slide
///         - Holding Corpse
/// </Contents>

public class PlayerController : MonoBehaviour, IPlayerController
{
    public CharacterController controller;

    #region AssistMode
    public bool AssistMode = false;
    #endregion

    #region Animatior
    Animator _Animator;
    bool isJumping;
    bool isHoldingCorpse;
    #endregion

    #region UI
    public GameObject score;
    public int deathCount { get; private set; }
    private GameObject gameManager;
    private GameObject pauseMenu;
    #endregion

    #region Overrides
    private bool dialogueActive;
    private bool wetfloorOverride;
    public bool playerInvincible { get; private set; }
    private float clampOverride;
    #endregion

    #region Respawns
    public Transform respawnPoint;
    #endregion

    #region Inputs
    public Vector3 Velocity { get; private set; }
    public FrameInput Input { get; private set; }
    public bool JumpingThisFrame { get; private set; }
    public bool LandingThisFrame { get; private set; }
    public Vector3 RawMovement { get; private set; }
    public bool Grounded => _colDown;
    #endregion

    private Vector3 _lastPosition;

    [Header("SPEED")]
    [SerializeField] private float _currentHorizontalSpeed;
    [SerializeField] private float _currentVerticalSpeed;

    private bool _active;
    void Awake() => Invoke(nameof(Activate), 0.5f);
    void Activate() => _active = true;

    void Start()
    {
        _Animator = GetComponent<Animator>();
        playerInvincible = false;
        wetfloorOverride = false;
        clampOverride = _moveClamp;
        gameManager = GameObject.Find("GameManager");
        pauseMenu = GameObject.Find("Pause Menu");
    }

    private void Update() 
    {
        if(!_active) return;

        if (!pauseMenu.transform.GetChild(0).gameObject.activeSelf)
        {
            // Check if Dialogue Sequence is active
            dialogueActive = GlobalDialogueSystem.GetInstance().dialogueIsPlaying;

            // Calculate velocity
            Velocity = (transform.position - _lastPosition) / Time.deltaTime;
            _lastPosition = transform.position;

            if(!dialogueActive && !wetfloorOverride && !wallJumpLock) GatherInput();
            RunCollisionChecks();

            CalculateWalk(); // Horizontal movement
            CalculateJumpApex(); // Affects fall speed, so calculate before gravity
            CalculateGravity(); // Vertical movement
            CalculateJump(); // Possibly overrides vertical

            if(dialogueActive) _currentHorizontalSpeed = 0;

            MoveCharacter(); // Actually perform the axis movement

            CharacterRotation(false, 0);
            CharacterAnimation();
        }
    }

    #region Animation
    void CharacterRotation(bool wallHoping, float dir)
    {
        /// Changes the direction that player faces when moving
        if(!wallHoping)
        {
            if (Input.X < 0)
            {
                Quaternion target = Quaternion.Euler(0, 0, 0);
                _Animator.transform.rotation = target;
            }
            else if (Input.X > 0)
            {
                Quaternion target = Quaternion.Euler(0, 180, 0);
                _Animator.transform.rotation = target;
            }
        }
        else
        {
            if (dir< 0)
            {
                Quaternion target = Quaternion.Euler(0, 0, 0);
                _Animator.transform.rotation = target;
            }
            else if (dir > 0)
            {
                Quaternion target = Quaternion.Euler(0, 180, 0);
                _Animator.transform.rotation = target;
            }
        }
    }

    void CharacterAnimation()
    {
        bool isWalking = Input.X != 0;
        _Animator.SetBool("IsWalking", isWalking);
        _Animator.SetBool("IsJumping", isJumping);
        _Animator.SetBool("IsSliding", wetfloorOverride);
        _Animator.SetBool("IsHoldingCorpse", isHoldingCorpse);
        isJumping = false;
    }
    #endregion

    #region Gather Input
    private void GatherInput() 
    {
        Input = new FrameInput {
            Jumped = UnityEngine.Input.GetButtonDown("Jump"),
            X = UnityEngine.Input.GetAxisRaw("Horizontal")
        };
        if (Input.Jumped) {
            _lastJumpPressed = Time.time;
        }
    }
    #endregion

    #region Collisions
    [Header("COLLISION")]
    [SerializeField] private bool _colUp;
    [SerializeField] private bool _colRight, _colDown, _colLeft;
    private float _timeLeftGrounded;

    private void RunCollisionChecks() {
        // Ground
        LandingThisFrame = false;
        bool groundedCheck = controller.isGrounded;
        if (_colDown && !groundedCheck) _timeLeftGrounded = Time.time; // Only trigger when first leaving
        else if (!_colDown && groundedCheck) {
            _coyoteUsable = true; // Only trigger when first touching
            LandingThisFrame = true;
        }

        _colDown = groundedCheck;
        _colUp = Physics.CheckSphere(transform.position + new Vector3(0, 1.3f, 0), 0.05f);

        //Wall
        _colLeft = Physics.CheckBox(transform.position + new Vector3(-Dis, Tall, 0), new Vector3(BoxX/2, BoxY/2, BoxZ/2));
        _colRight = Physics.CheckBox(transform.position + new Vector3(Dis, Tall, 0), new Vector3(BoxX/2, BoxY/2, BoxZ/2));

        if (wallCheck && _colLeft || _colRight) {
            _wallhopUsable = true;
        }
        else if (wallCheck && !_colLeft && !_colRight)
        {
            wallCheck = false;
        }
    }
    #endregion

    // TODO: Comment out for production build
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position + new Vector3(0, 1.3f, 0), 0.05f);
        Gizmos.DrawCube(transform.position + new Vector3(-Dis, Tall, 0), new Vector3(BoxX, BoxY, BoxZ));
        Gizmos.DrawCube(transform.position + new Vector3(Dis, Tall, 0), new Vector3(BoxX, BoxY, BoxZ));
    }

    [Header("SIDE COLLISION")]
    [SerializeField] private float BoxX = 0.05f;
    [SerializeField] private float BoxY = 1.5f, BoxZ = 0.1f, Dis = 0.4f, Tall = 0.3f;

    #region Walk
    [Header("WALKING")] [SerializeField] private float _acceleration = 90;
    [SerializeField] private float _moveClamp = 13;
    [SerializeField] private float _deceleration = 60f;
    [SerializeField] private float _apexBonus = 2;

    private void CalculateWalk() 
    {
        if (Input.X != 0) 
        {
            // Set horizontal move speed
            _currentHorizontalSpeed += Input.X * _acceleration * Time.deltaTime;

            // clamped by max frame movement
            _currentHorizontalSpeed = Mathf.Clamp(_currentHorizontalSpeed, -_moveClamp, _moveClamp);

            // Apply bonus at the apex of a jump
            var apexBonus = Mathf.Sign(Input.X) * _apexBonus * _apexPoint;
            _currentHorizontalSpeed += apexBonus * Time.deltaTime;
        }
        else 
        {
            // No input. Let's slow the character down
            _currentHorizontalSpeed = Mathf.MoveTowards(_currentHorizontalSpeed, 0, _deceleration * Time.deltaTime);
        }
    }
    #endregion

    #region Gravity
    [Header("GRAVITY")] [SerializeField] private float _fallClamp = -40f;
    [SerializeField] private float _minFallSpeed = 80f;
    [SerializeField] private float _maxFallSpeed = 120f;
    private float _fallSpeed;

    private void CalculateGravity() 
    {
        // Fall
        _currentVerticalSpeed -= _fallSpeed * Time.deltaTime;

        // Clamp
        if (_currentVerticalSpeed < _fallClamp) _currentVerticalSpeed = _fallClamp;
    }
    #endregion

    #region Jump
    [Header("JUMPING")] [SerializeField] private float _jumpHeight = 30;
    [SerializeField] private float _jumpApexThreshold = 10f;
    [SerializeField] private float _coyoteTimeThreshold = 0.1f;
    [SerializeField] private float _jumpBuffer = 0.1f;
    private bool _coyoteUsable;
    private float _apexPoint; // Becomes 1 at the apex of a jump
    private float _lastJumpPressed;
    private bool CanUseCoyote => _coyoteUsable && !_colDown && _timeLeftGrounded + _coyoteTimeThreshold > Time.time;
    private bool HasBufferedJump => _colDown && _lastJumpPressed + _jumpBuffer > Time.time;

    private void CalculateJumpApex() 
    {
        if (!_colDown) {
            // Gets stronger the closer to the top of the jump
            _apexPoint = Mathf.InverseLerp(_jumpApexThreshold, 0, Mathf.Abs(Velocity.y));
            _fallSpeed = Mathf.Lerp(_minFallSpeed, _maxFallSpeed, _apexPoint);
        }
        else {
            _apexPoint = 0;
        }
    }

    private bool _wallhopUsable;
    private bool CanUseWallHop => _wallhopUsable && !_colDown;
    
    private void CalculateJump() 
    {
        // Jump if: grounded or within coyote threshold || sufficient jump buffer
        if (Input.Jumped) 
        {
            // Wall Jump
            if (wallCheck && !controller.isGrounded)
            {
                if(!wallJumpLock && CanUseWallHop && wallHit != null)
                {
                    // Debug.DrawRay(hit.point, hit.normal, Color.red, 1.25f);
                    _currentVerticalSpeed = _jumpHeight;
                    _currentHorizontalSpeed = wallHit.normal.x * _jumpHeight * _wallJumpBonus;

                    isJumping = true;
                    CharacterRotation(true, wallHit.normal.x);

                    wallHit = null;

                    _moveClamp = _wallJumpClamp;
                    
                    if(!wallJumpLock)
                    {
                        StartCoroutine(SetWallJumpLock());
                    }
                }
            }
            else // Normal Jump
            {
                if (CanUseCoyote || HasBufferedJump)
                {
                    _currentVerticalSpeed = _jumpHeight;
                    _coyoteUsable = false;
                    _timeLeftGrounded = float.MinValue;
                    JumpingThisFrame = true;

                    isJumping = true;
                }
            }
        }
        else 
        {
            JumpingThisFrame = false;
        }

        if (_colUp) 
        {
            if (_currentVerticalSpeed > 0) _currentVerticalSpeed = 0;
        }
    }
    #endregion

    #region Wall Jump
    bool wallJumpLock;
    [Header("WALL JUMP")] [SerializeField] private float _wallJumpBonus;
    [SerializeField] private float _wallJumpClamp;

    bool wallCheck;
    ControllerColliderHit wallHit;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(!_colDown && hit.normal.y < 0.1f)
        {

            wallCheck = true;
            wallHit = hit;
        }
    }

    [SerializeField] float jumpLockTime;
    IEnumerator SetWallJumpLock()
    {
        wallJumpLock = true;
        yield return new WaitForSeconds(jumpLockTime);
        wallJumpLock = false;
        _moveClamp = clampOverride;
    }
    #endregion

    #region Move
    [Header("MOVE")] 
    // [SerializeField, Tooltip("Raising this value increases collision accuracy at the cost of performance.")]
    // private int _freeColliderIterations = 10;

    [SerializeField] private float WallSlideClamp = 4;
    private void MoveCharacter() 
    {
        if (wallCheck)
        {
            if (_currentVerticalSpeed < _fallClamp/WallSlideClamp) 
                _currentVerticalSpeed = _fallClamp/WallSlideClamp;
        }

        RawMovement = new Vector3(_currentHorizontalSpeed, _currentVerticalSpeed);
        var move = RawMovement * Time.deltaTime;

        controller.Move(move);
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);
    }
    #endregion

    #region Wet Floor Trap
    public void WetFloor()
    {
        controller.height = 0.01f;
        controller.center = new Vector3(0, 0, 0);
        wetfloorOverride = true;
        playerInvincible = true;

        this.transform.Find("PickUpBodyHitbox").GetComponent<DetectBodyPickUp>().Reset();

        _minFallSpeed = 8f;
        _maxFallSpeed = 12f;
        _currentVerticalSpeed = 0f;
        StartCoroutine(WetFloorDuration());
    }

    IEnumerator WetFloorDuration()
    {
        yield return new WaitForSeconds(1.75f);
        GameObject.Find("WetFloorWithSign").transform.GetChild(0).gameObject.GetComponent<WetFloorTrap>().SpawnDeadBody();
    }
    #endregion

    #region RespawnCalls
    public void RespawnCall()
    {
        // reset highlighted corpse
        Transform pickupBodyHitbox = this.transform.Find("PickUpBodyHitbox");
        // this if is here so scenes without the hitbox implemented can still run
        if (pickupBodyHitbox != null) {
            pickupBodyHitbox.GetComponent<DetectBodyPickUp>().Reset();
        }

        _currentHorizontalSpeed = 0;
        _currentVerticalSpeed = 0;

        this.gameObject.GetComponent<Transform>().position = respawnPoint.position + new Vector3(0,-3,0);
        
        /// Reset Player Height
        controller.height = 0.05f;
        controller.center = new Vector3(0, 0.0115f, 0);
        
        this.gameObject.SetActive(true);
        wetfloorOverride = false;
        playerInvincible = false;
        wallJumpLock = false;
        isHoldingCorpse = false;    // disable holding corpse flag
        _moveClamp = clampOverride;

        _minFallSpeed = 80f;
        _maxFallSpeed = 120f;
        
        deathCount++;
        score.GetComponent<Text>().text = "Employee Number UT069-0" + (deathCount + 1);
        gameManager.GetComponent<GameManager>().deathCount++;
        gameManager.GetComponent<GameManager>().currDeathCount++;
    }

    public void SetRespawnPoint(Transform newLocation)
    {
        this.respawnPoint = newLocation;
    }
    #endregion

    public void RemoveHorizontalInertia()
    {
        _lastPosition = transform.position;
    }

    public void FaceNPC()
    {
        Quaternion target = Quaternion.Euler(0, 90, 0);
        _Animator.transform.rotation = target;
    }

    // Toggle boolean indicating if player is holding a corpse (for animation)
    public void PickUpCorpse(bool holdingCorpse) {
        isHoldingCorpse = holdingCorpse;
    }
}
