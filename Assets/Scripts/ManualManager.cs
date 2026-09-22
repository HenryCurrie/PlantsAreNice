using UnityEngine;

public class ManualManager : MonoBehaviour
{
    public static ManualManager Instance;
    public GameObject manualUIPanel;
    private CustomerAI currentCustomer;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void OpenManual(CustomerAI customer)
    {
        currentCustomer = customer;
        manualUIPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void SubmitAnswer(int problemID)
    {
        if (currentCustomer != null) currentCustomer.AnswerReceived(problemID);
        CloseManual();
    }

    public void CloseManual()
    {
        manualUIPanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}