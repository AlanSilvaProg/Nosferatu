using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public enum PlayerState { IDLE, WALK, ATTACK, JUMPING, FALL }

public class Controller : MonoBehaviour
{

    public static Controller Instance;

    public PlayerState playerState;

    public Animator playerAnimator;
    public GameObject meshPlayer;
    public LayerMask isWalkable;
    public float velocity;
    public float acceleration;
    float actualVelo;
    public float runningAcrease;
    float actualRunningVelo;
    public float veloRotate;
    public float dashDuration;
    public float dashVelocity;
    public float maxJumpHeight = 1.5f;
    public float maxJumpVelo = 30;
    public float jumpVelo = 10;
    public bool acelerateOnJump;
    public float currentHeight;

    public float jumpSpeed;

    public UnityEvent EnableDashEvent = new UnityEvent();
    public UnityEvent DisableDashEvent = new UnityEvent();
    public UnityEvent EnableStealthEvent = new UnityEvent();
    public UnityEvent DisableStealthEvent = new UnityEvent();

    [HideInInspector] public float addVeloFury = 0;

    delegate void WalkEvents();
    WalkEvents walkEvents;

    Transform back, center, front, left, right, verticalRight, verticalLeft, leftCam, rightCam;
    Transform fVertRight, fVertLeft, bVertRight, bVertLeft;
    bool grounded;
    [HideInInspector] public bool crouch;
    float groundY;
    Rigidbody playerRB;

    float horizontalInput;
    float verticalInput;

    [HideInInspector] public bool onDash;
    float timerDash;

    [HideInInspector] public bool running;

    float colliderRadius;

    float mouseX = 0.0f; //Variáveis que controla a rotação do mouse

    // for debugs

    Transform[] checkersList = new Transform[9];

    InputMaster inputMaster;

    bool blockCrouch;

    Keyboard kb;
    Gamepad controller;

    [HideInInspector] public bool ruivando;

    void Awake()
    {

        Instance = this;

        #region Ground Checkers configuration

        colliderRadius = GetComponent<CapsuleCollider>().radius + 0.01f;

        back = GameObject.Find("BackChecker").gameObject.GetComponent<Transform>();
        center = GameObject.Find("CenterChecker").gameObject.GetComponent<Transform>();
        front = GameObject.Find("FrontChecker").gameObject.GetComponent<Transform>();
        left = GameObject.Find("LeftChecker").gameObject.GetComponent<Transform>();
        right = GameObject.Find("RightChecker").gameObject.GetComponent<Transform>();
        verticalRight = GameObject.Find("RefRight").gameObject.GetComponent<Transform>();
        verticalLeft = GameObject.Find("RefLeft").gameObject.GetComponent<Transform>();
        fVertRight = GameObject.Find("FVertRightChecker").gameObject.GetComponent<Transform>();
        fVertLeft = GameObject.Find("FVertLeftChecker").gameObject.GetComponent<Transform>();
        bVertRight = GameObject.Find("BVertRightChecker").gameObject.GetComponent<Transform>();
        bVertLeft = GameObject.Find("BVertLeftChecker").gameObject.GetComponent<Transform>();

        back.position = new Vector3(back.transform.position.x, back.transform.position.y, back.transform.position.z - colliderRadius * 2);
        front.position = new Vector3(front.transform.position.x, front.transform.position.y, front.transform.position.z + colliderRadius * 2);
        left.position = new Vector3(left.transform.position.x - colliderRadius * 2, left.transform.position.y, left.transform.position.z);
        right.position = new Vector3(right.transform.position.x + colliderRadius * 2, right.transform.position.y, right.transform.position.z);
        fVertRight.position = new Vector3(fVertRight.transform.position.x + colliderRadius * 2, fVertRight.transform.position.y, fVertRight.transform.position.z + colliderRadius);
        fVertLeft.position = new Vector3(fVertLeft.transform.position.x - colliderRadius * 2, fVertLeft.transform.position.y, fVertLeft.transform.position.z + colliderRadius);
        bVertRight.position = new Vector3(bVertRight.transform.position.x + colliderRadius * 2, bVertRight.transform.position.y, bVertRight.transform.position.z - colliderRadius);
        bVertLeft.position = new Vector3(bVertLeft.transform.position.x - colliderRadius * 2, bVertLeft.transform.position.y, bVertLeft.transform.position.z - colliderRadius);

        #endregion

        #region Input System Configs

        inputMaster = new InputMaster();
        inputMaster.PlayerControlls.Stealth.performed += ctx => StealthInput();
        inputMaster.PlayerControlls.DashInputs.performed += ctx =>
        {
            if (!onDash && !blockCrouch)
            {
                blockCrouch = true;
                EnableDashEvent.Invoke();
            }
        };
        inputMaster.PlayerControlls.RunInputs.performed += ctx =>
        {

        };

        #endregion

    }

