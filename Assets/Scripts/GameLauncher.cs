using UnityEngine;

public class GameLauncher : MonoBehaviour 
{
    [Header("UI Canvas Groups")]
    [Tooltip("The Main Menu / Start Screen object")]
    public GameObject startMenuCanvas;
    
    [Tooltip("The HUD (Timer, Money, Crosshair) object")]
    public GameObject hudCanvas;
    
    [Tooltip("The Summary / Manual / End Screen object")]
    public GameObject summaryPanel;

    [Header("Player Settings")]
    [Tooltip("Drag your Player Controller script or Player object here")]
    public MonoBehaviour playerController;

    private DayManager dayManager;

    void Awake()
    {
        // Find the DayManager in the scene automatically
        dayManager = Object.FindAnyObjectByType<DayManager>();
        
        // Ensure the game world is frozen at the very start
        Time.timeScale = 0f;
    }

    void Start()
    {
        // INITIAL STATE: This runs the moment the scene loads
        // We force everything to the correct 'Waiting' state
        if (startMenuCanvas != null) startMenuCanvas.SetActive(true);
        if (hudCanvas != null) hudCanvas.SetActive(false);
        
        // CRITICAL FIX: Force the manual/summary OFF immediately
        if (summaryPanel != null) summaryPanel.SetActive(false);

        // Show and unlock the mouse for the menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Keep the player from moving while in the menu
        if (playerController != null) playerController.enabled = false;
    }

    /// <summary>
    /// This function is called by your PLAY BUTTON's OnClick event
    /// </summary>
    public void PlayGame()
    {
        Debug.Log("GameLauncher: Play Button Clicked. Initializing Gameplay...");

        // 1. RESUME THE WORLD
        Time.timeScale = 1f;

        // 2. UI SWAP
        // Hide the Start Menu
        if (startMenuCanvas != null) startMenuCanvas.SetActive(false);
        
        // Show the Gameplay HUD
        if (hudCanvas != null) hudCanvas.SetActive(true);
        
        // DOUBLE-CHECK: Force the summary/manual to stay OFF
        if (summaryPanel != null) 
        {
            summaryPanel.SetActive(false);
            Debug.Log("GameLauncher: Summary Panel forced to INACTIVE.");
        }

        // 3. MOUSE & PLAYER CONTROL
        // Lock the cursor for gameplay (FPS style)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Enable player movement/interaction
        if (playerController != null) playerController.enabled = true;
        
        // 4. START THE GAME LOGIC
        if (dayManager != null) 
        {
            dayManager.StartFirstDay();
        }
        else
        {
            Debug.LogWarning("GameLauncher: No DayManager found! Money and Timer might not start.");
        }
    }
}