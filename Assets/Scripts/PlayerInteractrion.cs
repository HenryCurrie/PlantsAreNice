using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float reachDistance = 4f;
    public LayerMask customerLayer;
    public float interactionDuration = 5f;

    [Header("UI References")]
    public Image crosshairRadialFill; // Image Type must be 'Filled', Method 'Radial 360'

    private float holdTimer = 0f;

    void Update()
    {
        if (Time.timeScale == 0)
        {
            ResetInteraction();
            return;
        }

        HandleInteractionInput();
    }

    void HandleInteractionInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, reachDistance, customerLayer))
        {
            CustomerAI customer = hit.collider.GetComponent<CustomerAI>();

            if (customer != null && customer.currentState == CustomerAI.CustomerState.WaitingAtCounter)
            {
                if (Input.GetMouseButton(0))
                {
                    holdTimer += Time.deltaTime;

                    if (crosshairRadialFill != null)
                        crosshairRadialFill.fillAmount = holdTimer / interactionDuration;

                    if (holdTimer >= interactionDuration)
                    {
                        customer.StartInteraction();
                        ResetInteraction();
                    }
                    return;
                }
            }
        }

        if (Input.GetMouseButtonUp(0) || !Physics.Raycast(ray, reachDistance, customerLayer))
        {
            ResetInteraction();
        }
    }

    void ResetInteraction()
    {
        holdTimer = 0f;
        if (crosshairRadialFill != null)
            crosshairRadialFill.fillAmount = 0f;
    }
}