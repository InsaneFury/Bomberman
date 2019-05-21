using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Player : SingletonMonobehaviour<Player>
{
    public delegate void OnPlayerAction(Player player);
    public OnPlayerAction OnPlayerDie;
    public OnPlayerAction OnPlayerDropBomb;

    [Header("PlayerSettings")]
    public int startLives = 2;
    public float moveSpeed = 5f;
    public bool canMove = true;
    public bool dead = false;
    public float snapSpeed = 2f;
    public float snapTime = 2f;
    public float gridSize;

    [Header("PlayerBombsSettings")]
    public bool canDropBombs = true;
    public int maxBombsAtSameTime = 1;
    public int sizeOfExplosion = 1;
    public float timeToExplode = 3f;
    public int currentBombs = 1;

    public GameObject bombPrefab;

    [HideInInspector]
    public int lives;
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
        lives = startLives;
        currentBombs = maxBombsAtSameTime;
        rigidBody = GetComponent<Rigidbody>();
        myTransform = transform;
        animator = GetComponent<Animator>();
        OnPlayerDie += Die;
        OnPlayerDropBomb += DropBomb;
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
        PlayerInputs();  
    }

    /// <summary>
    /// Player's movement and facing rotation using the WASD keys and drops bombs using Space
    /// </summary>
    private void PlayerInputs()
    {
        if (Input.GetKey(KeyCode.W))
        { //Up movement
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, moveSpeed);
            myTransform.rotation = Quaternion.Euler(0, 0, 0);
            animator.SetBool("Run", true);
        }
        else if (Input.GetKey(KeyCode.S))
        { //Down movement
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, -moveSpeed);
            myTransform.rotation = Quaternion.Euler(0, 180, 0);
            animator.SetBool("Run", true);
        }
        else if (Input.GetKey(KeyCode.A))
        { //Left movement
            rigidBody.velocity = new Vector3(-moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler(0, 270, 0);
            animator.SetBool("Run", true);
        }
        else if (Input.GetKey(KeyCode.D))
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
                PlayerDropBomb();
            }
        }
    }

    void PlayerDropBomb()
    {
        if(OnPlayerDropBomb != null){
            OnPlayerDropBomb(this);
        }
    }

    private void DropBomb(Player p)
    {
        if (bombPrefab)
        { //Check if bomb prefab is assigned first
            // Create new bomb and snap it to a tile
           Instantiate(bombPrefab,
                new Vector3(Mathf.RoundToInt(myTransform.position.x), 
                bombPrefab.transform.position.y, 
                Mathf.RoundToInt(myTransform.position.z)),
                bombPrefab.transform.rotation);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!dead && other.CompareTag("Explosion"))
        {
            Debug.Log("Player hit by explosion!");
            dead = true;
            playerDie();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!dead && collision.collider.CompareTag("Enemy"))
        {
            Debug.Log("Player hit by Enemy!");
            dead = true;
            playerDie();
        }
    }

    void Die(Player player)
    {
        lives--;
    }

    public void Revive()
    {
        dead = false;
    }

    void playerDie()
    {
        if (OnPlayerDie != null)
        {
            OnPlayerDie(this);
        }
    }

    public void StopPlayer()
    {
        canMove = false;
        canDropBombs = false;
    }

    public void ResetPlayer()
    {
        canMove = true;
        canDropBombs = true;
    }

    void SnapPlayer()
    {
        //Snaping player to grid
        Vector3 roundPos = new Vector3(
        Mathf.RoundToInt(myTransform.localPosition.x),
        myTransform.localPosition.y,
        Mathf.RoundToInt(myTransform.localPosition.z));
        Vector3 roundedFinalPos =  Vector3.Lerp(myTransform.localPosition, roundPos,snapSpeed * Time.deltaTime);
        myTransform.localPosition = roundedFinalPos;
    }
}
