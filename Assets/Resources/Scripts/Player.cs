using UnityEngine;
using System.Collections;
using System;

public class Player : SingletonMonobehaviour<Player>
{
    //Manager
    //public GlobalStateManager globalManager;

    [Header("PlayerSettings")]
    public float moveSpeed = 5f;
    public bool canMove = true;
    public bool dead = false;
    public float snapSpeed = 2f;
    public float gridSize;
    [Header("PlayerBombsSettings")]
    public bool canDropBombs = true;
    public int maxBombsAtSameTime = 1;
    [HideInInspector]
    public int currentBombs = 1;

    public GameObject bombPrefab;

    Rigidbody rigidBody;
    Transform myTransform;
    Animator animator;

    Vector3 truePos;

    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        currentBombs = maxBombsAtSameTime;
        rigidBody = GetComponent<Rigidbody>();
        myTransform = transform;
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        animator.SetBool("Run", false); //Resets walking animation to idle

        if (!canMove)
        { //Return if player can't move
            return;
        }
        UpdatePlayer1Movement();  
    }

    /// <summary>
    /// Updates Player 1's movement and facing rotation using the WASD keys and drops bombs using Space
    /// </summary>
    private void UpdatePlayer1Movement()
    {
        if (Input.GetKey(KeyCode.W))
        { //Up movement
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, moveSpeed);
            myTransform.rotation = Quaternion.Euler(0, 0, 0);
            animator.SetBool("Run", true);
        }

        if (Input.GetKey(KeyCode.A))
        { //Left movement
            rigidBody.velocity = new Vector3(-moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler(0, 270, 0);
            animator.SetBool("Run", true);
        }

        if (Input.GetKey(KeyCode.S))
        { //Down movement
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, -moveSpeed);
            myTransform.rotation = Quaternion.Euler(0, 180, 0);
            animator.SetBool("Run", true);
        }

        if (Input.GetKey(KeyCode.D))
        { //Right movement
            rigidBody.velocity = new Vector3(moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler(0, 90, 0);
            animator.SetBool("Run", true);
        }

        if (currentBombs > 0)
        {
            if (canDropBombs && Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                currentBombs--;
                DropBomb();
            }
        }

        SnapPlayer();
    }

    private void DropBomb()
    {
        if (bombPrefab)
        { //Check if bomb prefab is assigned first
            // Create new bomb and snap it to a tile
            Instantiate(bombPrefab,
                new Vector3(Mathf.RoundToInt(myTransform.position.x), bombPrefab.transform.position.y, Mathf.RoundToInt(myTransform.position.z)),
                bombPrefab.transform.rotation);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!dead && other.CompareTag("Explosion"))
        {
            Debug.Log("Player hit by explosion!");

            dead = true;
            Destroy(gameObject);
        }
    }

    void SnapPlayer()
    {
        //Snaping player to grid
        Vector3 roundPos = new Vector3(
            Mathf.Floor(myTransform.localPosition.x * 100) / 100,
            Mathf.Floor(myTransform.localPosition.y * 100) / 100,
            Mathf.Floor(myTransform.localPosition.z * 100) / 100);

        Vector3 roundedFinalPos =  Vector3.Lerp(myTransform.localPosition, roundPos,snapSpeed * Time.deltaTime);
        myTransform.localPosition = roundedFinalPos;
    }
}
