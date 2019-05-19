using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("EnemySettings")]
    public float moveSpeed = 5f;
    public bool canMove = true;
    public bool dead = false;
    public float snapSpeed = 2f;
    public float gridSize;
    public LayerMask layerMask;
    public float rayDistance = 5f;

    Rigidbody rigidBody;
    Transform myTransform;

    enum Directions
    {
        Up,
        Down,
        Right,
        Left
    }
    Directions direction;

    void Start()
    {
        direction = (Directions)Mathf.RoundToInt(Random.Range(0f, 3f));
        rigidBody = GetComponent<Rigidbody>();
        myTransform = transform;
    }

    void FixedUpdate()
    {
        if (CheckIfIsEmpty())
        {
            Move();
        } 
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!dead && other.CompareTag("Explosion"))
        {
            Debug.Log("Enemy hit by explosion!");

            dead = true;
            Destroy(gameObject);
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
                rigidBody.velocity = new Vector3(-moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
                myTransform.rotation = Quaternion.Euler(0, 270, 0);
                break;
            case Directions.Right:
                rigidBody.velocity = new Vector3(moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
                myTransform.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case Directions.Left:
                rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, -moveSpeed);
                myTransform.rotation = Quaternion.Euler(0, 180, 0);
                break;
        }
       // SnapPlayer();
    }

    void SnapPlayer()
    {
        //Snaping player to grid
        Vector3 roundPos = new Vector3(
            Mathf.Floor(myTransform.localPosition.x),
            Mathf.Floor(myTransform.localPosition.y),
            Mathf.Floor(myTransform.localPosition.z));

        Vector3 roundedFinalPos = Vector3.Lerp(myTransform.localPosition, roundPos, snapSpeed * Time.deltaTime);
        myTransform.localPosition = roundedFinalPos;
    }

   bool CheckIfIsEmpty()
   {
        RaycastHit hit;

        if (Physics.Raycast(myTransform.localPosition, myTransform.forward, out hit, rayDistance, layerMask))
        {
            rigidBody.isKinematic = true;
            rigidBody.isKinematic = false;
            Debug.DrawRay(transform.localPosition, myTransform.forward * hit.distance, Color.yellow);
            if (direction == Directions.Up)
            {
                direction = Directions.Down;
            }
            else if (direction == Directions.Down)
            {
                direction = Directions.Up;
            }
            else if (direction == Directions.Right)
            {
                direction = Directions.Left;
            }
            else if (direction == Directions.Left)
            {
                direction = Directions.Right;
            }
            Debug.Log(direction);
            return false;
        }
        else
        {
            Debug.DrawRay(transform.localPosition, myTransform.forward * hit.distance, Color.white);
            return true;
        }
   }
}
