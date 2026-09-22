using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance;

    [Header("UI Panels")]
    public GameObject startScreenPanel; 
    public GameObject gameHUDPanel;     // Drag the Panel containing Timer/Money here
    public GameObject summaryPanel;    

    [Header("Money Tracking")]
    public float totalMoney = 0f;
    public float shiftMoney = 0f;
    public TextMeshProUGUI gameHUDMoneyText;

    [Header("Timer Settings")]
    public float shiftDuration = 60f;
    public TextMeshProUGUI timerText; 
    private float timer;
    private bool isShiftActive = false;

    [Header("Summary Details")]
    public TextMeshProUGUI totalMoneyReportText;
    public TextMeshProUGUI shiftMoneyReportText;
    public TextMeshProUGUI dayCounterText;
    public Button nextDayButton;

    private int dayCount = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Freeze time immediately so the menu works
        Time.timeScale = 0f;
    }

    void Start()
    {
        // Force time to 0 so the menu stays up
    Time.timeScale = 0f;
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;

    if (nextDayButton != null) 
        nextDayButton.onClick.AddListener(StartNewShift);
    
    Debug.Log("DayManager Initialized. Waiting for Start Button...");
    }

    public void StartFirstDay()
    {
      Debug.Log("STEP 1: Button Click Received!");

    // 1. Hide Menu
    if (startScreenPanel != null)
    {
        startScreenPanel.SetActive(false);
        Debug.Log("STEP 2: Start Screen Hidden.");
    }
    else { Debug.LogError("ERROR: Start Screen Panel slot is EMPTY!"); }

    // 2. Show HUD
    if (gameHUDPanel != null)
    {
        gameHUDPanel.SetActive(true);
        Debug.Log("STEP 3: HUD Shown.");
    }

    // 3. Unfreeze Game
    Time.timeScale = 1f; 
    isShiftActive = true;
    dayCount = 1;
    
    // 4. Reset Timer
    timer = shiftDuration;

    // 5. Mouse Control
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;

    Debug.Log("STEP 4: Game Unfrozen. Timer should be running.");
    }

    void Update()
    {
        if (!isShiftActive) return;

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            UpdateTimerUI(); 
        }
        else
        {
            timer = 0;
            UpdateTimerUI();
            EndShift();
        }
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            float minutes = Mathf.FloorToInt(timer / 60);
            float seconds = Mathf.FloorToInt(timer % 60);
            timerText.text = string.Format("Time: {0:0}:{1:00}", minutes, seconds);
        }
    }

    public void AddMoney(float amount)
    {
        shiftMoney += amount;
        totalMoney += amount;
        UpdateHUD();
    }

    void UpdateHUD()
    {
        if (gameHUDMoneyText != null)
            gameHUDMoneyText.text = "$" + totalMoney.ToString("F2");
    }

    void EndShift()
    {
        isShiftActive = false;
        Time.timeScale = 0f; 
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (gameHUDPanel != null) gameHUDPanel.SetActive(false);
        if (summaryPanel != null)
        {
            summaryPanel.SetActive(true);
            shiftMoneyReportText.text = "Shift Earnings: $" + shiftMoney.ToString("F2");
            totalMoneyReportText.text = "Total Bank: $" + totalMoney.ToString("F2");
            dayCounterText.text = "Day " + dayCount + " Complete";
        }
    }

    public void StartNewShift()
    {
        if (isShiftActive && timer <= 0) dayCount++; 
        
        shiftMoney = 0f;
        timer = shiftDuration;
        isShiftActive = true;

        if (summaryPanel != null) summaryPanel.SetActive(false);
        if (gameHUDPanel != null) gameHUDPanel.SetActive(true);
        
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        UpdateHUD();
    }
}