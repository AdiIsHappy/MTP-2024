using UnityEngine;
using UnityEngine.UI; // For UI elements

public class EarthquakeForceApplier : MonoBehaviour
{
    public Rigidbody rb; // The Rigidbody to which the forces will be applied
    public EqDataContainer eqDataContainer; // The earthquake data container ScriptableObject

    private int currentIndex = 0; // The current index in the earthquake data list
    private float elapsedTime = 0f; // Track time since the start
    public bool earthquakeActive = false; // Toggle earthquake on and off

    [Range(0f, 5f)]
    public float intensity = 1f;

    [Range(0f, 5f)]
    public float frequency = 1f;

    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        if (eqDataContainer == null || eqDataContainer.earthquakeDataList.Count == 0)
        {
            Debug.LogError("EqDataContainer is not set or is empty.");
            enabled = false;
            return;
        }

       
    }

    void FixedUpdate()
    {
        if (earthquakeActive)
        {
            // Increment the elapsed time
            elapsedTime += Time.fixedDeltaTime;

            // Apply force at specific time intervals based on the data
            while (currentIndex < eqDataContainer.earthquakeDataList.Count && elapsedTime * frequency>= float.Parse(eqDataContainer.earthquakeDataList[currentIndex].time)-27f)
            {
                // Get the earthquake data for the current index
                EarthquakeData currentData = eqDataContainer.earthquakeDataList[currentIndex];

                // Create a force vector from xAcc and yAcc
                Vector3 force = new Vector3(currentData.xAcc, 0f, currentData.yAcc); // Z-axis used instead of Y for 3D movement
                Debug.Log(force);

                // Apply the force to the Rigidbody using ForceMode.Acceleration
                rb.AddForce(force * intensity, ForceMode.Acceleration);

                // Move to the next data point
                currentIndex++;
            }
        }
    }

    // Toggle the earthquake on and off
    public void ToggleEarthquake()
    {
        earthquakeActive = !earthquakeActive;

        if (earthquakeActive)
        {
            ResetEarthquake();
        }

        
    }

    // Update the button label to reflect the current state


    // Reset the earthquake data to start over
    private void ResetEarthquake()
    {
        currentIndex = 0;
        elapsedTime = 0f;
    }
}