    // Start is called before the first frame update
    void Start()
    {

        #region debugs
#if UNITY_EDITOR
        for (int i = 0; i < checkersList.Length; i++)
        {
            checkersList[i] = i == 0 ? back : i == 1 ? center : i == 2 ? front : i == 3 ? left : i == 4 ? right : i == 5 ? fVertRight : i == 6 ? fVertLeft : i == 7 ? bVertRight : i == 8 ? bVertLeft : bVertLeft;
        }
#endif
        #endregion

        playerState = PlayerState.IDLE;
        playerRB = GetComponent<Rigidbody>();
        playerRB.useGravity = true;
        playerRB.isKinematic = false;
        jumpSpeed = jumpVelo;

        walkEvents += UpdateRotation;

        EnableDashEvent.AddListener(() =>
        {
            onDash = true;
            timerDash = 0;
        });

        DisableDashEvent.AddListener(() =>
        {
            onDash = false;
        });

    }

    // Update is called once per frame
    void Update()
    {

        if(playerState == PlayerState.WALK)
        {
            if(actualVelo < velocity)
            {
                actualVelo += Time.deltaTime * acceleration;
                actualVelo = Mathf.Clamp(actualVelo, 0, velocity);
            }
            if (running)
            {
                if (actualRunningVelo < runningAcrease)
                {
                    actualRunningVelo += Time.deltaTime * acceleration;
                    actualRunningVelo = Mathf.Clamp(actualRunningVelo, 0, runningAcrease);
                }
            }
        }
        else
        {
            actualVelo = 0;
            actualRunningVelo = 0;
        }

        kb = InputSystem.GetDevice<Keyboard>();
        controller = InputSystem.GetDevice<Gamepad>();

        if (PlayerInfo.Instance.life <= 0) return;

        #region debugs
#if UNITY_EDITOR
        foreach (Transform checkers in checkersList)
        {
            Debug.DrawRay(checkers.position, Vector3.down * 0.7f, Color.red);
        }
#endif
        #endregion

        if (Cheats.Instance != null)
        {
            if (!Cheats.Instance.noClipCheat)
                GroundCheck();
            else
                grounded = true;
        }
        else
        {
            GroundCheck();
        }

        if (ruivando)
        {
            horizontalInput = 0;
            verticalInput = 0;
        }
        else
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
        }

        if (onDash)
        {
            timerDash += Time.deltaTime;
            if (timerDash >= dashDuration)
            {
                DisableDashEvent.Invoke();
            }
        }

        if (playerState != PlayerState.FALL)
        {
            #region Update Player States

            // grounded true line
            if (playerState != PlayerState.ATTACK && playerState != PlayerState.JUMPING)
            {
                playerState = grounded ? horizontalInput != 0 ? PlayerState.WALK : verticalInput != 0 ? PlayerState.WALK : PlayerState.IDLE
                    // grounded false line
                    : PlayerState.FALL;

                running = playerState == PlayerState.WALK ? kb.leftShiftKey.isPressed ? true : controller != null ? controller.leftStickButton.isPressed : false : false;
            }
            #endregion
        }
        // Mantendo jogador no chão quando estiver próximo, e ativando gravidade quando não estiver no chão
        if (grounded && playerState != PlayerState.JUMPING && playerState != PlayerState.FALL && !Cheats.Instance.noClipCheat)
        {
            Vector3 newPos = new Vector3(transform.position.x, groundY, transform.position.z);
            transform.position = newPos;
        }

        if (horizontalInput != 0 && !onDash || verticalInput != 0 && !onDash)
        {
            walkEvents();
        }

        if (playerState != PlayerState.FALL && playerState != PlayerState.JUMPING)
        {
            playerRB.velocity = Vector3.zero;
        }

