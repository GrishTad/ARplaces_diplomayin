using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class GetLocation : MonoBehaviour {
	public Text singleText;
	public static float lat;
	public static float lon;
	public static float alt;
	public static float hrAc;
	public static float vrAc;
	public void Start()
	{

		// turn on location services, if available 
		Input.location.Start(10,0.1f);

       StartCoroutine( GetCurrentLocation());

    }

	public void Update()
	{

    }

    public IEnumerator GetCurrentLocation()
    {
        yield return new WaitForSeconds(1);
        //Do nothing if location services are not available
        if (Input.location.isEnabledByUser)
        {
            lat = Input.location.lastData.latitude;
            lon = Input.location.lastData.longitude;
            alt = Input.location.lastData.altitude;
            hrAc = Input.location.lastData.horizontalAccuracy;
            vrAc = Input.location.lastData.verticalAccuracy;


            //singleText.text = "Depart lat: " + lat + ", lon: " + lon + ", alt: " + alt + ", hrAc: " + hrAc + ", vrAc :" + vrAc;

            if (lat == 0 && lon == 0)
            {
                StartCoroutine(GetCurrentLocation());
            }
            else
            {
                getLocationJson.instance.SetLocation(lat, lon, 100, "null");
                //CompassController.instance.nearintsObjectName.text =lat+" "+lon;
                Debug.Log("LocationInfo found");
            }
        }
        else
        {
#if UNITY_EDITOR
            getLocationJson.instance.SetLocation(lat, lon, 100, "null");
#else
            CompassController.instance.nearintsObjectName.text = "turn on gps";
            singleText.text = "gps off";
            Debug.Log("GPSOFF");
            StartCoroutine(GetCurrentLocation());
#endif

        }
    }


}