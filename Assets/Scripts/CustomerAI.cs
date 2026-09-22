using UnityEngine;
using UnityEngine.AI;

public class CustomerAI : MonoBehaviour
{
    public enum CustomerState { WalkingToPlant, PickingPlant, WalkingToCounter, WaitingAtCounter, Leaving }
    public CustomerState currentState;

    [Header("Settings")]
    public bool isShopper;
    public int correctProblemID; // 0=Mold, 1=Yellow, 2=Crispy, 3=Bugs
    public float pickDuration = 2f;
    private float pickTimer = 0f;

    [Header("Advice Dialogues")]
    public string[] problemDialogues = {
        "My plant has mold!",
        "My plant is turning yellow!",
        "My plant has crispy leaves!",
        "My plant has bugs!"
    };

    [Header("Attachments")]
    public Transform handSocket; 
    public GameObject speechBubble;
    public TMPro.TextMeshProUGUI speechText;

    private NavMeshAgent agent;
    private Transform plantTarget;
    private Transform counterTarget;
    private Transform exitTarget;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        counterTarget = GameObject.FindGameObjectWithTag("Counter")?.transform;
        exitTarget = GameObject.FindGameObjectWithTag("Exit")?.transform;

        if (isShopper)
        {
            FindRandomPlant();
            SetState(CustomerState.WalkingToPlant);
        }
        else
        {
            SetState(CustomerState.WalkingToCounter);
        }
    }

    void FindRandomPlant()
    {
        GameObject[] plants = GameObject.FindGameObjectsWithTag("Plant");
        if (plants.Length > 0)
            plantTarget = plants[Random.Range(0, plants.Length)].transform;
        else
            isShopper = false;
    }

    void Update()
    {
        if (Time.timeScale == 0 || agent == null) return;

        if (currentState == CustomerState.PickingPlant)
        {
            pickTimer += Time.deltaTime;
            if (pickTimer >= pickDuration) FinishPicking();
            return;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            OnDestinationReached();
    }

    void OnDestinationReached()
    {
        switch (currentState)
        {
            case CustomerState.WalkingToPlant: StartPicking(); break;
            case CustomerState.WalkingToCounter: 
                currentState = CustomerState.WaitingAtCounter; 
                ShowSpeech(); 
                break;
            case CustomerState.Leaving: Destroy(gameObject); break;
        }
    }

    public void StartInteraction()
    {
        if (isShopper)
        {
            if (DayManager.Instance != null) DayManager.Instance.AddMoney(25f);
            SetState(CustomerState.Leaving);
        }
        else
        {
            if (ManualManager.Instance != null) ManualManager.Instance.OpenManual(this);
        }
    }

    public void AnswerReceived(int playerChoiceID)
    {
        if (playerChoiceID == correctProblemID)
        {
            if (DayManager.Instance != null) DayManager.Instance.AddMoney(15f);
            Debug.Log("Correct!");
        }
        SetState(CustomerState.Leaving);
    }

    void StartPicking()
    {
        currentState = CustomerState.PickingPlant;
        agent.isStopped = true;
        if (plantTarget != null && handSocket != null)
        {
            GameObject clone = Instantiate(plantTarget.gameObject, handSocket.position, handSocket.rotation);
            clone.transform.SetParent(handSocket);
            clone.transform.localPosition = Vector3.zero;
            clone.transform.localRotation = Quaternion.identity;
            if (clone.GetComponent<Collider>()) clone.GetComponent<Collider>().enabled = false;
        }
    }

    void FinishPicking() { agent.isStopped = false; SetState(CustomerState.WalkingToCounter); }

    void ShowSpeech()
    {
        if (speechBubble == null || speechText == null) return;
        speechBubble.SetActive(true);
        if (isShopper) speechText.text = "I'd like to buy this!";
        else speechText.text = problemDialogues[Mathf.Clamp(correctProblemID, 0, 3)];
    }

    public void SetState(CustomerState newState)
    {
        currentState = newState;
        if (agent == null) return;
        agent.isStopped = false;
        if (newState == CustomerState.Leaving && speechBubble != null) speechBubble.SetActive(false);

        switch (currentState)
        {
            case CustomerState.WalkingToPlant: agent.SetDestination(plantTarget.position); break;
            case CustomerState.WalkingToCounter: agent.SetDestination(counterTarget.position); break;
            case CustomerState.Leaving: agent.SetDestination(exitTarget.position); break;
            default: agent.isStopped = true; break;
        }
    }
}