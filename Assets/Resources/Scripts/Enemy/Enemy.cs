using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public delegate void OnEnemyAction(Enemy e);
    public static OnEnemyAction OnEnemyDie;

    [Header("EnemySettings")]
    public float moveSpeed = 5f;
    public bool canMove = true;
    public bool dead = false;
    public float snapSpeed = 2f;
    public float gridSize;
    public LayerMask layerMask;
    public Directions startDirection;

    public int score = 500;
    bool onCollision = false;
    Rigidbody rigidBody;
    Transform myTransform;

    public enum Directions
    {
        Up,
        Down,
        Right,
        Left
    }
    Directions direction;

    void Start()
    {
        canMove = true;
        direction = startDirection;
        rigidBody = GetComponent<Rigidbody>();
        myTransform = transform;
        OnEnemyDie += DisableEnemy;
    }

    void FixedUpdate()
    {
        Move();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!dead && other.CompareTag("Explosion"))
        {
            Debug.Log("Enemy hit by explosion!");

            dead = true;
            Die();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!onCollision)
        {
            ChangeDirection();
            onCollision = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (onCollision)
        {
            onCollision = false;
        }
    }

    void Move()
    {
        if (!canMove)
        {
            return;
        }

        switch (direction)
        {
            case Directions.Up:
                rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, moveSpeed);
                myTransform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case Directions.Down:
                rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, -moveSpeed);
                myTransform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case Directions.Right:
                rigidBody.velocity = new Vector3(moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
                myTransform.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case Directions.Left:
                rigidBody.velocity = new Vector3(-moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
                myTransform.rotation = Quaternion.Euler(0, 270, 0);

                break;
        }
        //SnapPlayer();
    }

    void ChangeDirection()
    {
        if (direction == Directions.Up)
        {
            direction = Directions.Down;

        }
        else if (direction == Directions.Down)
        {
            direction = Directions.Up;
        }
        else if(direction == Directions.Right)
        {
            direction = Directions.Left;
        }
        else if(direction == Directions.Left)
        {
            direction = Directions.Right;
        }
    }

    void Die()
    {
        if (OnEnemyDie != null)
        {
            OnEnemyDie(this);
        }
    }

    void DisableEnemy(Enemy e)
    {
        e.gameObject.SetActive(false);
    }

    void SnapPlayer()
    {
        //Snaping player to grid
        Vector3 roundPos = new Vector3(
            Mathf.RoundToInt(myTransform.localPosition.x),
            myTransform.localPosition.y,
            Mathf.RoundToInt(myTransform.localPosition.z));

        Vector3 roundedFinalPos = Vector3.Lerp(myTransform.localPosition, roundPos, snapSpeed * Time.deltaTime);
        myTransform.localPosition = roundedFinalPos;
    }
}
