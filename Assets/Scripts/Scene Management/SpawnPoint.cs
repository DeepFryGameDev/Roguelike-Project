using System.Collections;
using UnityEngine;

// Purpose: Script to determine where player should spawn into each scene
// Directions: Create an GameObject called SpawnPoint and attach this to it.  The object should be in the same position and rotation the player should be spawned in
// Other notes:

public class SpawnPoint : MonoBehaviour
{
    GameObject player; // Used to set position and rotation for the player

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");

        StartCoroutine(SetSpawn());
    }

    /// <summary>
    /// Coroutine, as a workaround to move the player's position/rotation without affecting the ridigbody
    /// Works by setting rigidbody as kinematic for one frame, and sets the position/rotation during that frame
    /// </summary>
    IEnumerator SetSpawn()
    {
        //Debug.Log("Player should spawn at " + transform.position + " with rotation " + transform.eulerAngles);

        player.GetComponent<Rigidbody>().isKinematic = true;
        player.transform.position = transform.position;
        player.transform.rotation = transform.rotation;

        yield return new WaitForEndOfFrame();

        player.GetComponent<Rigidbody>().isKinematic = false;

        //Debug.Log("Players position: " + player.transform.position);
    }
}
