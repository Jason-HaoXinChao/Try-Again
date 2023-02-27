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
///         - TODO: Wall Hop
///     
///     UI Score Update
///         - death count
///
///     Dialogue System Override
///
///     Traps:
///         - Wet Floor Override
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
    private Transform respawnPoint;
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

        if(!dialogueActive && !wetfloorOverride) GatherInput();
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
        if (Input.X != 0) {
            // Set horizontal move speed
            _currentHorizontalSpeed += Input.X * _acceleration * Time.deltaTime;

            // clamped by max frame movement
            _currentHorizontalSpeed = Mathf.Clamp(_currentHorizontalSpeed, -_moveClamp, _moveClamp);

            // Apply bonus at the apex of a jump
            var apexBonus = Mathf.Sign(Input.X) * _apexBonus * _apexPoint;
            _currentHorizontalSpeed += apexBonus * Time.deltaTime;
        }
        else {
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

    // #region Wall Jump
    // bool wallJumpLock;
    // void OnControllerColliderHit(ControllerColliderHit hit)
    // {
    //     if(!controller.isGrounded && hit.normal.y < 0.1f)
    //     {
    //         if(Input.GetButtonDown("Jump"))
    //         {
    //             Debug.DrawRay(hit.point, hit.normal, Color.red, 1.25f);
    //             verticalVelocity = jumpForce;
    //             moveVector = hit.normal * speed;
    //             if(!wallHopLock)
    //             {
    //                 StartCoroutine(SetWallHopLock());
    //             }
    //         }
    //     }
    // }

    // IEnumerator SetWallHopLock()
    // {
    //     wallHopLock = true;
    //     yield return new WaitForSeconds(0.2f);
    //     wallHopLock = false;
    // }
    // #endregion

    #region Move
    // [Header("MOVE")] [SerializeField, Tooltip("Raising this value increases collision accuracy at the cost of performance.")]
    // private int _freeColliderIterations = 10;

    // We cast our bounds before moving to avoid future collisions
    private void MoveCharacter() 
    {
        var pos = transform.position;
        RawMovement = new Vector3(_currentHorizontalSpeed, _currentVerticalSpeed); // Used externally
        var move = RawMovement * Time.deltaTime;
        var furthestPoint = pos + move;

        // check furthest movement. If nothing hit, move and don't do extra checks
        controller.Move(move);

        // // otherwise increment away from current pos; see what closest position we can move to
        // var positionToMoveTo = transform.position;
        // for (int i = 1; i < _freeColliderIterations; i++) {
        //     // increment to check all but furthestPoint - we did that already
        //     var t = (float)i / _freeColliderIterations;
        //     var posToTry = Vector2.Lerp(pos, furthestPoint, t);

        //     if (Physics2D.OverlapBox(posToTry, _characterBounds.size, 0, _groundLayer)) {
        //         transform.position = positionToMoveTo;

        //         // We've landed on a corner or hit our head on a ledge. Nudge the player gently
        //         if (i == 1) {
        //             if (_currentVerticalSpeed < 0) _currentVerticalSpeed = 0;
        //             var dir = transform.position - hit.transform.position;
        //             transform.position += dir.normalized * move.magnitude;
        //         }

        //         return;
        //     }

        //     positionToMoveTo = posToTry;
        // }
    }
    #endregion

    #region RespawnCalls
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
    #endregion

    public void RemoveHorizontalInertia()
    {
        // this.lastMove.x = 0;

        // TODO: Test This
        _lastPosition = transform.position;
    }
}
