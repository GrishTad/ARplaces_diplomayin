using UnityEngine; 
using System.Collections; 
using UnityEngine.UI; 
using System.Collections.Generic;
[System.Serializable]
public class getLocationJson : MonoBehaviour {

    public static getLocationJson instance;
	public string json;
	public static string tex;
    public string locationJson;
    public float latitude;
    public float longitude;
    public int radius;
    public string type;
    public jsonData data;
    public GameObject Directions;


    public Text texts;
    // Use this for initialization
    private void Awake()
    {
        instance = this;
    }

    void Start()
	{
       

	}

    public void SetLocation(float lat, float lon, int rad, string currentType)
    {
        latitude = lat;
        longitude = lon;
        radius = rad;
        type = currentType;
        Debug.Log(latitude+" "+longitude+" "+radius+" "+type);
        StartCoroutine(GetLocationData());
    }

    public IEnumerator GetLocationData()
    {
        //type = null;
        Debug.Log("starting");
        GameObject origin = GameObject.CreatePrimitive(PrimitiveType.Cube);
        origin.name = "origin";
        origin.transform.parent = Directions.transform;
#if UNITY_EDITOR
        origin.transform.position = new Vector3(40.18114f * 2000, 0, 44.51667f   * 2000);
#else
        origin.transform.position = new Vector3(latitude * 2000, 0, longitude * 2000);
      //  CompassController.instance.nearintsObjectName.text = latitude + " " + longitude;
#endif

        origin.transform.eulerAngles = new Vector3(0, 220 ,0);
        origin.layer = 8;
        CompassController.instance.origin = origin;
#if UNITY_EDITOR
        WWW product_get = new WWW ("https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=40.18114,44.51667&radius=50&language=en&types=null&key=AIzaSyCTGuzUEOAdzCXSOjpndeVVbLk7jZ4YmuA");
#else
        WWW product_get = new WWW("https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=" + latitude + "," + longitude + "&radius=" + radius + "&language=en&types=" + type + "&key=AIzaSyCTGuzUEOAdzCXSOjpndeVVbLk7jZ4YmuA");
#endif
        yield return product_get;
        json = product_get.text;
        readJson();


    }



	[System.Serializable]
	public struct jsonObject 
	{
		public string name;
		public string icon;
		public string vicinity;
		public objGeometry geometry;
		public List<string> types;
	}
	[System.Serializable]
	public struct objGeometry
	{
		public objLocation location;

	}
	[System.Serializable]
	public struct objLocation
	{
		public float lat;
		public float lng;
	}

	[System.Serializable]
	public class jsonData 
	{
		public List<jsonObject> results;
	}
	public void readJson()
	{
		data = JsonUtility.FromJson<jsonData>(json);
		Debug.Log ("objects count:"+data.results.Count);
		//tex = data.results [0].icon;
		for (int i = 0; i < data.results.Count; i++) 
		{
			Debug.Log (data.results[i].name+" "+data.results[i].geometry.location.lat + " "+data.results[i].geometry.location.lng+" "+data.results[i].vicinity );

            texts.text += data.results[i].name+"   ";

            /*for (int j = 0; j < data.results [i].types.Count; j++) {
				Debug.Log (data.results [i].types [j]);
			}*/
        }

        for (int i = 0; i < data.results.Count; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.parent = Directions.transform;
            cube.name = data.results[i].name;
            cube.layer = 8;
            cube.transform.position = new Vector3(data.results[i].geometry.location.lat*2000, 0, data.results[i].geometry.location.lng*2000);
           
            CompassController.instance.places.Add(cube);

        }
        StartCoroutine(CompassController.instance.CheckNearistObject());
        //try
        //{
        //    Debug.Log("<color=green>" + CompassController.instance.CheckObjects().name + "</color>");
        //}
        //catch
        //{
        //    Debug.Log("<color=red>Dont Have a places</color>");
        //}
		/* foreach (groceryObject tes in data.results) {
			Debug.Log (tes.name + tes.types);
		}
		*/
	}
}