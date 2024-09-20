using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class GeolocationHandler : MonoBehaviour
{
    public TMPro.TMP_Text statusTxt;
    private Coroutine gpsCoroutine;
    private float updateInterval = 5f; // Update every 5 seconds
    private float timer;

    private void Start()
    {
        StartCoroutine(UpdateGPSLocation());
    }

    void Update()
    {

    }


    private IEnumerator UpdateGPSLocation()
    {
        // Check if the user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("Location services not enabled");
            yield break;
        }

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            yield break;
        }

        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            yield break;
        }

        // If we reach here, we have successfully initialized the location service
        // Now we can start getting location updates
        while (true)
        {
            if (Input.location.status == LocationServiceStatus.Running)
            {
                statusTxt.text = $"Location: {Input.location.lastData.latitude:F6}, {Input.location.lastData.longitude:F6}";
            }
            else
            {
                statusTxt.text = "Location service not running";
            }

            // Wait for 1 second before the next update
            yield return new WaitForSeconds(1f);
        }
    }

    private void OnDisable()
    {
        Input.location.Stop();
    }
}