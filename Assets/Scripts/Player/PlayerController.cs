using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Animations;
using UnityEngine.AI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float playerHeight = 2f;

    [Header("Movement")]
    public float moveSpeed = 6f;

    [Header("Gizmos")]
    public bool DrawGizmos = false;

    [Header("Enemy Detection")]
    public float DetectRadius = 12f;
    public float MinimumRadius = 2f;
    public LayerMask EnemyLayer;
    public Transform target;
    public string enemyTag;
    Vector3 direction;
    public float distanceToTarget;

    [Header("Jumping")]
    public float jumpForce = 5f;

    [Header("Exp")]
    public float Level = 0;
    public float Exp = 0f;
    public ExpBar expBar;
    public Transform UpgradeBar;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    [Header("Time Speed")]
    public float TimeSpeed = 1;

    [Header("Target Rotation")]
    public float FacingRotationSpeed = 12f;
    [Tooltip("if the variable set to true, Enemy will look directly on target(and rotate it self in target origin position)")]
    public bool lookAtTarget = false;

    [Header("Drag")]
    [Tooltip("Default 6f")]
    public float groundDrag = 6f;
    [Tooltip("Default 2f")]
    public float airDrag = 2f;

    [Header("Rotation")]
    public float movementRotationSpeed = 0.08f;

    float horizontalMovement;
    float verticalMovement;

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    public Transform groundEdgeCheck;
    public Transform groundEdgeOrigin;
    public bool isGroundEdgeCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance = 0.4f;
    public bool isGrounded;
    public Transform WeaponManager;

    public float ladderRange;
    public bool isLadderCheck;
    public LayerMask LadderMask;
    public Transform ladderOrigin;

    public Transform targetGrappling;

    public Transform startRotation;

    public Camera playerCamera;

    public Transform playerGFX;

    public Transform PlayerCenter;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    public AmmoCounter ammoCounter;

    public TextMeshProUGUI lvl1;

    Rigidbody rb;

    RaycastHit slopeHit;

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        InvokeRepeating("UpdateTarget", 0f, 0.5f);

        InvokeRepeating("UpdateGrapplingPoints", 0f, 1f);

        Level = 1;

        UpgradeBar.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <= shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= DetectRadius)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    void UpdateGrapplingPoints()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target Grappling");
        float shortestDistance1 = Mathf.Infinity;
        GameObject nearestTarget = null;
        foreach (GameObject target in targets)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, target.transform.position);
            if (distanceToEnemy <= shortestDistance1)
            {
                shortestDistance1 = distanceToEnemy;
                nearestTarget = target;
            }
        }

        if (nearestTarget != null && shortestDistance1 <= DetectRadius)
        {
            targetGrappling = nearestTarget.transform;
        }
        else
        {
            targetGrappling = null;
        }
    }

    void Update()
    {
        if (target != null)
        {
            distanceToTarget = Vector3.Distance(target.position, transform.position);
        }

        if (targetGrappling != null)
        {
            if (targetGrappling.position.y >= PlayerCenter.transform.position.y)
            {
                GrapplingPoint1 tg1 = targetGrappling.GetComponent<GrapplingPoint1>();
                tg1.Show();
            }
        }

        if (isLadderCheck != true)
        {
            MyInput();
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        isGroundEdgeCheck = Physics.CheckSphere(groundEdgeCheck.position, 0.25f, groundMask);

        isLadderCheck = Physics.CheckSphere(ladderOrigin.position, ladderRange, LadderMask);

        if (isLadderCheck == true)
        {
            isGrounded = false;

            rb.drag = airDrag;

            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        ControlDrag();

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);

        //Enemy Detection

        if (target != null)
        {
            Vector3 fromPosition = PlayerCenter.transform.position;
            Vector3 toPosition = target.transform.position;
            direction = toPosition - fromPosition;
        }

        RaycastHit hit;
        if (Physics.Raycast(PlayerCenter.transform.position, direction, out hit, Mathf.Infinity, EnemyLayer))
        {
            Debug.DrawRay(PlayerCenter.transform.position, direction * hit.distance, Color.red);

            if (target != null)
            {
                FacingTarget(target);
            }

            WeaponManager.LookAt(hit.point);
        }

        transform.rotation = Quaternion.identity;

        if (isGrounded == true)
        {
            rb.drag = groundDrag;
        }

        if (Level == 1)
        {
            expBar.SetMaxExp(15f);
            lvl1.text = "1";
        }
        else if (Level == 2)
        {
            expBar.SetMaxExp(20f);
            lvl1.text = "2";
        }
        else if (Level == 3)
        {
            expBar.SetMaxExp(25f);
            lvl1.text = "3";
        }
        else if (Level == 4)
        {
            expBar.SetMaxExp(30f);
            lvl1.text = "4";
        }

        if (Exp == 15)
        {
            Invoke("LevelUp", 0f);
        }
        else if (Exp == 20)
        {
            Invoke("LevelUp", 0f);
        }
        else if (Exp == 25)
        {
            Invoke("LevelUp", 0f);
        }
        else if (Exp == 30)
        {
            Invoke("LevelUp", 0f);
        }
    }

    public void LevelUp()
    {
        UpgradeBar.gameObject.SetActive(true);
        Level += 1f;
        Exp = 0f;
        expBar.SetExp(Exp);
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void TakeExp()
    {
        Exp += 1f;
        expBar.SetExp(Exp);
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void FacingTarget(Transform target1)
    {
        Vector3 direction = (target1.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        playerGFX.transform.rotation = Quaternion.Slerp(playerGFX.transform.rotation, lookRotation, Time.deltaTime * FacingRotationSpeed);
    }

    void MyInput()
    {
        horizontalMovement = -Input.GetAxisRaw("Horizontal");
        verticalMovement = -Input.GetAxisRaw("Vertical");

        moveDirection = transform.forward * verticalMovement + transform.right * horizontalMovement;

    }

    private void FixedUpdate()
    {
        if (Input.GetKey(jumpKey) && isGrounded == true && !isGroundEdgeCheck)
        {
            rb.drag = airDrag;

            Jump();
        }

        MyInput();
        MovePlayer();
    }

    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    void MovePlayer()
    {
        if (isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 5, ForceMode.Acceleration);
        }
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * 10, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 5 * 0.2f, ForceMode.Acceleration);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (DrawGizmos == true)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, DetectRadius);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, MinimumRadius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(ladderOrigin.position, ladderRange);
        }
    }
}
