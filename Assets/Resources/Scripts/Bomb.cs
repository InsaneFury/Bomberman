using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class Bomb : MonoBehaviour
{
    [Header("BombSettings")]
    public AudioClip explosionSound;
    public GameObject explosionPrefab;
    // This LayerMask makes sure the rays cast to check for free spaces only hits the blocks in the level
    public LayerMask levelMask;
    public int explosionSize = 1;

    Player player;

    bool exploded = false;

    void Start ()
    {
        player = Player.Get();
        explosionSize = player.sizeOfExplosion;
        Invoke ("Explode", player.timeToExplode); //Call Explode in 3 seconds
    }

    void Explode ()
    {
        //Explosion sound
        AudioSource.PlayClipAtPoint (explosionSound, transform.position);

        //Create a first explosion at the bomb position
        Instantiate (explosionPrefab, transform.position, Quaternion.identity);

        //For every direction, start a chain of explosions
        StartCoroutine (CreateExplosions (Vector3.forward));
        StartCoroutine (CreateExplosions (Vector3.right));
        StartCoroutine (CreateExplosions (Vector3.back));
        StartCoroutine (CreateExplosions (Vector3.left));

        GetComponent<MeshRenderer> ().enabled = false; //Disable mesh
        exploded = true;
        player.currentBombs++;
        transform.Find ("Collider").gameObject.SetActive (false); //Disable the collider
        Destroy (gameObject, .3f); //Destroy the actual bomb in 0.3 seconds, after all coroutines have finished
    }

    public void OnTriggerEnter (Collider other)
    {
        if (!exploded && other.CompareTag("Explosion"))
        { //If not exploded yet and this bomb is hit by an explosion...
            CancelInvoke("Explode"); //Cancel the already called Explode, else the bomb might explode twice 
            Explode(); //Finally, explode!
        }
    }

    private IEnumerator CreateExplosions (Vector3 direction)
    {
        for (int i = 1; i < explosionSize; i++)
        { //The explosionSize here dictates how far the raycasts will check, in this case 1 tiles far
            RaycastHit hit; //Holds all information about what the raycast hits

            Physics.Raycast(transform.position, direction, out hit, i, levelMask); //Raycast in the specified direction at i distance, because of the layer mask it'll only hit blocks, not players or bombs

            if (!hit.collider)
            { // Free space, make a new explosion
                Instantiate(explosionPrefab, transform.position + (i * direction), explosionPrefab.transform.rotation);
            }
            else
            { //Hit a block, stop spawning in this direction
                break;
            }
            yield return new WaitForSeconds (.05f); //Wait 50 milliseconds before checking the next location
        }
    }
}
