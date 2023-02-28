using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <Disclaimer>
///     Original Base Code by Tarodev
///     Ultimate 2D Platformer Controller in Unity: https://youtu.be/3sWTzMsmdx8
/// </Disclaimer>

/// <Contents>
///     Player Movement
///         - Coyote Time
///         - Queued Jump
///         - Wall Hop
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
///         - TODO: everything
/// </Contents>

public class PlayerController : MonoBehaviour, IPlayerController
{
    public CharacterController controller;

    #region UI
    public GameObject score { get; private set; }
    public int deathCount { get; private set; }
    #endregion

    #region Overrides
    private bool dialogueActive;
    private bool wetfloorOverride;
    public bool playerInvincible { get; private set; }
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
    private float _currentHorizontalSpeed, _currentVerticalSpeed;

    private bool _active;
    void Awake() => Invoke(nameof(Activate), 0.5f);
    void Activate() => _active = true;

    void Start()
    {
        playerInvincible = false;
        wetfloorOverride = false;
    }

    private void Update() 
    {
        if(!_active) return;

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
    }

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
    private bool _colUp, _colRight, _colDown, _colLeft;
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

        // FIXME: Collison Error with all hitbox
        // TODO: Modify this to fit accurate hitbox
        // _colUp = Physics.CheckSphere(transform.position + new Vector3(0, 2, 0), 0.2f);
    }
    #endregion

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

        // if (_currentHorizontalSpeed > 0 && _colRight || _currentHorizontalSpeed < 0 && _colLeft) {
        //     // Don't walk through walls
        //     _currentHorizontalSpeed = 0;
        // }
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
    
    private void CalculateJump() 
    {
        // Jump if: grounded or within coyote threshold || sufficient jump buffer
        if (Input.Jumped && CanUseCoyote || HasBufferedJump) 
        {
            _currentVerticalSpeed = _jumpHeight;
            _coyoteUsable = false;
            _timeLeftGrounded = float.MinValue;
            JumpingThisFrame = true;
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

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(!_colDown && hit.normal.y < 0.1f)
        {
            if(Input.Jumped && !wallJumpLock)
            {
                // Debug.DrawRay(hit.point, hit.normal, Color.red, 1.25f);
                _currentVerticalSpeed = _jumpHeight;
                _currentHorizontalSpeed = hit.normal.x * _jumpHeight * _wallJumpBonus;

                if(!wallJumpLock)
                {
                    StartCoroutine(SetWallJumpLock());
                }
            }
        }
    }

    [SerializeField] float jumpLockTime;
    IEnumerator SetWallJumpLock()
    {
        float clampOverride = _moveClamp;
        _moveClamp = _wallJumpClamp;
        wallJumpLock = true;
        yield return new WaitForSeconds(jumpLockTime);
        wallJumpLock = false;
        _moveClamp = clampOverride;
    }
    #endregion

    #region Move
    // [Header("MOVE")] [SerializeField, Tooltip("Raising this value increases collision accuracy at the cost of performance.")]
    // private int _freeColliderIterations = 10;

    private void MoveCharacter() 
    {
        RawMovement = new Vector3(_currentHorizontalSpeed, _currentVerticalSpeed);
        var move = RawMovement * Time.deltaTime;

        controller.Move(move);
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);
    }
    #endregion

    #region Wet Floor Trap
    public void WetFloor()
    {
        if(Input.X > 0)
        {
            this.gameObject.GetComponent<Transform>().Rotate(0f, 0f, 90f, Space.Self);
        }
        else
        {
            this.gameObject.GetComponent<Transform>().Rotate(0f, 0f, -90f, Space.Self);
        }
        controller.height = 0.027f;
        controller.center = new Vector3(0, 0, 0);
        wetfloorOverride = true;
        playerInvincible = true;
        StartCoroutine(WetFloorDuration());
    }

    IEnumerator WetFloorDuration()
    {
        yield return new WaitForSeconds(2);
        
        GameObject.Find("WetFloorWithSign").transform.GetChild(0).gameObject.GetComponent<WetFloorTrap>().SpawnDeadBody();

        if(this.gameObject.GetComponent<Transform>().rotation.z > 0)
        { 
            this.gameObject.GetComponent<Transform>().Rotate(0f, 0f, -90f, Space.Self);
        }
        else
        {
            this.gameObject.GetComponent<Transform>().Rotate(0f, 0f, 90f, Space.Self);
        }
        controller.height = 0.055f;
        controller.center = new Vector3(0, 0.01f, 0);
    }
    #endregion

    #region RespawnCalls
    public void RespawnCall()
    {
        _currentHorizontalSpeed = 0;
        _currentVerticalSpeed = 0;
        this.gameObject.GetComponent<Transform>().position = respawnPoint.position + new Vector3(0,-3,0);
        this.gameObject.SetActive(true);
        wetfloorOverride = false;
        playerInvincible = false;
        deathCount++;
        score.GetComponent<Text>().text = "Employee Number UT069-0" + (deathCount + 1);
    }

    public void SetRespawnPoint(Transform newLocation)
    {
        this.respawnPoint = newLocation;
    }
    #endregion

    public void RemoveHorizontalInertia()
    {
        // this.lastMove.x = 0;

        // FIXME: Test This
        _lastPosition = transform.position;
    }
}

/// <Checks>
///     [x] Spikes
///     [x] Wall Spikes
///     [x] Wet Floor
///     [x] Fire + Pressure Plate
///     [ ] Door + Keycard
///     [x] Lights
///     [x] NPC
/// </Checks>