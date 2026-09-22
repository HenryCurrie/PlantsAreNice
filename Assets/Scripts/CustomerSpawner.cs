using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 5f;
    private GameObject currentCustomer;
    private float timer;

    void Update()
    {
        if (currentCustomer == null)
        {
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                SpawnNewCustomer();
                timer = 0;
            }
        }
    }

    void SpawnNewCustomer()
    {
        currentCustomer = Instantiate(customerPrefab, spawnPoint.position, spawnPoint.rotation);
        CustomerAI ai = currentCustomer.GetComponent<CustomerAI>();
        
        if (ai != null)
        {
            ai.isShopper = Random.value > 0.5f;

            if (!ai.isShopper) 
            {
                ai.correctProblemID = Random.Range(0, 4);
            }
            else
            {
                ai.correctProblemID = -1;
            }
        }
    }
}