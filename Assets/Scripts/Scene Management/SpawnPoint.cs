using System.Collections;
using UnityEngine;

// Purpose: Script to determine where player should spawn into each scene
// Directions: Create an GameObject called SpawnPoint and attach this to it.  This object should be in the same position and rotation the player should be spawned in
// Other notes:

public class SpawnPoint : MonoBehaviour
{
    GameObject player; // Used to set position and rotation for the player

    private void Awake() // Ensures this is not ran while player is being generated
    {
        if (GameManager.GetGameSet())
        {
            player = GameObject.FindWithTag("Player");

            StartCoroutine(SetSpawn(player));
        }      
    }

    /// <summary>
    /// Coroutine, as a workaround to move the player's position/rotation without affecting the ridigbody
    /// Works by setting rigidbody as kinematic for one frame, and sets the position/rotation during that frame
    /// </summary>
    /// <param name="playerObj">The object of the player to be spawned</param>
    public IEnumerator SetSpawn(GameObject playerObj)
    {
        //Debug.Log("Player should spawn at " + transform.position + " with rotation " + transform.eulerAngles);

        Rigidbody rb = playerObj.GetComponentInChildren<Rigidbody>();

        rb.isKinematic = true;
        playerObj.transform.position = transform.position;
        playerObj.transform.rotation = transform.rotation;

        yield return new WaitForEndOfFrame();

        rb.isKinematic = false;

        //Debug.Log("Players position: " + playerObj.transform.position);
    }
}
