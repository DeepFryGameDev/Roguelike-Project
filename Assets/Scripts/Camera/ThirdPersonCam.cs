using UnityEngine;

// Purpose: Handles the camera positioning and rotation for the different camera modes as well as keeps camera looking behind player when they move around
// Directions: Attach to [Cameras] -> CameraHolder -> PlayerCam Transform
// Other notes: Should all be moved over to CameraManager eventually

public class ThirdPersonCam : MonoBehaviour
{
    [Tooltip("The transform to be used to determine player's orientation")]
    [SerializeField] Transform orientation;

    [Tooltip("The transform of the player, used to calculate view direction")]
    [SerializeField] Transform player;

    [Tooltip("Used with Vector3.slerp to move the camera's view with the player's view direction")]
    [SerializeField] Transform playerObj;

    [Tooltip("When currentStyle is 'Combat', this is the transform used to keep the camera focused forward")]
    [SerializeField] Transform combatLookAt;

    [Tooltip("Set to the camera used for third person view")]
    [SerializeField] GameObject thirdPersonCam;

    [Tooltip("Set to the camera used for Combat camera style")]
    [SerializeField] GameObject combatCam;

    [Tooltip("Set to the camera used for top-down camera style")]
    [SerializeField] GameObject topDownCam;

    //public Rigidbody rb;

    [Tooltip("Rotation speed of the camera as the player moves around")]
    [SerializeField] float rotationSpeed;

    [Tooltip("The default camera mode")]
    public EnumHandler.CameraModes currentMode;
    

    void Start()
    {
        HideCursor();
    }

    /// <summary>
    /// Simply sets the cursor's lockstate to Locked so it stays focused and hides it
    /// </summary>
    void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        SetCameraMode();
        UpdateRotations();
    }

    /// <summary>
    /// Keeps orientation and rotation of camera updated for each camera mode
    /// </summary>
    void UpdateRotations()
    {
        // rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        // rotate player object
        if (currentMode == EnumHandler.CameraModes.BASIC || currentMode == EnumHandler.CameraModes.TOPDOWN)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
            {
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
            }
        }
        else if (currentMode == EnumHandler.CameraModes.COMBAT)
        {
            Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = dirToCombatLookAt.normalized;

            playerObj.forward = dirToCombatLookAt.normalized;
        }
    }

    /// <summary>
    /// Sets style of camera control
    /// </summary>
    void SetCameraMode()
    {
        // switch styles
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchCameraMode(EnumHandler.CameraModes.BASIC);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchCameraMode(EnumHandler.CameraModes.COMBAT);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchCameraMode(EnumHandler.CameraModes.TOPDOWN);
    }

    /// <summary>
    /// Disables all cameras and sets new camera mode from given argument
    /// </summary>
    void SwitchCameraMode(EnumHandler.CameraModes newMode)
    {
        combatCam.SetActive(false);
        thirdPersonCam.SetActive(false);
        topDownCam.SetActive(false);

        if (newMode == EnumHandler.CameraModes.BASIC) thirdPersonCam.SetActive(true);
        if (newMode == EnumHandler.CameraModes.COMBAT) combatCam.SetActive(true);
        if (newMode == EnumHandler.CameraModes.TOPDOWN) topDownCam.SetActive(true);

        currentMode = newMode;
    }
}
