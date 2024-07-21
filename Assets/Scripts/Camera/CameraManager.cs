using UnityEngine;

// Purpose: Handles camera style management
// Directions: Attach to [Cameras] Transform
// Other notes: 

public class CameraManager : MonoBehaviour
{
    [Tooltip("The default camera mode")]
    public EnumHandler.CameraModes currentMode;

    ThirdPersonCam thirdPersonCam; // Set to the primary player camera that contains Cinemachine Brain
    public ThirdPersonCam GetThirdPersonCam() { return thirdPersonCam; }

    [SerializeField] GameObject freeLookCam; // Set to the camera used for third person view
    public GameObject GetFreeLookCam () { return freeLookCam; }

    [SerializeField] GameObject combatCam; // Set to the camera used for Combat camera style
    public GameObject GetCombatCam() { return combatCam; }

    [SerializeField] GameObject topDownCam; // Set to the camera used for top-down camera style
    public GameObject GetTopDownCam() { return topDownCam; }

    Animator playerAnim; // Used to switch between Combat and Basic animations for each camera mode

    public static CameraManager instance; // This keeps the script persisting across scenes

    bool camerasSet; // Set to true when all camera variables have been set for game creation
    public void SetCamerasSet(bool set) {  camerasSet = set; }
    public bool GetCamerasSet() { return camerasSet; }

    void Awake()
    {
        Singleton();
    }

    void Singleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //SwitchCameraMode(currentMode);
    }

    private void Update()
    {
        SetCameraMode(); // Likely will be removed eventually, only used to switch camera modes from pressing 1-3 for testing
    }

    /// <summary>
    /// Disables all cameras and sets new camera mode from given argument
    /// </summary>
    public void SwitchCameraMode(EnumHandler.CameraModes newMode)
    {
        if (camerasSet)
        {
            //Debug.Log("Setting camera mode");
            if (playerAnim == null)
            {
                playerAnim = GameObject.FindWithTag("Player").GetComponentInChildren<Animator>();
            }

            combatCam.SetActive(false);
            freeLookCam.SetActive(false);
            topDownCam.SetActive(false);

            playerAnim.SetTrigger("camResetTrigger");

            switch (newMode)
            {
                case EnumHandler.CameraModes.BASIC:
                    freeLookCam.SetActive(true);
                    playerAnim.SetTrigger("basicCamTrigger");
                    break;
                case EnumHandler.CameraModes.COMBAT:
                    combatCam.SetActive(true);
                    playerAnim.SetTrigger("combatCamTrigger");
                    break;

                case EnumHandler.CameraModes.TOPDOWN:
                    topDownCam.SetActive(true);
                    break;
            }

            currentMode = newMode;
        }
    }             
    

    /// <summary>
    /// Sets style of camera control based on pressing 1-3 during gameplay
    /// </summary>
    void SetCameraMode()
    {
        // switch styles
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchCameraMode(EnumHandler.CameraModes.BASIC);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchCameraMode(EnumHandler.CameraModes.COMBAT);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchCameraMode(EnumHandler.CameraModes.TOPDOWN);
    }
}
