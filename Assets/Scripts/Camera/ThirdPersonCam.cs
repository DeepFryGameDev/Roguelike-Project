using UnityEngine;

// Purpose: Handles the camera positioning and rotation for the different camera modes as well as keeps camera looking behind player when they move around
// Directions: Attach to [Cameras] -> CameraHolder -> PlayerCam Transform
// Other notes: Should all be moved over to CameraManager eventually
// Tutorial followed: https://www.youtube.com/watch?v=UCwwn2q4Vys

public class ThirdPersonCam : MonoBehaviour
{
    Transform orientation; // The transform to be used to determine player's orientation
    public void SetOrientation(Transform orientation) { this.orientation = orientation; }

    Transform playerParent; // The transform of the [Player] object in the hierarchy
    public void SetPlayerParent(Transform playerParent) { this.playerParent = playerParent; }

    Transform playerObj; // Used with Vector3.slerp to move the camera's view with the player's view direction
    public void SetPlayerObj(Transform playerObj) { this.playerObj = playerObj; }

    Transform combatLookAt; // When currentStyle is 'Combat', this is the transform used to keep the camera focused forward
    public void SetCombatLookAt(Transform combatLookAt) { this.combatLookAt = combatLookAt; }

    float rotationSpeed = 7f; // Rotation speed of the camera as the player moves around

    CameraManager cameraManager; // Changes camera aim based on camera mode

    bool cameraSetupComplete; // Set to true when the camera variables have all been set up for game startup
    public void SetCameraSetupComplete(bool set) {cameraSetupComplete = set; }

    void Start()
    {
        cameraManager = FindObjectOfType<CameraManager>();
    }

    void Update()
    {
        if (cameraSetupComplete) UpdateRotations();
    }

    /// <summary>
    /// Keeps orientation and rotation of camera updated for each camera mode
    /// </summary>
    void UpdateRotations()
    {
        // rotate orientation
        Vector3 viewDir = playerParent.position - new Vector3(transform.position.x, playerParent.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        // rotate player object
        if (cameraManager.currentMode == EnumHandler.CameraModes.BASIC || cameraManager.currentMode == EnumHandler.CameraModes.TOPDOWN)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
            {
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
            }
        }
        else if (cameraManager.currentMode == EnumHandler.CameraModes.COMBAT) // When camera is on 'COMBAT' mode, it will always stay aimed in the direction the player is facing
        {
            Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = dirToCombatLookAt.normalized;

            playerObj.forward = dirToCombatLookAt.normalized;
        }
    }

    /// <summary>
    /// Simply sets the cursor's lockstate to Locked so it stays focused and hides it
    /// Should eventually be moved to some kind of system script to handle random tasks like this
    /// </summary>
    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