        if (Cheats.Instance != null)
        {
            if (Cheats.Instance.noClipCheat)
            {
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<CapsuleCollider>().enabled = false;
            }
        }
        playerAnimator.SetBool("Walking", playerState == PlayerState.WALK);
        playerAnimator.SetBool("Running", running);

    }

    private void FixedUpdate()
    {
        if (ruivando || PlayerInfo.Instance.life <= 0) return;

        if (blockCrouch && crouch)
        {
            EnableStealthEvent.Invoke();
            blockCrouch = false;
        }
        else if (blockCrouch)
        {
            blockCrouch = false;
        }
        if (playerState != PlayerState.ATTACK && playerState != PlayerState.IDLE)
        {
            float velo;

            if (!onDash)
            {
                velo = running ? actualVelo + actualRunningVelo : actualVelo;
                velo += addVeloFury;
            }
            else
            {
                velo = running ? dashVelocity + actualRunningVelo : dashVelocity;
                velo += addVeloFury;
            }

            if (onDash)
            {
                if (Cheats.Instance != null)
                {
                    if (Cheats.Instance.noClipCheat)
                    {
                        playerRB.velocity = new Vector3(Camera.main.transform.forward.x * Time.deltaTime * velo, Camera.main.transform.forward.y * Time.deltaTime * velo, Camera.main.transform.forward.z * Time.deltaTime * velo);
                        return;
                    }
                    else
                    {
                        playerRB.velocity = new Vector3(meshPlayer.gameObject.transform.forward.x * Time.deltaTime * velo, playerRB.velocity.y, meshPlayer.gameObject.transform.forward.z * Time.deltaTime * velo);
                        return;
                    }
                }
                else
                {
                    playerRB.velocity = new Vector3(meshPlayer.gameObject.transform.forward.x * Time.deltaTime * velo, playerRB.velocity.y, meshPlayer.gameObject.transform.forward.z * Time.deltaTime * velo);
                    return;
                }
            }

            if (verticalInput == 0 && horizontalInput != 0)
            {
                if (Cheats.Instance != null)
                {
                    if (Cheats.Instance.noClipCheat)
                    {
                        velo *= Mathf.Abs(horizontalInput);
                        playerRB.velocity = new Vector3(Camera.main.transform.forward.x * Time.deltaTime * velo, Camera.main.transform.forward.y * Time.deltaTime * velo, Camera.main.transform.forward.z * Time.deltaTime * velo);
                    }
                    else
                    {
                        velo *= Mathf.Abs(horizontalInput);
                        playerRB.velocity = new Vector3(meshPlayer.gameObject.transform.forward.x * Time.deltaTime * velo, playerRB.velocity.y, meshPlayer.gameObject.transform.forward.z * Time.deltaTime * velo);
                    }
                }
                else
                {
                    velo *= Mathf.Abs(horizontalInput);
                    playerRB.velocity = new Vector3(meshPlayer.gameObject.transform.forward.x * Time.deltaTime * velo, playerRB.velocity.y, meshPlayer.gameObject.transform.forward.z * Time.deltaTime * velo);
                }
            }
            else
            {
                if (Cheats.Instance != null)
                {
                    if (Cheats.Instance.noClipCheat)
                    {
                        velo *= Mathf.Abs(verticalInput);
                        playerRB.velocity = new Vector3(Camera.main.transform.forward.x * Time.deltaTime * velo, Camera.main.transform.forward.y * Time.deltaTime * velo, Camera.main.transform.forward.z * Time.deltaTime * velo);
                    }
                    else
                    {
                        velo *= Mathf.Abs(verticalInput);
                        playerRB.velocity = new Vector3(meshPlayer.gameObject.transform.forward.x * Time.deltaTime * velo, playerRB.velocity.y, meshPlayer.gameObject.transform.forward.z * Time.deltaTime * velo);
                    }
                }
                else
                {
                    velo *= Mathf.Abs(verticalInput);
                    playerRB.velocity = new Vector3(meshPlayer.gameObject.transform.forward.x * Time.deltaTime * velo, playerRB.velocity.y, meshPlayer.gameObject.transform.forward.z * Time.deltaTime * velo);
                }
            }
        }

    }

    void GroundCheck()
    {
        grounded = false;
        RaycastHit hit;

        switch (playerState)
        {
            #region Case 1 - Idle state Checking

            case PlayerState.IDLE:
                grounded = false;
                if (Physics.Raycast(center.position, Vector3.down, out hit, isWalkable))
                {
                    grounded = Vector3.Distance(center.position, hit.point) < 0.7f;
                    groundY = hit.point.y;
                }

                playerRB.useGravity = !grounded;

                if (!grounded || Cheats.Instance.noClipCheat)
                {
                    playerState = PlayerState.FALL;
                }

                break;

            #endregion

            #region Case 2 - Walk state Checking

            case PlayerState.WALK:
                grounded = false;
                #region horizontal moviment checkers
                if (horizontalInput > 0 && verticalInput == 0)
                {
                    if (Physics.Raycast(right.position, Vector3.down, out hit, isWalkable))
                    {
                        grounded = Vector3.Distance(right.position, hit.point) < 0.7f; ;
                        groundY = hit.point.y;

                        if (Physics.Raycast(fVertRight.position, Vector3.down, out hit, isWalkable))
                        {
                            grounded = Vector3.Distance(fVertRight.position, hit.point) < 0.7f; ;
                            groundY = groundY > hit.point.y ? hit.point.y : groundY;
                        }

                        if (Physics.Raycast(bVertRight.position, Vector3.down, out hit, isWalkable))
                        {
                            grounded = Vector3.Distance(bVertRight.position, hit.point) < 0.7f; ;
                            groundY = groundY > hit.point.y ? hit.point.y : groundY;
                        }
                    }

                    playerRB.useGravity = !grounded;
                }
                else if (horizontalInput < 0 && verticalInput == 0)
                {
                    if (Physics.Raycast(left.position, Vector3.down, out hit, isWalkable))
                    {
                        grounded = Vector3.Distance(left.position, hit.point) < 0.7f;
                        groundY = hit.point.y;

                        if (Physics.Raycast(fVertLeft.position, Vector3.down, out hit, isWalkable))
                        {
                            grounded = Vector3.Distance(fVertLeft.position, hit.point) < 0.7f;
                            groundY = groundY > hit.point.y ? hit.point.y : groundY;
                        }
                        if (Physics.Raycast(bVertLeft.position, Vector3.down, out hit, isWalkable))
                        {
                            grounded = Vector3.Distance(bVertLeft.position, hit.point) < 0.7f;
                            groundY = groundY > hit.point.y ? hit.point.y : groundY;
                        }
                    }

                    playerRB.useGravity = !grounded;
                }

                #endregion

                #region vertical moviment checkers
                if (verticalInput > 0 && horizontalInput == 0)
                {
                    if (Physics.Raycast(fVertRight.position, Vector3.down, out hit, isWalkable))
                    {
                        grounded = Vector3.Distance(fVertRight.position, hit.point) < 0.7f;
                        groundY = hit.point.y;

                        if (Physics.Raycast(front.position, Vector3.down, out hit, isWalkable))
                        {
                            grounded = Vector3.Distance(front.position, hit.point) < 0.7f;
                            groundY = groundY > hit.point.y ? hit.point.y : groundY;
                        }
                        else if (Physics.Raycast(fVertLeft.position, Vector3.down, out hit, isWalkable))
                        {
                            grounded = Vector3.Distance(fVertLeft.position, hit.point) < 0.7f;
                            groundY = groundY > hit.point.y ? hit.point.y : groundY;
                        }
                    }

                    playerRB.useGravity = !grounded;
                }
                else if (verticalInput < 0 && horizontalInput == 0)
                {
                    if (Physics.Raycast(bVertRight.position, Vector3.down, out hit, isWalkable))
                    {
                        grounded = Vector3.Distance(bVertRight.position, hit.point) < 0.7f;
                        groundY = hit.point.y;

                        if (Physics.Raycast(back.position, Vector3.down, out hit, isWalkable))
                        {
                            grounded = Vector3.Distance(back.position, hit.point) < 0.7f;
                            groundY = groundY > hit.point.y ? hit.point.y : groundY;
                        }
                        if (Physics.Raycast(bVertLeft.position, Vector3.down, out hit, isWalkable))
                        {
                            grounded = Vector3.Distance(bVertLeft.position, hit.point) < 0.7f;
                            groundY = groundY > hit.point.y ? hit.point.y : groundY;
                        }
                    }

                    playerRB.useGravity = !grounded;
                }
                #endregion

                #region diagonal moviment checkers
                if (horizontalInput != 0 && verticalInput != 0)
                {
                    if (verticalInput > 0 && horizontalInput != 0)
                    {
                        if (Physics.Raycast(fVertRight.position, Vector3.down, out hit, isWalkable))
                        {
                            grounded = Vector3.Distance(fVertRight.position, hit.point) < 0.7f;
                            groundY = hit.point.y;

                            if (Physics.Raycast(front.position, Vector3.down, out hit, isWalkable))
                            {
                                grounded = Vector3.Distance(front.position, hit.point) < 0.7f;
                                groundY = groundY > hit.point.y ? hit.point.y : groundY;
                            }
                            if (Physics.Raycast(fVertLeft.position, Vector3.down, out hit, isWalkable))
                            {
                                grounded = Vector3.Distance(fVertLeft.position, hit.point) < 0.7f;
                                groundY = groundY > hit.point.y ? hit.point.y : groundY;
                            }
                        }

                        playerRB.useGravity = !grounded;
                    }
                    else if (verticalInput < 0 && horizontalInput != 0)
                    {
                        if (Physics.Raycast(bVertRight.position, Vector3.down, out hit, isWalkable))
                        {
                            grounded = Vector3.Distance(bVertRight.position, hit.point) < 0.7f;
                            groundY = hit.point.y;

                            if (Physics.Raycast(back.position, Vector3.down, out hit, isWalkable))
                            {
                                grounded = Vector3.Distance(back.position, hit.point) < 0.7f;
                                groundY = groundY > hit.point.y ? hit.point.y : groundY;
                            }
                            if (Physics.Raycast(bVertLeft.position, Vector3.down, out hit, isWalkable))
                            {
                                grounded = Vector3.Distance(bVertLeft.position, hit.point) < 0.7f;
                                groundY = groundY > hit.point.y ? hit.point.y : groundY;
                            }
                        }
                        playerRB.useGravity = !grounded;
                    }
                }
                #endregion


                if (!grounded || Cheats.Instance.noClipCheat)
                {
                    playerState = PlayerState.FALL;
                }

                break;
            #endregion

            #region Case 3 - Fall state Checking
            case PlayerState.FALL:

                if (Physics.Raycast(center.position, Vector3.down, out hit, isWalkable))
                {
                    grounded = Vector3.Distance(center.position, hit.point) < 0.7f;
                    groundY = hit.point.y;
                }

                playerRB.useGravity = !grounded;
                if (grounded || Cheats.Instance.noClipCheat)
                {
                    playerState = PlayerState.IDLE;
                }

                break;
            #endregion

            #region Case 4 - Jump state Checking
            case PlayerState.JUMPING:

                grounded = false;

                playerRB.useGravity = true;

                break;
                #endregion
        }

    } // Checa colisão com o chão através de ray cast para verificar em qual estrutura está sobreposto

    void UpdateRotation()
    {

        Quaternion directionRot = meshPlayer.gameObject.transform.rotation;

        if (verticalInput != 0 && horizontalInput == 0)
        {
            directionRot = Quaternion.Euler(0f, Mathf.Rad2Deg * Mathf.Atan2(horizontalInput, verticalInput), 0f);
            directionRot = directionRot * Camera.main.transform.rotation;
            directionRot.x = 0;
            directionRot.z = 0;
            meshPlayer.gameObject.transform.rotation = Quaternion.Slerp(meshPlayer.gameObject.transform.rotation, directionRot, veloRotate * Time.deltaTime);
            return;
        }

        if (horizontalInput != 0 && verticalInput == 0)
        {
            directionRot = Quaternion.Euler(0f, Mathf.Rad2Deg * Mathf.Atan2(horizontalInput, verticalInput), 0f);
            directionRot = directionRot * Camera.main.transform.rotation;
            directionRot.x = 0;
            directionRot.z = 0;
            meshPlayer.gameObject.transform.rotation = Quaternion.Slerp(meshPlayer.gameObject.transform.rotation, directionRot, veloRotate * Time.deltaTime);
            return;
        }

        if (verticalInput != 0 && horizontalInput != 0)
        {
            directionRot = Quaternion.Euler(0f, Mathf.Rad2Deg * Mathf.Atan2(horizontalInput, verticalInput), 0f);
            directionRot = directionRot * Camera.main.transform.rotation;
            directionRot.x = 0;
            directionRot.z = 0;
            meshPlayer.gameObject.transform.rotation = Quaternion.Slerp(meshPlayer.gameObject.transform.rotation, directionRot, veloRotate * Time.deltaTime);
            return;
        }

    }


    public void StealthInput()
    {
        if (blockCrouch) return;
        blockCrouch = true;
        if (!crouch)
        {
            crouch = true;
            return;
        }
        else
        {
            crouch = false;
            return;
        }
    }

    private void OnEnable()
    {
        inputMaster.Enable();
    }

    private void OnDisable()
    {
        inputMaster.Disable();
    }

}
