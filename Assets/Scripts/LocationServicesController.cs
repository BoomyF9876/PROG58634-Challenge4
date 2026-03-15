using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocationServicesController : Singleton<LocationServicesController>
{
    public bool isSearching = false;

    public float latitude;
    public float longitude;

    public TMP_Text latitudeText;
    public TMP_Text longitudeText;

    public System.Action OnLocationUpdateCallback;

    void Start()
    {
        latitudeText.text = "";
        longitudeText.text = "";
        GetLocation();
    }

    public void GetLocation()
    {
        if (isSearching == false)
        {
            isSearching = true;
            StartCoroutine(UpdateLocation());
        }
    }

    private IEnumerator UpdateLocation()
    {
        Input.location.Start();

        int waitTime = 20;
        while(Input.location.status == LocationServiceStatus.Initializing && waitTime > 0)
        {
            yield return new WaitForSeconds(1);
            waitTime--;
        }

        if (waitTime <= 0)
        {
            latitudeText.text = "Timed Out";
            isSearching = false;
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed || Input.location.status == LocationServiceStatus.Stopped)
        {
            latitudeText.text = "Failed to determine device location";
            isSearching = false;
            yield break;
        }

        latitude = Input.location.lastData.latitude;
        longitude = Input.location.lastData.longitude;

        latitudeText.text = $"Latitude: {latitude}";
        longitudeText.text = $"Longitude: {longitude}";

        Input.location.Stop();
        isSearching = false;

        if (OnLocationUpdateCallback != null)
        {
            OnLocationUpdateCallback();
        }
    }
}
