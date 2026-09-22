using UnityEngine;
using TMPro;

public class PlantHealth : MonoBehaviour
{
    public string[] issues = { 
        "My plant has mold!", 
        "The leaves are yellow!", 
        "My plant is dry!", 
        "My plant has bugs!" 
    };
    
    public int currentIssueIndex;
    public bool isShoppingMode = false;

    private TextMeshProUGUI textDisplay;

    void Awake()
    {
        // Find the text component immediately
        textDisplay = GetComponent<TextMeshProUGUI>();
    }

    public void GenerateIssue()
    {
        // 1. If we are in Shopping Mode, don't do anything
        if (isShoppingMode) return;

        // 2. Double-check that we have the text component
        if (textDisplay == null) textDisplay = GetComponent<TextMeshProUGUI>();

        // 3. Pick the issue
        currentIssueIndex = Random.Range(0, issues.Length);
        
        // 4. Update the text
        if (textDisplay != null)
        {
            textDisplay.text = issues[currentIssueIndex];
            Debug.Log("PlantHealth: Set text to " + issues[currentIssueIndex]);
        }
        else
        {
            Debug.LogError("PlantHealth: No TextMeshProUGUI component found on " + gameObject.name);
        }
    }

    void OnEnable()
    {
        // When the bubble turns on, if it's not a shopper, pick an issue
        if (!isShoppingMode)
        {
            GenerateIssue();
        }
    }
}