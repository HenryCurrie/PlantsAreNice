using UnityEngine;
using TMPro; // Needed for the UI text

public class GameManager : MonoBehaviour
{
    // 1. The Singleton - This allows CustomerAI to say "GameManager.instance"
    public static GameManager instance;

    [Header("Economy Settings")]
    public int currentMoney = 0;
    
    [Header("UI References")]
    public TextMeshProUGUI moneyText; // Drag your UI text here in the Inspector

    void Awake()
    {
        // Setup the Singleton
        if (instance == null)
        {
            instance = this;
            // Optional: Keep the manager alive if you change scenes
            // DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateMoneyUI();
    }

    // 2. The function the Customer calls
    public void AddMoney(int amount)
    {
        currentMoney += amount;
        Debug.Log("Sale completed! Total Money: $" + currentMoney);
        UpdateMoneyUI();
    }

    // 3. Keep the screen updated
    void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = "Cash: $" + currentMoney.ToString();
        }
    }
}